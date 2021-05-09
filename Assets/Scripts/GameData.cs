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
