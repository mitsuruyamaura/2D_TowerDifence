using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �_�񉉏o�p�̃N���X
/// </summary>
public class ContractDetail : MonoBehaviour
{
    [SerializeField]
    private Image imgChara;

    [SerializeField]
    private Text txtCharaName;

    [SerializeField]
    private Button btnSubmitContractStamp;

    [SerializeField]
    private Button btnFilter;

    [SerializeField]
    private CanvasGroup canvasGrouContractSet;

    [SerializeField]
    private CanvasGroup canvasGroupSubmitContractStamp;

    [SerializeField]
    private Image imgContractStamp;


    /// <summary>
    /// �_�񉉏o�̐ݒ�
    /// </summary>
    /// <param name="charaData"></param>
    public void SetUpContractDetail(CharaData charaData) {

        // �_�񂵂��L�����̉摜��ݒ�
        imgChara.sprite = charaData.charaSprite;

        // �_�񂵂��L�����̖��O������
        txtCharaName.text = charaData.charaName;

        // ���ԂɃ{�^����������悤�ɁA���Ƃ���\������X�^���v�̉摜�������Ȃ��悤�ɐݒ�
        canvasGrouContractSet.alpha = 0;
        canvasGroupSubmitContractStamp.alpha = 0;
        canvasGroupSubmitContractStamp.blocksRaycasts = false;
        imgContractStamp.enabled = false;

        // �{�^���Ƀ��\�b�h��o�^
        btnFilter.onClick.AddListener(OnClickFilter);
        btnSubmitContractStamp.onClick.AddListener(OnClickSubmitContract);

        // �ŏ��̃{�^���p�� CanvasGruop ��\��
        canvasGrouContractSet.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// �X�^���v�O�Ƀ^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickFilter() {

        // �X�^���v�𓮂���
        imgContractStamp.transform.localScale = Vector3.one * 3;
        imgContractStamp.transform.eulerAngles = new Vector3(0, 0, Random.Range(-30.0f, 30.0f));
        imgContractStamp.enabled = true;

        canvasGroupSubmitContractStamp.alpha = 1.0f;

        // �X�^���v�����̑傫���ɖ߂�
        imgContractStamp.transform.DOScale(Vector3.one, 0.75f)
            .SetEase(Ease.OutBack, 1.0f)
            .OnComplete(() => 
            {
                // �{�^����������悤�ɂ���
                canvasGroupSubmitContractStamp.blocksRaycasts = true;
            }
        );
    }

    /// <summary>
    /// �X�^���v��Ƀ^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickSubmitContract() {
        // �_�񉉏o���I�����āA�|�b�v�A�b�v������
        canvasGrouContractSet.DOFade(0.0f, 0.5f).SetEase(Ease.Linear).OnComplete(() => { Destroy(gameObject); });       
    }
}
