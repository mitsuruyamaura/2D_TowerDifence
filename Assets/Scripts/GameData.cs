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

    // TODO �G�̔j�󐔂̊Ǘ��� ReactiveProperty ���g��


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
