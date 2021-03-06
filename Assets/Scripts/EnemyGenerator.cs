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

    private StageData stageData;


    //void Start() {
    //    isEnemyGenerate = true;

    //    StartCoroutine(PreparateEnemyGenerate());    
    //}


    /// <summary>
    /// 敵の生成準備
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

                    //int randomValue = Random.Range(0, pathDatas.Length);

                    //EnemyController enemyController = Instantiate(enemyControllerPrefab, pathDatas[randomValue].generateTran.position, Quaternion.identity);

                    //Vector3[] paths = pathDatas[randomValue].pathTranArray.Select(x => x.position).ToArray();

                    //// 敵の情報の設定
                    //enemyController.SetUpEnemyController(paths);

                    //StartCoroutine(PreparateCreatePathLine(paths, enemyController));



                    // 敵の生成と List への追加
                    gameManager.AddEnemyList(GenerateEnemy(gameManager.generateEnemyCount));

                    // 最大生成数を超えたら生成停止
                    gameManager.JudgeGenerateEnemysEnd();
                }
            }

            yield return null;
        }

        // TODO 生成終了


    }


    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="generateNo"></param>
    /// <returns></returns>
    public EnemyController GenerateEnemy(int generateNo) {

        //int randomValue = Random.Range(0, enemyGenerateTrans.Length);
        //EnemyController enemy = Instantiate(enemyControllerPrefab, enemyGenerateTrans[randomValue].position, Quaternion.identity);
        //StartCoroutine(enemy.SetUpEnemyController(enemyPathDatas[randomValue], gameManager, stageData.enemyPathDatas[posNo] Random.Range(0, enemyDataSO.enemyDatasList.Count))));

        // 生成位置(基本的には Element の番号と同じ。-1 の場合はランダム)
        int posNo = generateNo;

        // 生成位置がランダムか確認
        if (stageData.mapInfo.appearEnemyInfos[generateNo].isRandomPos) {
            posNo = Random.Range(0, stageData.mapInfo.appearEnemyInfos.Length);
        }

        // 敵の生成
        EnemyController enemyController = Instantiate(enemyControllerPrefab, stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.generateTran.position, Quaternion.identity);

        // 敵の種類
        int enemyNo = stageData.mapInfo.appearEnemyInfos[generateNo].enemyNo;

        // 敵がランダムか確認
        if (stageData.mapInfo.appearEnemyInfos[generateNo].enemyNo == -1) {
            enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
        }

        // 経路の作成
        Vector3[] paths = stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.pathTranArray.Select(x => x.position).ToArray();

        // 敵の情報の設定
        enemyController.SetUpEnemyController(paths, gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == enemyNo));

        StartCoroutine(PreparateCreatePathLine(paths, enemyController));

        return enemyController;
    }

    /// <summary>
    /// ライン生成の準備
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    private IEnumerator PreparateCreatePathLine(Vector3[] paths, EnemyController enemyController) {
        yield return StartCoroutine(CreatePathLine(paths));

        yield return new WaitUntil(() => gameManager.currentGameState == GameManager.GameState.Play);
        
        enemyController.ResumeMove();
    }

    /// <summary>
    /// 経路の生成と破棄
    /// </summary>
    private IEnumerator CreatePathLine(Vector3[] paths) {

        List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>();

        // １つの Path ごとに１つずつ順番に経路を生成
        for (int i = 0; i < paths.Length - 1; i++) {
            DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

            Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

            drawPathLine.CreatePathLine(drawPaths);

            drawPathLinesList.Add(drawPathLine);

            yield return new WaitForSeconds(0.1f);
        }

        // すべてのラインを生成して待機
        yield return new WaitForSeconds(0.5f);

        // １つのラインずつ順番に削除する
        for (int i = 0; i < drawPathLinesList.Count; i++) {
            Destroy(drawPathLinesList[i].gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// ステージに応じた PathDatas をセット
    /// </summary>
    public void SetUpPathDatas(PathData[] pathDatas) {
 
        // 初期化して代入
        this.pathDatas = new PathData[pathDatas.Length];
        this.pathDatas = pathDatas;
    }
}
