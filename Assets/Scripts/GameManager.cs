using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isEnemyGenerate;

    public int generateIntervalTime;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    private int generateEnemyCount;
    private int destroyEnemyCount;

    public int maxEnemyCount;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private DefenseBase defenseBase;

    [SerializeField]
    private CharaGenerator charaGenerator;

    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState {
        Wait,
        Play,
        GameUp
    }

    public GameState currentGameState;

    
    void Start()
    {
        currentGameState = GameState.Wait;

        StartCoroutine(charaGenerator.SetUpCharaGenerator(this));

        defenseBase.SetUpDefenseBase(this);

        isEnemyGenerate = true;

        currentGameState = GameState.Play;

        // 敵の生成準備
        StartCoroutine(SetUpEnemyGenerate());
    }

    /// <summary>
    /// 敵の生成準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetUpEnemyGenerate() {

        int timer = 0;

        while (isEnemyGenerate) {

            timer++;

            if (timer > generateIntervalTime) {
                timer = 0;

                // 敵の生成と List への追加
                enemiesList.Add(enemyGenerator.GenerateEnemy(this));
                generateEnemyCount++;

                // 最大生成数を超えたら
                if (generateEnemyCount >= maxEnemyCount) {
                    isEnemyGenerate = false;
                }
            }

            yield return null;
        }

        // TODO 生成終了


    }

    /// <summary>
    /// 敵の情報を List から削除
    /// </summary>
    /// <param name="removeEnemy"></param>
    public void RemoveEnemyList(EnemyController removeEnemy) {
        enemiesList.Remove(removeEnemy);
    }

    /// <summary>
    /// 破壊した敵の数をカウント
    /// </summary>
    public void CountUpDestoryEnemyCount() {
        destroyEnemyCount++;

        // ゲームクリア判定
        JudgeGameClear();
    }

    /// <summary>
    /// ゲームクリア判定
    /// </summary>
    public void JudgeGameClear() {
        // 生成数を超えているか
        if (destroyEnemyCount >= generateEnemyCount) {

            Debug.Log("ゲームクリア");

            GameUp();

            uiManager.CreateGameClearSet();

            // TODO ゲームクリアの処理を追加
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver() {

        GameUp();

        // 表示
        uiManager.CreateGameOverSet();

        // TODO ゲームオーバーの処理を追加
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    private void GameUp() {

        currentGameState = GameState.GameUp;

        // キャラ配置用のポップアップが開いている場合には破棄
        charaGenerator.DestroyPlacementCharaSelectPopUp();

        // TODO ゲーム終了時に行う処理を追加

    }
}
