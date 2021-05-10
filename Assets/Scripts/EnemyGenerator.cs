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
    /// 敵の生成準備
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

                // 敵の生成と List への追加
                gameManager.AddEnemyList(GenerateEnemy(gameManager.generateEnemyCount));

                // 最大生成数を超えたら生成停止
                gameManager.JudgeGenerateEnemysEnd();
            }

            yield return null;
        }

        // TODO 生成終了


    }


    /// <summary>
    /// 敵の生成
    /// </summary>
    public EnemyController GenerateEnemy(int generateNo) {

        //int randomValue = Random.Range(0, enemyGenerateTrans.Length);
        //EnemyController enemy = Instantiate(enemyControllerPrefab, enemyGenerateTrans[randomValue].position, Quaternion.identity);
        //StartCoroutine(enemy.SetUpEnemyController(enemyPathDatas[randomValue], gameManager, stageData.enemyPathDatas[posNo] Random.Range(0, enemyDataSO.enemyDatasList.Count))));

        // 生成位置
        int posNo = stageData.enemys[generateNo].y;

        // 生成位置がランダムか確認
        if (stageData.enemys[generateNo].y == -1) {
            posNo = Random.Range(0, stageData.enemyPathDatas.Length);
        }

        // 敵の生成
        EnemyController enemy = Instantiate(enemyControllerPrefab, stageData.enemyPathDatas[posNo].generateTran.position, Quaternion.identity);

        // 敵の種類
        int enemyNo = stageData.enemys[generateNo].x;

        // 敵がランダムか確認
        if (stageData.enemys[generateNo].x == -1) {
            enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
        }

        // 敵の情報の設定
        StartCoroutine(enemy.SetUpEnemyController(stageData.enemyPathDatas[posNo], gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == enemyNo)));

        return enemy;
    }
}
