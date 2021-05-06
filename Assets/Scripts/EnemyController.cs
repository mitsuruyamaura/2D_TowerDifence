using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private PathData pathData;

    public float moveSpeed;

    private Vector3 currentPos;

    
    void Start()
    {
        Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

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
        // TODO Dubug.Log 部分にアニメ遷移を入れる

        if (transform.position.x < currentPos.x) {
            Debug.Log("左方向");
        } else if (transform.position.y > currentPos.y) {
            Debug.Log("上左向");
        } else if (transform.position.y < currentPos.y) {
            Debug.Log("下方向");
        } else {
            //if (transform.position.x > currentPos.x) {
                Debug.Log("右方向");
            //} else
        }

        currentPos = transform.position;
    }
}
