using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [HideInInspector]
    public ReactiveProperty<int> CurrencyReactiveProperty = new ReactiveProperty<int>();

    [Header("コスト用の通貨")]
    public int currency;

    [Header("カレンシーの最大値")]
    public int maxCurrency;

    [Header("加算までの待機時間")]
    public int currencyIntervalTime;

    [Header("加算値")]
    public int addCurrencyPoint;

    [HideInInspector]
    public int maxCharaPlacementCount;    // デバッグ用

    [Header("デバッグモードの切り替え")]
    public bool isDebug;　　　　　　　　　// true の場合、デバッグモードとする

    public int defenseBaseDurability;     // デバッグ用

    public int stageNo;

    public int totalClearPoint;

    [Header("契約して所持しているキャラの番号")]
    public List<int> engageCharaNosList = new List<int>();

    [Header("表示するステージの番号")]
    public List<int> clearedStageNosList = new List<int>();



    [System.Serializable]
    public class SaveData {
        public int clearPoint;
        public string engageCharaNosListString;
        public string clearedStageNosListString;
    }

    private const string SAVE_KEY = "SaveData";


    // 未
    public int charaPlacementCount;       // デバッグ用(不要) 

    //public string save;     // デバッグ用(不要)

    // TODO 敵の破壊数の管理も ReactiveProperty を使う


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        //ユーザーデータの初期化
        Initialize();

        // ユーザーデータの初期化用のローカルメソッド
        void Initialize() {

            // SaveData がセーブされているか確認
            if (PlayerPrefsHelper.ExistsData(SAVE_KEY)) {
                // セーブされている場合のみロード
                GetSaveData();

            } else {
                // セーブされていなければ初期値設定
                totalClearPoint = 0;

                if (!engageCharaNosList.Contains(0)) {
                    engageCharaNosList.Add(0);
                }

                if (!clearedStageNosList.Contains(0)) {
                    clearedStageNosList.Add(0);
                }
            }
        }

        // デバッグ用(不要)
        //save = PlayerPrefsHelper.ConvertListToString(clearedStageNosList);

        //clearedStageNosList = PlayerPrefsHelper.ConvertStringToList(save);
    }

    /// <summary>
    /// セーブする値を SaveData に設定してセーブ
    /// セーブするタイミングは、ステージクリア時、キャラ契約時
    /// </summary>
    public void SetSaveData() {

        // セーブ用のデータを作成
        SaveData saveData = new SaveData {
            clearPoint = totalClearPoint,

            // 各 List は string 型に変換
            engageCharaNosListString = PlayerPrefsHelper.ConvertListToString(engageCharaNosList),
            clearedStageNosListString = PlayerPrefsHelper.ConvertListToString(clearedStageNosList)
        };

        // SaveData クラスとして SAVE_KEY の名前でセーブ
        PlayerPrefsHelper.SaveSetObjectData(SAVE_KEY, saveData);
    }

    /// <summary>
    /// SaveData をロードして、各値に設定
    /// </summary>
    public void GetSaveData() {
        SaveData saveData = PlayerPrefsHelper.LoadGetObjectData<SaveData>(SAVE_KEY);

        totalClearPoint = saveData.clearPoint;

        engageCharaNosList = PlayerPrefsHelper.ConvertStringToList(saveData.engageCharaNosListString);

        clearedStageNosList = PlayerPrefsHelper.ConvertStringToList(saveData.clearedStageNosListString);
    }
}
