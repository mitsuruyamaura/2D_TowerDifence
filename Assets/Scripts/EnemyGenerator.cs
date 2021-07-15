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
    /// “G‚Ì¶¬€”õ
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

                    // “G‚Ìî•ñ‚Ìİ’è
                    enemyController.SetUpEnemyController(paths);

                    StartCoroutine(PreparateCreatePathLine(paths, enemyController));



                    // “G‚Ì¶¬‚Æ List ‚Ö‚Ì’Ç‰Á
                    gameManager.AddEnemyList(GenerateEnemy(gameManager.generateEnemyCount));

                    gameManager.generateEnemyCount++;

                    // Å‘å¶¬”‚ğ’´‚¦‚½‚ç¶¬’â~
                    gameManager.JudgeGenerateEnemysEnd();
                }
            }

            yield return null;
        }

        // TODO ¶¬I—¹


    }


    /// <summary>
    /// “G‚Ì¶¬
    /// </summary>
    public EnemyController GenerateEnemy(int generateNo) {

        //int randomValue = Random.Range(0, enemyGenerateTrans.Length);
        //EnemyController enemy = Instantiate(enemyControllerPrefab, enemyGenerateTrans[randomValue].position, Quaternion.identity);
        //StartCoroutine(enemy.SetUpEnemyController(enemyPathDatas[randomValue], gameManager, stageData.enemyPathDatas[posNo] Random.Range(0, enemyDataSO.enemyDatasList.Count))));

        // ¶¬ˆÊ’u
        int posNo = stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.y;

        // ¶¬ˆÊ’u‚ªƒ‰ƒ“ƒ_ƒ€‚©Šm”F
        if (stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.y == -1) {
            posNo = Random.Range(0, stageData.mapInfo.appearEnemyInfos.Length);
        }

        // “G‚Ì¶¬
        EnemyController enemyController = Instantiate(enemyControllerPrefab, stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.generateTran.position, Quaternion.identity);

        // “G‚Ìí—Ş
        int enemyNo = stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.x;

        // “G‚ªƒ‰ƒ“ƒ_ƒ€‚©Šm”F
        if (stageData.mapInfo.appearEnemyInfos[generateNo].enemyInfo.x == -1) {
            enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
        }

        // Œo˜H‚Ìì¬
        Vector3[] paths = stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.pathTranArray.Select(x => x.position).ToArray();

        // “G‚Ìî•ñ‚Ìİ’è
        enemyController.SetUpEnemyController(paths, DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == enemyNo));

        StartCoroutine(PreparateCreatePathLine(paths, enemyController));

        enemyController.ResumeMove();

        return enemyController;
    }

    /// <summary>
    /// ƒ‰ƒCƒ“¶¬‚Ì€”õ
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    private IEnumerator PreparateCreatePathLine(Vector3[] paths, EnemyController enemyController) {
        yield return StartCoroutine(CreatePathLine(paths));

        enemyController.ResumeMove();
    }

    /// <summary>
    /// Œo˜H‚Ì¶¬‚Æ”jŠü
    /// </summary>
    private IEnumerator CreatePathLine(Vector3[] paths) {

        List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>();

        // ‚P‚Â‚Ì Path ‚²‚Æ‚É‚P‚Â‚¸‚Â‡”Ô‚ÉŒo˜H‚ğ¶¬
        for (int i = 0; i < paths.Length - 1; i++) {
            DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

            Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

            drawPathLine.CreatePathLine(drawPaths);

            drawPathLinesList.Add(drawPathLine);

            yield return new WaitForSeconds(0.1f);
        }

        // ‚·‚×‚Ä‚Ìƒ‰ƒCƒ“‚ğ¶¬‚µ‚Ä‘Ò‹@
        yield return new WaitForSeconds(0.5f);

        // ‚P‚Â‚Ìƒ‰ƒCƒ“‚¸‚Â‡”Ô‚Éíœ‚·‚é
        for (int i = 0; i < drawPathLinesList.Count; i++) {
            Destroy(drawPathLinesList[i].gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// ƒXƒe[ƒW‚É‰‚¶‚½ PathDatas ‚ğƒZƒbƒg
    /// </summary>
    public void SetUpPathDatas(PathData[] pathDatas) {
 
        // ‰Šú‰»‚µ‚Ä‘ã“ü
        this.pathDatas = new PathData[pathDatas.Length];
        this.pathDatas = pathDatas;
    }
}
