using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyControllerPrefab;

    [SerializeField]
    private PathData[] pathDatas;

    [SerializeField]
    private DrawPathLine pathLinePrefab;


    //public bool isEnemyGenerate;

    //public int generateIntervalTime;

    //public int generateEnemyCount;

    //public int maxEnemyCount;

    //[SerializeField]
    //private PathData[] enemyPathDatas;

    private GameManager gameManager;


    // mi
    private StageData stageData;


    //void Start() {
    //    isEnemyGenerate = true;

    //    StartCoroutine(PreparateEnemyGenerate());    
    //}


    /// <summary>
    /// �G�̐�������
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreparateEnemyGenerate(GameManager gameManager, StageData stageData=null) {
        this.gameManager = gameManager;
        this.stageData = stageData;

        int timer = 0;

        while (gameManager.isEnemyGenerate) {

            if (this.gameManager.currentGameState == GameManager.GameState.Play) {
                timer++;

                if (timer > gameManager.generateIntervalTime) {
                    timer = 0;

                    int randomValue = Random.Range(0, pathDatas.Length);

                    EnemyController enemyController = Instantiate(enemyControllerPrefab, pathDatas[randomValue].generateTran.position, Quaternion.identity);

                    Vector3[] paths = pathDatas[randomValue].pathTranArray.Select(x => x.position).ToArray();

                    // �G�̏��̐ݒ�
                    enemyController.SetUpEnemyController(paths);

                    StartCoroutine(PreparateCreatePathLine(paths, enemyController));



                    // �G�̐����� List �ւ̒ǉ�
                    gameManager.AddEnemyList(GenerateEnemy(gameManager.generateEnemyCount));

                    gameManager.generateEnemyCount++;

                    // �ő吶�����𒴂����琶����~
                    gameManager.JudgeGenerateEnemysEnd();
                }
            }

            yield return null;
        }

        // TODO �����I��


    }


    /// <summary>
    /// �G�̐���
    /// </summary>
    public EnemyController GenerateEnemy(int generateNo) {

        //int randomValue = Random.Range(0, enemyGenerateTrans.Length);
        //EnemyController enemy = Instantiate(enemyControllerPrefab, enemyGenerateTrans[randomValue].position, Quaternion.identity);
        //StartCoroutine(enemy.SetUpEnemyController(enemyPathDatas[randomValue], gameManager, stageData.enemyPathDatas[posNo] Random.Range(0, enemyDataSO.enemyDatasList.Count))));

        // �����ʒu
        int posNo = stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.y;

        // �����ʒu�������_�����m�F
        if (stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.y == -1) {
            posNo = Random.Range(0, stageData.mapInfo.appearEnemyInfos.Length);
        }

        // �G�̐���
        EnemyController enemyController = Instantiate(enemyControllerPrefab, stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.generateTran.position, Quaternion.identity);

        // �G�̎��
        int enemyNo = stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.x;

        // �G�������_�����m�F
        if (stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.x == -1) {
            enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
        }

        // �o�H�̍쐬
        Vector3[] paths = stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.pathTranArray.Select(x => x.position).ToArray();

        // �G�̏��̐ݒ�
        enemyController.SetUpEnemyController(paths, DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == enemyNo));

        StartCoroutine(PreparateCreatePathLine(paths, enemyController));

        enemyController.ResumeMove();

        return enemyController;
    }

    /// <summary>
    /// ���C�������̏���
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    private IEnumerator PreparateCreatePathLine(Vector3[] paths, EnemyController enemyController) {
        yield return StartCoroutine(CreatePathLine(paths));

        enemyController.ResumeMove();
    }

    /// <summary>
    /// �o�H�̐����Ɣj��
    /// </summary>
    private IEnumerator CreatePathLine(Vector3[] paths) {

        List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>();

        // �P�� Path ���ƂɂP�����ԂɌo�H�𐶐�
        for (int i = 0; i < paths.Length - 1; i++) {
            DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

            Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

            drawPathLine.CreatePathLine(drawPaths);

            drawPathLinesList.Add(drawPathLine);

            yield return new WaitForSeconds(0.1f);
        }

        // ���ׂẴ��C���𐶐����đҋ@
        yield return new WaitForSeconds(0.5f);

        // �P�̃��C�������Ԃɍ폜����
        for (int i = 0; i < drawPathLinesList.Count; i++) {
            Destroy(drawPathLinesList[i].gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// �X�e�[�W�ɉ����� PathDatas ���Z�b�g
    /// </summary>
    public void SetUpPathDatas(PathData[] pathDatas) {
 
        // ���������đ��
        this.pathDatas = new PathData[pathDatas.Length];
        this.pathDatas = pathDatas;
    }
}
