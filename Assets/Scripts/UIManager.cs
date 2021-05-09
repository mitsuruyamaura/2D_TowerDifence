using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PopUpBase gameClearSetPrefab;

    [SerializeField]
    private PopUpBase gameOverSetPrefab;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private Text txtCost;

    void Start() {
        GameData.instance.CurrencyReactiveProperty.Subscribe((x) => txtCost.text = x.ToString());
    }


    /// <summary>
    /// �Q�[���N���A�\������
    /// </summary>
    public void CreateGameClearSet() {
        PopUpBase gameClearSet = Instantiate(gameClearSetPrefab, canvasTran, false);
        gameClearSet.SetUpPopUpBase();
    }

    /// <summary>
    /// �Q�[���I�[�o�[�\������
    /// </summary>
    public void CreateGameOverSet() {
        PopUpBase gameOverSet = Instantiate(gameOverSetPrefab, canvasTran, false);
        gameOverSet.SetUpPopUpBase();
    }
}
