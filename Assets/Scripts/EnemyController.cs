using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private PathData pathData;

    [Header("移動速度")]
    public float moveSpeed;

    private Vector3 currentPos;

    private Animator anim;

    
    void Start()
    {
        TryGetComponent(out anim);

        // 移動する地点を取得
        Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // 各地点に向けて移動
        transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);
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
}
