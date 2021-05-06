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
    /// �G�̐i�s�������擾
    /// </summary>
    private void ChangeAnimeDirection() {
        // TODO Dubug.Log �����ɃA�j���J�ڂ�����

        if (transform.position.x < currentPos.x) {
            Debug.Log("������");
        } else if (transform.position.y > currentPos.y) {
            Debug.Log("�㍶��");
        } else if (transform.position.y < currentPos.y) {
            Debug.Log("������");
        } else {
            //if (transform.position.x > currentPos.x) {
                Debug.Log("�E����");
            //} else
        }

        currentPos = transform.position;
    }
}
