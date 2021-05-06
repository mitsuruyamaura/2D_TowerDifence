using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private PathData pathData;

    [Header("�ړ����x")]
    public float moveSpeed;

    private Vector3 currentPos;

    private Animator anim;

    
    void Start()
    {
        TryGetComponent(out anim);

        // �ړ�����n�_���擾
        Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // �e�n�_�Ɍ����Ĉړ�
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

        if (transform.position.x < currentPos.x) {
            anim.SetFloat("Y", 0f);
            anim.SetFloat("X", -1.0f);

            Debug.Log("������");
        } else if (transform.position.y > currentPos.y) {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", 1.0f);

            Debug.Log("�㍶��");
        } else if (transform.position.y < currentPos.y) {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", -1.0f);

            Debug.Log("������");
        } else {
            anim.SetFloat("Y", 0f);
            anim.SetFloat("X", 1.0f);

            Debug.Log("�E����");         
        }

        currentPos = transform.position;
    }
}
