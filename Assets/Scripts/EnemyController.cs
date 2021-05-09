using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private PathData pathData;

    [SerializeField]
    private GameObject hitEffectPrefab;

    [SerializeField]
    private GameObject destroyEffectPrefab;

    [SerializeField]
    private DrawPathLine pathLinePrefab;

    public int attackPower;

    [Header("移動速度")]
    public float moveSpeed;

    [Header("HP")]
    public int maxHp;

    [SerializeField]
    private int hp;

    private Vector3 currentPos;

    private Animator anim;

    private Tween tween;

    private GameManager gameManager;

    public EnemyData enemyData;

    
    //IEnumerator  Start()
    //{
    //    hp = maxHp;

    //    TryGetComponent(out anim);

    //    // 移動する地点を取得
    //    Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

    //    // 経路生成
    //    yield return StartCoroutine(CreatePathLine(paths));

    //    // 各地点に向けて移動
    //    tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);
    //}

    /// <summary>
    /// 敵の設定
    /// </summary>
    /// <returns></returns>
    public IEnumerator SetUpEnemyController(PathData pathData, GameManager gameManager, EnemyData enemyData) {
        this.enemyData = enemyData;

        moveSpeed = this.enemyData.moveSpeed;
        attackPower = this.enemyData.attackPower;

        maxHp = this.enemyData.hp;
        hp = maxHp;

        TryGetComponent(out anim);

        this.pathData = pathData;
        this.gameManager = gameManager;

        // 移動する地点を取得
        Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // 経路生成
        yield return StartCoroutine(CreatePathLine(paths));

        // 各地点に向けて移動
        tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);
    }


    void Update()
    {
        // 敵の進行方向を取得
        ChangeAnimeDirection();
    }

    /// <summary>
    /// 敵の進行方向を取得
    /// </summary>
    private void ChangeAnimeDirection() {

        if (transform.position.x < currentPos.x) {
            anim.SetFloat("Y", 0f);
            anim.SetFloat("X", -1.0f);

            Debug.Log("左方向");
        } else if (transform.position.y > currentPos.y) {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", 1.0f);

            Debug.Log("上左向");
        } else if (transform.position.y < currentPos.y) {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", -1.0f);

            Debug.Log("下方向");
        } else {
            anim.SetFloat("Y", 0f);
            anim.SetFloat("X", 1.0f);

            Debug.Log("右方向");         
        }

        currentPos = transform.position;
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="amount"></param>
    public void CulcDamage(int amount) {

        hp = Mathf.Clamp(hp -= amount, 0, maxHp);

        Debug.Log("残りHP : " + hp);

        if (hp <= 0) {

            // 破壊
            DestroyEnemy(true);
        }

        // 演出用のエフェクト生成
        CreateHitEffect();

        // ヒットストップ演出
        StartCoroutine(WaitMove());        
    }

    /// <summary>
    /// 敵破壊処理
    /// </summary>
    public void DestroyEnemy(bool isPlayerDestroyed) {
        tween.Kill();

        // TODO SE


        GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect,1.5f);

        // TODO Enemy の List から削除
        gameManager.RemoveEnemyList(this);

        // プレイヤーが破壊している場合
        if (isPlayerDestroyed) {
            // 倒した敵の数をカウント
            gameManager.CountUpDestoryEnemyCount();
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// ヒットエフェクト生成
    /// </summary>
    private void CreateHitEffect() {
        // TODO SE


        GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        
    }

    /// <summary>
    /// ヒットストップ演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitMove() {

        tween.timeScale = 0.05f;

        yield return new WaitForSeconds(0.5f);

        tween.timeScale = 1.0f;
    }

    /// <summary>
    /// 経路の生成と破棄
    /// </summary>
    private IEnumerator CreatePathLine(Vector3[] paths) {

        yield return null;

        List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>(); 

        // １つの Path ごとに１つずつ順番に経路を生成
        for (int i = 0; i < paths.Length -1; i++) {
            DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

            Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

            drawPathLine.CreatePathLine(drawPaths);

            drawPathLinesList.Add(drawPathLine);

            yield return new WaitForSeconds(0.1f);
        }

        // すべてのラインを生成して待機
        yield return new WaitForSeconds(0.5f);

        // １つのラインずつ順番に削除する
        for (int i = 0; i < drawPathLinesList.Count;i++) {
            Destroy(drawPathLinesList[i].gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
