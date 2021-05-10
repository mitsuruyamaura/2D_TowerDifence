using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyControllerPrefab;

    //[SerializeField]
    //private Transform[] enemyGenerateTrans;

    //[SerializeField]
    //private PathData[] enemyPathDatas;

    private GameManager gameManager;
    private StageData stageData;



    /// <summary>
    /// �G�̐�������
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreparateEnemyGenerate(GameManager gameManager, StageData stageData) {
        this.gameManager = gameManager;
        this.stageData = stageData;

        int timer = 0;

        while (gameManager.isEnemyGenerate) {

            timer++;

            if (timer > gameManager.generateIntervalTime) {
                timer = 0;

                // �G�̐����� List �ւ̒ǉ�
                gameManager.AddEnemyList(GenerateEnemy(gameManager.generateEnemyCount));

                // �ő吶�����𒴂����琶����~
                gameManager.JudgeGenerateEnemysEnd();
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
        int posNo = stageData.enemys[generateNo].y;

        // �����ʒu�������_�����m�F
        if (stageData.enemys[generateNo].y == -1) {
            posNo = Random.Range(0, stageData.enemyPathDatas.Length);
        }

        // �G�̐���
        EnemyController enemy = Instantiate(enemyControllerPrefab, stageData.enemyPathDatas[posNo].generateTran.position, Quaternion.identity);

        // �G�̎��
        int enemyNo = stageData.enemys[generateNo].x;

        // �G�������_�����m�F
        if (stageData.enemys[generateNo].x == -1) {
            enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
        }

        // �G�̏��̐ݒ�
        StartCoroutine(enemy.SetUpEnemyController(stageData.enemyPathDatas[posNo], gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == enemyNo)));

        return enemy;
    }
}
