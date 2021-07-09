using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelectPopUp : MonoBehaviour
{
    [SerializeField]
    private Text txtStageName;

    [SerializeField]
    private Image imgStage;

    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private Button btnCancel;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private StageSelect stageSelect;

    /// <summary>
    /// ポップアップの設定
    /// </summary>
    /// <param name="stageSelect"></param>
    public void SetUpStageSelectPopUp(StageSelect stageSelect) {

        canvasGroup.alpha = 0;

        this.stageSelect = stageSelect;

        btnCancel.onClick.AddListener(OnClickCancel);
        btnSubmit.onClick.AddListener(OnClickSubmit);

        HideStageSelectPopUp();
    }

    /// <summary>
    /// ポップアップを表示
    /// </summary>
    /// <param name="stageData"></param>
    public void ShowStageSelectPopUp(StageData stageData) {

        GameData.instance.stageNo = stageData.stageNo;

        // ステージ情報の設定
        txtStageName.text = stageData.stageName;

        imgStage.sprite = stageData.stageSprite;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// ポップアップを隠す
    /// </summary>
    public void HideStageSelectPopUp() {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 決定選択時
    /// </summary>
    private void OnClickSubmit() {

        // シーン遷移の準備
        stageSelect.PraparateBattle();

        HideStageSelectPopUp();
    }

    /// <summary>
    /// キャンセル選択時
    /// </summary>
    private void OnClickCancel() {
        HideStageSelectPopUp();
    }
}
