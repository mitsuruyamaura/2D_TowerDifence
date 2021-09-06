using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ��o�H�̏��")]
    private PathData pathData;

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�ő�HP")]
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


    // ��
    [SerializeField, HideInInspector]
    private GameObject hitEffectPrefab;        // EffectManager �őΉ�

    [SerializeField, HideInInspector]
    private GameObject destroyEffectPrefab;    // EffectManager �őΉ�


    //IEnumerator Start() {
    //    hp = maxHp;

    //    TryGetComponent(out anim);

    //    //Vector3[]  paths = new Vector3[pathData.pathTranArray.Length];

    //    //for (int i = 0; i < pathData.pathTranArray.Length; i++) {
    //    //    paths[i] = pathData.pathTranArray[i].position;
    //    //}

    //    // �ړ�����n�_���擾
    //    paths = pathData.pathTranArray.Select(x => x.position).ToArray();

    //    // �o�H����
    //    //yield return StartCoroutine(CreatePathLine(paths));

    //    // �e�n�_�Ɍ����Ĉړ�
    //    tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear).OnWaypointChange(ChangeAnimeDirection);

    //    yield return null;
    //}

    /// <summary>
    /// �G�̐ݒ�
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

        // �ړ�����n�_���擾
        //paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        paths = pathsData;

        // �o�H����
        //yield return StartCoroutine(CreatePathLine(paths));

        // �e�n�_�Ɍ����Ĉړ�
        tween = transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear).OnWaypointChange(ChangeAnimeDirection);

        PauseMove();

        // �Q�[����~���Ȃ�ړ����~�߂�
        //if (gameManager.currentGameState == GameManager.GameState.Stop) {
        //    PauseMove();
        //}
    }


    //void Update()
    //{
    //    // �G�̐i�s�������擾
    //    ChangeAnimeDirection();
    //}

    /// <summary>
    /// �G�̐i�s�������擾���āA�ړ��A�j���Ɠ���
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

        //    Debug.Log("������");
        //} else if (transform.position.y < paths[index].y) {
        //    anim.SetFloat("X", 0f);
        //    anim.SetFloat("Y", 1.0f);

        //    Debug.Log("�㍶��");
        //} else if (transform.position.y > paths[index].y) {
        //    anim.SetFloat("X", 0f);
        //    anim.SetFloat("Y", -1.0f);

        //    Debug.Log("������");
        //} else {
        //    anim.SetFloat("Y", 0f);
        //    anim.SetFloat("X", 1.0f);

        //    Debug.Log("�E����");         
        //}

        //currentPos = transform.position;
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
            DestroyEnemy(true);
        } else {

            // ���o�p�̃G�t�F�N�g����
            CreateHitEffect();

            // �q�b�g�X�g�b�v���o
            StartCoroutine(WaitMove());
        }
    }

    /// <summary>
    /// �G�j�󏈗�
    /// </summary>
    public void DestroyEnemy(bool isPlayerDestroyed = true) {   // �����܂��g���ĂȂ��̂ŃT�C�g�ɂ͋L�ڂ��Ă��Ȃ�
        tween.Kill();

        // �v���C���[���j�󂵂��ꍇ(�h�q���_�ɐN�������ꍇ�ɂ́A�h�q���_�̃G�t�F�N�g���o���B���������ƌ����Ȃ��Ȃ邽��)
        if (isPlayerDestroyed) {
            // TODO SE

            // �G�t�F�N�g
            //GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Destroy_Enemy), transform.position, Quaternion.identity);
            Destroy(effect, 1.5f);
        }

        //// Enemy �� List ����폜  => CountUpDestroyEnemyCount �̒��ł��̂ŕs�v
        //gameManager.RemoveEnemyList(this);

        // �v���C���[���j�󂵂Ă���ꍇ  =>  �����Ȃ��悤�ɂ����̂ŕs�v
        //if (isPlayerDestroyed) {
        // �|�����G�̐����J�E���g(Enemy �� List ����폜)
        gameManager.CountUpDestoryEnemyCount(this);
        //}

        Destroy(gameObject);
    }

    /// <summary>
    /// �q�b�g�G�t�F�N�g����
    /// </summary>
    private void CreateHitEffect() {
        // TODO SE


        //GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Hit_Enemy) , transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        
    }

    ///// <summary>
    ///// �o�H�̐����Ɣj��
    ///// </summary>
    //private IEnumerator CreatePathLine(Vector3[] paths) {

    //    yield return null;

    //    List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>(); 

    //    // �P�� Path ���ƂɂP�����ԂɌo�H�𐶐�
    //    for (int i = 0; i < paths.Length -1; i++) {
    //        DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

    //        Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

    //        drawPathLine.CreatePathLine(drawPaths);

    //        drawPathLinesList.Add(drawPathLine);

    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    // ���ׂẴ��C���𐶐����đҋ@
    //    yield return new WaitForSeconds(0.5f);

    //    // �P�̃��C�������Ԃɍ폜����
    //    for (int i = 0; i < drawPathLinesList.Count;i++) {
    //        Destroy(drawPathLinesList[i].gameObject);

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    /// <summary>
    /// �ړ����ꎞ��~
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// �ړ��ĊJ
    /// </summary>
    public void ResumeMove() {
        tween.Play();
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
    /// AnimatorController �� AnimatorOverrideController �ŕύX
    /// </summary>
    private void SetUpAnimation() {
        if (enemyData.enemyOverrideController != null) {
            anim.runtimeAnimatorController = enemyData.enemyOverrideController;   // enemyData.overrideController.runtimeAnimatorController ���ƃ_��
        }
    }
}
