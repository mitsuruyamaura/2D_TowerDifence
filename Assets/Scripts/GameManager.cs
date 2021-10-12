using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private CharaGenerator charaGenerator;

    public bool isEnemyGenerate;

    public int generateIntervalTime;

    public int generateEnemyCount;

    public int maxEnemyCount;

    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState {
        Preparate,
        Play,
        Stop,
        GameUp
    }

    public GameState currentGameState;

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();

    private int destroyEnemyCount;

    //[SerializeField]
    private DefenseBase defenseBase;
    private DefenseBase[] defenseBases;

    [SerializeField]
    private MapInfo currentMapInfo;

    [SerializeField]
    private DefenseBase defenseBasePrefab;

    [SerializeField]
    private StageData currentStageData;


    IEnumerator Start()
    {
        // ゲームの進行状態を準備中に設定
        SetGameState(GameState.Preparate);

        // ゲームデータを初期化
        RefreshGameData();

        // ステージの設定 + ステージごとの PathData を設定
        SetUpStageData();

        // キャラ配置用ポップアップの生成と設定
        StartCoroutine(charaGenerator.SetUpCharaGenerator(this, currentStageData));

        // 拠点の設定(複数にしたので、SetUpStageData メソッド内で行う)
        //defenseBase.SetUpDefenseBase(this, currentStageData.defenseBaseDurability, uiManager);

        // オープニング演出再生
        yield return StartCoroutine(uiManager.Opening());
        
        isEnemyGenerate = true;

        // ゲームの進行状態をプレイ中に変更
        SetGameState(GameState.Play);

        // 敵の生成準備開始
        StartCoroutine(enemyGenerator.PreparateEnemyGenerate(this, currentStageData));

        // カレンシーの自動獲得処理の開始
        StartCoroutine(TimeToCurrency());
    }

    /// <summary>
    /// 敵の情報を List に追加
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyList(EnemyController enemy) {
        enemiesList.Add(enemy);
        generateEnemyCount++;
    }

    /// <summary>
    /// 敵の生成を停止するか判定
    /// </summary>
    public void JudgeGenerateEnemysEnd() {
        if (generateEnemyCount >= maxEnemyCount) {
            isEnemyGenerate = false;
        }
    }

    /// <summary>
    /// GameState の変更
    /// </summary>
    /// <param name="nextGameState"></param>
    public void SetGameState(GameState nextGameState) {
        currentGameState = nextGameState;
    }

    /// <summary>
    /// すべての敵の移動を一時停止
    /// </summary>
    public void PauseEnemies() {
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].PauseMove();
        }
    }

    /// <summary>
    /// すべての敵の移動を再開
    /// </summary>
    public void ResumeEnemies() {
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].ResumeMove();
        }
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
    public void CountUpDestoryEnemyCount(EnemyController enemyController) {
        // 敵の情報を List から削除
        RemoveEnemyList(enemyController);

        destroyEnemyCount++;

        Debug.Log("破壊した敵の数 : " + destroyEnemyCount);

        // ゲームクリア判定
        JudgeGameClear();
    }

    /// <summary>
    /// ゲームクリア判定
    /// </summary>
    public void JudgeGameClear() {

        // 防衛拠点の耐久力が 0 以下の場合
        if (GameData.instance.defenseBaseDurability <= 0) {

            // ゲームオーバー
            StartCoroutine(GameOver());
            return;
        }

        // 生成数を超えているか
        if (destroyEnemyCount >= maxEnemyCount) {

            Debug.Log("ゲームクリア");

            // ゲームクリアの処理を追加(クリア報酬)
            StartCoroutine(GameClearAndResult());
        }
    }

    /// <summary>
    /// 時間の経過に応じてカレンシーを加算
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimeToCurrency() {

        int timer = 0;

        // ゲームプレイ中のみ加算
        while (currentGameState == GameState.Play) {
            timer++;

            // 規定の時間が経過し、カレンシーが最大値でなければ
            if (timer > GameData.instance.currencyIntervalTime && GameData.instance.CurrencyReactiveProperty.Value < GameData.instance.maxCurrency) {
                timer = 0;

                // 最大値以下になるようにカレンシーを加算
                GameData.instance.CurrencyReactiveProperty.Value = Mathf.Clamp(GameData.instance.CurrencyReactiveProperty.Value += GameData.instance.addCurrencyPoint, 0, GameData.instance.maxCurrency);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 選択したキャラの情報を List に追加
    /// </summary>
    public void AddCharasList(CharaController chara) {
        charasList.Add(chara);

        // TODO キャラ数カウント
        GameData.instance.charaPlacementCount++;
    }

    /// <summary>
    /// 選択したキャラを破棄し、情報を List から削除
    /// </summary>
    /// <param name="chara"></param>
    public void RemoveCharasList(CharaController chara) {
        Destroy(chara.gameObject);
        charasList.Remove(chara);
    }

    /// <summary>
    /// 現在の配置しているキャラの数の取得
    /// </summary>
    /// <returns></returns>
    public int GetPlacementCharaCount() {
        return charasList.Count;
    }

    /// <summary>
    /// 配置解除を選択するポップアップ作成の準備
    /// </summary>
    /// <param name="chara"></param>
    public void PreparateCreateReturnCharaPopUp(CharaController chara) {

        // ゲームの進行状態をゲーム停止に変更
        SetGameState(GameState.Stop);

        // すべての敵の移動を一時停止
        PauseEnemies();

        // 配置解除を選択するポップアップを作成
        uiManager.CreateReturnCharaPopUp(chara, this);
    }

    /// <summary>
    /// 選択したキャラの配置解除
    /// </summary>
    /// <param name="isReturnChara"></param>
    /// <param name="chara"></param>
    public void JudgeReturnChara(bool isReturnChara, CharaController chara) {

        // キャラの配置を解除する場合
        if (isReturnChara) {
            // 選択したキャラを破棄し、情報を List から削除
            RemoveCharasList(chara);

            // 配置数を減算
            GameData.instance.charaPlacementCount--;
        }

        //  ゲームの進行状態をプレイ中に変更して、ゲーム再開
        SetGameState(GameState.Play);

        // すべての敵の移動を再開
        ResumeEnemies();

        // カレンシーの加算処理を再開
        StartCoroutine(TimeToCurrency());
    }

    /// <summary>
    /// ステージデータの設定
    /// </summary>
    private void SetUpStageData() {

        // GameData の stageNo から StageData を取得
        currentStageData = DataBaseManager.instance.stageDataSO.stageDatasList[GameData.instance.stageNo];
        generateIntervalTime = currentStageData.generateIntervalTime;
        maxEnemyCount = currentStageData.mapInfo.appearEnemyInfos.Length;

        // TODO 他にもあれば追加
        currentMapInfo = Instantiate(currentStageData.mapInfo);

        //defenseBase = Instantiate(defenseBasePrefab, currentMapInfo.GetDefenseBaseTran());

        // DefenseBase の位置情報を取得
        Transform[] defenseBaseTrans = currentMapInfo.GetMultipleDefenseBaseTrans();
        Debug.Log(defenseBaseTrans.Length);

        defenseBases = new DefenseBase[defenseBaseTrans.Length];

        // 複数の DefenseBase の生成(１つの場合にも対応)
        for (int i = 0; i < defenseBaseTrans.Length; i++) {
            defenseBases[i] = Instantiate(defenseBasePrefab, defenseBaseTrans[i]);
            defenseBases[i].SetUpDefenseBase(this, currentStageData.defenseBaseDurability, uiManager);
        }

        // PathDatas の設定
        PathData[] pathDatas = new PathData[currentStageData.mapInfo.appearEnemyInfos.Length];
        for (int i = 0; i < currentStageData.mapInfo.appearEnemyInfos.Length; i++) {
            pathDatas[i] = currentStageData.mapInfo.appearEnemyInfos[i].enemyPathData;
        }
        enemyGenerator.SetUpPathDatas(pathDatas);
    }

    /// <summary>
    /// ゲームクリアと報酬処理
    /// </summary>
    private IEnumerator GameClearAndResult() {

        // ゲーム終了
        GameUpToCommon();

        // TODO ゲームクリア演出(文字)
        //yield return StartCoroutine(uiManager.CreateGameClearSet());

        // ロゴで演出
        yield return StartCoroutine(uiManager.GameClear());

        // クリアボーナスの獲得
        GameData.instance.totalClearPoint += currentStageData.clearPoint;

        GameData.instance.stageNo++;

        // 未クリアである場合
        if (!GameData.instance.clearedStageNosList.Contains(GameData.instance.stageNo)) {
            // 次のステージを登録してステージシーンで表示できるようにする
            GameData.instance.clearedStageNosList.Add(GameData.instance.stageNo);
        }

        // セーブ
        GameData.instance.SetSaveData();

        // シーン遷移
        SceneStateManager.instance.PreparateNextScene(SceneType.World);
    }

    /// <summary>
    /// ゲーム終了時の共通処理
    /// </summary>
    private void GameUpToCommon() {

        // ゲームの進行状態をゲーム終了に変更
        SetGameState(GameState.GameUp);

        // キャラ配置用のポップアップが開いている場合には破棄
        charaGenerator.InactivatePlacementCharaSelectPopUp();

        // TODO ゲーム終了時に、ゲームクリアとゲームオーバーの共通する処理を追加

    }

    /// <summary>
    /// ゲームデータを初期化
    /// </summary>
    private void RefreshGameData() {
        // デバッグ用
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
    /// ゲームオーバー処理
    /// </summary>
    public IEnumerator GameOver() {

        // ゲーム終了処理
        GameUpToCommon();

        // 表示
        uiManager.CreateGameOverSet();

        // TODO ゲームオーバー時の処理を追加


        yield return new WaitForSeconds(3.0f);

        // シーン遷移
        SceneStateManager.instance.PreparateNextScene(SceneType.World);
    }
}
