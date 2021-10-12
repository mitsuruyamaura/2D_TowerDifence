using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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

    /// <summary>
    /// セーブ・ロード用のクラス
    /// </summary>
    [System.Serializable]
    public class SaveData {
        public int clearPoint;
        public List<int> engageList = new List<int>();
        public List<int> clearedStageList = new List<int>();
        //public string engageCharaNosListString;
        //public string clearedStageNosListString;
    }

    //private const string CLEAR_POINT_KEY = "clearPoint";
    //private const string ENGAGE_CHARA_KEY = "engageCharaNosList";
    //private const string CLEARED_STAGE_KEY = "clearedStageNosList";

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

        // デバッグ用(不要)
        //SetSaveData();
        //GetSaveData();

        //SaveEngageCharaList();
        //LoadEngageCharaList();

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

            // 各値を設定
            clearPoint = totalClearPoint,
            engageList = engageCharaNosList,
            clearedStageList = clearedStageNosList

            // 各 List は string 型に変換(しなくてもシリアライズできたので、不要)
            //engageCharaNosListString = PlayerPrefsHelper.ConvertListToString(engageCharaNosList),
            //clearedStageNosListString = PlayerPrefsHelper.ConvertListToString(clearedStageNosList)
        };

        // SaveData クラスとして SAVE_KEY の名前でセーブ
        PlayerPrefsHelper.SaveSetObjectData(SAVE_KEY, saveData);
    }

    /// <summary>
    /// SaveData をロードして、各値に設定
    /// </summary>
    public void GetSaveData() {

        // SaveData としてロード
        SaveData saveData = PlayerPrefsHelper.LoadGetObjectData<SaveData>(SAVE_KEY);

        // 各値に SaveData 内の値を設定
        totalClearPoint = saveData.clearPoint;

        engageCharaNosList = saveData.engageList; //PlayerPrefsHelper.ConvertStringToList(saveData.engageCharaNosListString);

        clearedStageNosList = saveData.clearedStageList; //PlayerPrefsHelper.ConvertStringToList(saveData.clearedStageNosListString);
    }

    ///// <summary>
    ///// engageCharaNosList の値をセーブ
    ///// </summary>
    //public void SaveEngageCharaList() {

    //    // 新しく作成する文字列
    //    string engageCharaListString = "";

    //    // 契約したキャラの List をカンマ区切りの１行の文字列にする
    //    for (int i = 0; i < engageCharaNosList.Count; i++) {
    //        engageCharaListString += engageCharaNosList[i].ToString() + ",";
    //    }

    //    // 文字列をセットしてセーブ
    //    PlayerPrefs.SetString(ENGAGE_CHARA_KEY, engageCharaListString);
    //    PlayerPrefs.Save();
    //}

    ///// <summary>
    ///// engageCharaNosList の値をロード
    ///// </summary>
    //public void LoadEngageCharaList() {

    //    // 文字列としてロード
    //    string engageCharaListString = PlayerPrefs.GetString(ENGAGE_CHARA_KEY, "");

    //    // ロードした文字列がある場合
    //    if (!string.IsNullOrEmpty(engageCharaListString)) {

    //        // カンマの位置で区切って、文字列の配列を作成。その際、最後にできる空白の文字列を削除
    //        string[] strArray = engageCharaListString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
    //        Debug.Log(strArray.Length);

    //        // 配列の数だけ契約したキャラの情報があるので
    //        for (int i = 0; i < strArray.Length; i++) {
    //            Debug.Log(strArray[i]);

    //            // 配列の文字列の値を int 型に変換して List に追加して、契約キャラの List を復元
    //            engageCharaNosList.Add(int.Parse(strArray[i]));
    //        }
    //    }
    //}

    ///// <summary>
    ///// TotalClearPoint の値をセーブ
    ///// </summary>
    //public void SaveClearPoint() {

    //    PlayerPrefs.SetInt(CLEAR_POINT_KEY, totalClearPoint);

    //    PlayerPrefs.Save();

    //    Debug.Log("セーブ : " + CLEAR_POINT_KEY + " : " + totalClearPoint);
    //}

    ///// <summary>
    ///// TotalClearPoint の値をロード
    ///// </summary>
    //public void LoadClearPoint() {

    //    totalClearPoint = PlayerPrefs.GetInt(CLEAR_POINT_KEY, 0);

    //    Debug.Log("ロード : " + CLEAR_POINT_KEY + " : " + totalClearPoint);
    //}
}
