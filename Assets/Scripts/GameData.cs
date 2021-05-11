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

    [Header("Š‚µ‚Ä‚¢‚éƒLƒƒƒ‰‚Ì”Ô†")]
    public List<int> possessionCharaNosList = new List<int>();

    // TODO “G‚Ì”j‰ó”‚ÌŠÇ—‚à ReactiveProperty ‚ğg‚¤


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
