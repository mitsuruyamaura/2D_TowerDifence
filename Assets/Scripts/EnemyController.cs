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


    [Header("移動速度")]
    public float moveSpeed;

    [Header("HP")]
    public int maxHp;

    [SerializeField]
    private int hp;

    private Vector3 currentPos;

    private Animator anim;

    private Tween tween;
    
    void Start()
    {
        hp = maxHp;

        TryGetComponent(out anim);

        // 移動する地点を取得
        Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // 各地点に向けて移動
        tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);
    }


    void Update()
    {
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
            DestroyEnemy();
        }

        // TODO 演出用のエフェクト生成
        CreateHitEffect();

        // ヒットストップ演出
        StartCoroutine(WaitMove());        
    }

    /// <summary>
    /// 敵破壊処理
    /// </summary>
    private void DestroyEnemy() {
        tween.Kill();

        // TODO SE


        GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect,1.5f);
        Destroy(gameObject);
    }

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
}
