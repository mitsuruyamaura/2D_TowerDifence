using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PopUpBase gameClearSetPrefab;

    [SerializeField]
    private PopUpBase gameOverSetPrefab;

    [SerializeField]
    private Transform canvasTran;

    /// <summary>
    /// ゲームクリア表示生成
    /// </summary>
    public void CreateGameClearSet() {
        PopUpBase gameClearSet = Instantiate(gameClearSetPrefab, canvasTran, false);
        gameClearSet.SetUpPopUpBase();
    }

    /// <summary>
    /// ゲームオーバー表示生成
    /// </summary>
    public void CreateGameOverSet() {
        PopUpBase gameOverSet = Instantiate(gameOverSetPrefab, canvasTran, false);
        gameOverSet.SetUpPopUpBase();
    }
}
