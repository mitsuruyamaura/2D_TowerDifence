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
    /// �|�b�v�A�b�v�̐ݒ�
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
    /// �|�b�v�A�b�v��\��
    /// </summary>
    /// <param name="stageData"></param>
    public void ShowStageSelectPopUp(StageData stageData) {

        GameData.instance.stageNo = stageData.stageNo;

        // �X�e�[�W���̐ݒ�
        txtStageName.text = stageData.stageName;

        imgStage.sprite = stageData.stageSprite;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// �|�b�v�A�b�v���B��
    /// </summary>
    public void HideStageSelectPopUp() {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// ����I����
    /// </summary>
    private void OnClickSubmit() {

        // �V�[���J�ڂ̏���
        stageSelect.PraparateBattle();

        HideStageSelectPopUp();
    }

    /// <summary>
    /// �L�����Z���I����
    /// </summary>
    private void OnClickCancel() {
        HideStageSelectPopUp();
    }
}
