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


    [Header("�ړ����x")]
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

        // �ړ�����n�_���擾
        Vector3[] paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // �e�n�_�Ɍ����Ĉړ�
        tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);

        // �o�H����
        StartCoroutine(CreatePathLine(paths));
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

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="amount"></param>
    public void CulcDamage(int amount) {

        hp = Mathf.Clamp(hp -= amount, 0, maxHp);

        Debug.Log("�c��HP : " + hp);



        if (hp <= 0) {

            // �j��
            DestroyEnemy();
        }

        // TODO ���o�p�̃G�t�F�N�g����
        CreateHitEffect();

        // �q�b�g�X�g�b�v���o
        StartCoroutine(WaitMove());        
    }

    /// <summary>
    /// �G�j�󏈗�
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
    /// �q�b�g�X�g�b�v���o
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitMove() {

        tween.timeScale = 0.05f;

        yield return new WaitForSeconds(0.5f);

        tween.timeScale = 1.0f;
    }

    /// <summary>
    /// �o�H�𐶐�
    /// </summary>
    private IEnumerator CreatePathLine(Vector3[] paths) {

        yield return null;

        List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>(); 

        // �P�� Path ���ƂɂP�����ԂɌo�H�𐶐�
        for (int i = 0; i < paths.Length -1; i++) {
            DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

            Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

            drawPathLine.CreatePathLine(drawPaths);

            drawPathLinesList.Add(drawPathLine);

            yield return new WaitForSeconds(0.1f);
        }

        // ���ׂẴ��C���𐶐����đҋ@
        yield return new WaitForSeconds(0.5f);

        // �P�̃��C�������Ԃɍ폜����
        for (int i = 0; i < drawPathLinesList.Count;i++) {
            Destroy(drawPathLinesList[i].gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
