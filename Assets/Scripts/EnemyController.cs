using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動経路の情報")]
    private PathData pathData;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("最大HP")]
    private int maxHp;

    [SerializeField]
    private int hp;

    private Tween tween;
    private Vector3[] paths;

    private Animator anim;

    //private Vector3 currentPos;

    private GameManager gameManager;

    public int attackPower;

    public EnemyData enemyData;


    // 未
    [SerializeField, HideInInspector]
    private GameObject hitEffectPrefab;        // EffectManager で対応

    [SerializeField, HideInInspector]
    private GameObject destroyEffectPrefab;    // EffectManager で対応


    //IEnumerator Start() {
    //    hp = maxHp;

    //    TryGetComponent(out anim);

    //    //Vector3[]  paths = new Vector3[pathData.pathTranArray.Length];

    //    //for (int i = 0; i < pathData.pathTranArray.Length; i++) {
    //    //    paths[i] = pathData.pathTranArray[i].position;
    //    //}

    //    // 移動する地点を取得
    //    paths = pathData.pathTranArray.Select(x => x.position).ToArray();

    //    // 経路生成
    //    //yield return StartCoroutine(CreatePathLine(paths));

    //    // 各地点に向けて移動
    //    tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear).OnWaypointChange(ChangeAnimeDirection);

    //    yield return null;
    //}

    /// <summary>
    /// 敵の設定
    /// </summary>
    public void SetUpEnemyController(Vector3[] pathsData, GameManager gameManager, EnemyData enemyData = null) {
        this.enemyData = enemyData;

        moveSpeed = this.enemyData.moveSpeed;
        attackPower = this.enemyData.attackPower;

        maxHp = this.enemyData.hp;

        this.gameManager = gameManager;

        hp = maxHp;

        if (TryGetComponent(out anim)) {
            SetUpAnimation();
        }

        // 移動する地点を取得
        //paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        paths = pathsData;

        // 経路生成
        //yield return StartCoroutine(CreatePathLine(paths));

        // 各地点に向けて移動
        tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear).OnWaypointChange(ChangeAnimeDirection);

        PauseMove();

        // ゲーム停止中なら移動を止める
        //if (gameManager.currentGameState == GameManager.GameState.Stop) {
        //    PauseMove();
        //}
    }


    //void Update()
    //{
    //    // 敵の進行方向を取得
    //    ChangeAnimeDirection();
    //}

    /// <summary>
    /// 敵の進行方向を取得して、移動アニメと同期
    /// </summary>
    private void ChangeAnimeDirection(int index = 0) {

        //Debug.Log(index);

        if (index >= paths.Length) {
            return;
        }

        Vector3 direction = (transform.position - paths[index]).normalized;
        //Debug.Log(direction);

        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);


        //if (transform.position.x > paths[index].x) {
        //    anim.SetFloat("Y", 0f);
        //    anim.SetFloat("X", -1.0f);

        //    Debug.Log("左方向");
        //} else if (transform.position.y < paths[index].y) {
        //    anim.SetFloat("X", 0f);
        //    anim.SetFloat("Y", 1.0f);

        //    Debug.Log("上左向");
        //} else if (transform.position.y > paths[index].y) {
        //    anim.SetFloat("X", 0f);
        //    anim.SetFloat("Y", -1.0f);

        //    Debug.Log("下方向");
        //} else {
        //    anim.SetFloat("Y", 0f);
        //    anim.SetFloat("X", 1.0f);

        //    Debug.Log("右方向");         
        //}

        //currentPos = transform.position;
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
        } else {

            // 演出用のエフェクト生成
            CreateHitEffect();

            // ヒットストップ演出
            StartCoroutine(WaitMove());
        }
    }

    /// <summary>
    /// 敵破壊処理
    /// </summary>
    public void DestroyEnemy(bool isPlayerDestroyed = true) {   // 引数まだ使ってないのでサイトには記載していない
        tween.Kill();

        // プレイヤーが破壊した場合(防衛拠点に侵入した場合には、防衛拠点のエフェクトを出す。両方だすと見えなくなるため)
        if (isPlayerDestroyed) {
            // TODO SE

            // エフェクト
            //GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Destroy_Enemy), transform.position, Quaternion.identity);
            Destroy(effect, 1.5f);
        }

        //// Enemy の List から削除  => CountUpDestroyEnemyCount の中でやるので不要
        //gameManager.RemoveEnemyList(this);

        // プレイヤーが破壊している場合  =>  分けないようにしたので不要
        //if (isPlayerDestroyed) {
        // 倒した敵の数をカウント(Enemy の List から削除)
        gameManager.CountUpDestoryEnemyCount(this);
        //}

        Destroy(gameObject);
    }

    /// <summary>
    /// ヒットエフェクト生成
    /// </summary>
    private void CreateHitEffect() {
        // TODO SE


        //GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Hit_Enemy) , transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        
    }

    ///// <summary>
    ///// 経路の生成と破棄
    ///// </summary>
    //private IEnumerator CreatePathLine(Vector3[] paths) {

    //    yield return null;

    //    List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>(); 

    //    // １つの Path ごとに１つずつ順番に経路を生成
    //    for (int i = 0; i < paths.Length -1; i++) {
    //        DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

    //        Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

    //        drawPathLine.CreatePathLine(drawPaths);

    //        drawPathLinesList.Add(drawPathLine);

    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    // すべてのラインを生成して待機
    //    yield return new WaitForSeconds(0.5f);

    //    // １つのラインずつ順番に削除する
    //    for (int i = 0; i < drawPathLinesList.Count;i++) {
    //        Destroy(drawPathLinesList[i].gameObject);

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    /// <summary>
    /// 移動を一時停止
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// 移動再開
    /// </summary>
    public void ResumeMove() {
        tween.Play();
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
    /// AnimatorController を AnimatorOverrideController で変更
    /// </summary>
    private void SetUpAnimation() {
        if (enemyData.enemyOverrideController != null) {
            anim.runtimeAnimatorController = enemyData.enemyOverrideController;   // enemyData.overrideController.runtimeAnimatorController だとダメ
        }
    }
}
