using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();

    


    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState {
        Wait,
        Play,
        Stop,
        GameUp
    }

    public GameState currentGameState;

    
    void Start()
    {
        RefreshGameData();

        currentGameState = GameState.Wait;

        StartCoroutine(charaGenerator.SetUpCharaGenerator(this));

        defenseBase.SetUpDefenseBase(this);

        isEnemyGenerate = true;

        currentGameState = GameState.Play;

        // 敵の生成準備
        StartCoroutine(SetUpEnemyGenerate());

        // カレンシーの自動獲得
        StartCoroutine(TimeToCurrency());
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void RefreshGameData() {
        GameData.instance.charaPlacementCount = 0;

        // ゲームの度にインスタンスする
        GameData.instance.CurrencyReactiveProperty = new ReactiveProperty<int>();

        if (GameData.instance.isDebug) {
            GameData.instance.CurrencyReactiveProperty.Value = GameData.instance.maxCurrency;
        } else {
            GameData.instance.CurrencyReactiveProperty.Value = 0;
        }      
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

    /// <summary>
    /// 選択したキャラの情報を List に追加
    /// </summary>
    public void AddCharasList(CharaController chara) {
        charasList.Add(chara);
    }

    /// <summary>
    /// 選択したキャラを破棄し、情報を List から削除
    /// </summary>
    /// <param name="chara"></param>
    public void RemoveCharasList(CharaController chara) {
        Destroy(chara.gameObject);
        charasList.Remove(chara);
    }

    public void PreparateCreateReturnCharaPopUp(CharaController chara) {

        // ゲーム停止
        currentGameState = GameState.Stop;

        // 配置解除を選択するポップアップを作成
        uiManager.CreateReturnCharaPopUp(chara, this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void JudgeReturnChara(bool isReturnChara, CharaController chara) {

        // キャラの配置を解除する場合
        if (isReturnChara) {
            // 選択したキャラを破棄し、情報を List から削除
            RemoveCharasList(chara);

            // 配置数を減算
            GameData.instance.charaPlacementCount--;
        }

        // ゲーム再開
        currentGameState = GameState.Play;
    }

    /// <summary>
    /// 時間の経過に応じてカレンシーを加算
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeToCurrency() {

        int timer = 0;

        while (true) {
            timer++;

            // 規定の時間が経過し、カレンシーが最大値でなければ
            if (timer > GameData.instance.getCurrencyIntervalTime && GameData.instance.CurrencyReactiveProperty.Value < GameData.instance.maxCurrency) {
                timer = 0;

                // 最大値以下になるようにカレンシーを加算
                GameData.instance.CurrencyReactiveProperty.Value = Mathf.Clamp(GameData.instance.CurrencyReactiveProperty.Value += GameData.instance.addCurrencyPoint, 0, GameData.instance.maxCurrency);
            }

            yield return null;
        }
    }
}
