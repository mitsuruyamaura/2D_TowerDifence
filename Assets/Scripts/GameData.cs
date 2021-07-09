using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public ReactiveProperty<int> CurrencyReactiveProperty = new ReactiveProperty<int>();

    public int maxCurrency;

    public int charaPlacementCount;

    public int maxCharaPlacementCount;

    public bool isDebug;

    public int getCurrencyIntervalTime;

    public int addCurrencyPoint;

    public int defenseBaseDurability;

    public int stageNo;

    public int totalClearPoint;

    [Header("所持しているキャラの番号")]
    public List<int> possessionCharaNosList = new List<int>();

    [Header("表示するステージの番号")]
    public List<int> clearedStageNosList = new List<int>();


    // TODO 敵の破壊数の管理も ReactiveProperty を使う


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
