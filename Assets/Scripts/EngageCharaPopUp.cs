using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EngageCharaPopUp : CharaSelectPopUpBase
{
    [SerializeField]
    private Text txtEngagePoint;

    /// <summary>
    /// �|�b�v�A�b�v�̐ݒ�
    /// </summary>
    /// <param name="sceneStateBase"></param>
    public override void SetUpPopUp(SceneStateBase sceneStateBase) {

        base.SetUpPopUp(sceneStateBase);

        // �f�[�^�x�[�X���̂��ׂẴL�����̃f�[�^���{�^���Ƃ��Đ�������
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++) {
            SelectCharaDetail selectCharaDetail = Instantiate(selectCharaDetailPrefab, selectCharaDetailTran, false);
            selectCharaDetail.SetUpForEngagePopUp(this, DataBaseManager.instance.charaDataSO.charaDatasList[i]);
            selectCharaDetailsList.Add(selectCharaDetail);

            // �����s�b�N�A�b�v�L�����̓o�^���Ȃ��A�������Ă��Ȃ��L�����ŁA���A�_�񗿂��x������L����������ꍇ�́A�����s�b�N�A�b�v�Ƃ��ēo�^����
            if (chooseCharaData == null && selectCharaDetail.GetActivateButtonState()) {
                chooseCharaData = selectCharaDetail.GetCharaData();
            }
        }

        btnClosePopUp.interactable = true;
    }

    /// <summary>
    /// �|�b�v�A�b�v�̕\���ƌ_���Ԃ̊m�F
    /// </summary>
    public override void ShowPopUp() {
        
        // �_���Ԃ̊m�F
        for (int i = 0; i < selectCharaDetailsList.Count; i++) {
            selectCharaDetailsList[i].CheckEngageState();
        }

        base.ShowPopUp();
    }

    /// <summary>
    /// �I�����Ă���L������z�u����{�^�����������ۂ̏���
    /// </summary>
    protected override void OnClickSubmitChooseChara() {

        // �_�񗿂̎x�������\���ŏI�m�F
        if (chooseCharaData.engagePoint > GameData.instance.totalClearPoint) {
            return;
        }

        // �x����
        GameData.instance.totalClearPoint -= chooseCharaData.engagePoint;

        // GameData �ɃL�����ǉ�
        GameData.instance.engageCharaNosList.Add(chooseCharaData.charaNo);

        // �\���X�V�@UniRX �ł�
        sceneStateBase.UpdateDisplay();

        // �_�񉉏o
        StartCoroutine(PreparateEngageEffect());
    }

    /// <summary>
    /// �_�񉉏o�̏����Ƒҋ@
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreparateEngageEffect() {
        yield return StartCoroutine(GenerateEngageEffect());

        // �|�b�v�A�b�v�̔�\��
        HidePopUp();
    }

    /// <summary>
    /// �_�񉉏o
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateEngageEffect() {
        yield return null;
    }

    /// <summary>
    /// �I�����ꂽ SelectCharaDetail �̏����|�b�v�A�b�v���̃s�b�N�A�b�v�ɕ\������
    /// </summary>
    /// <param name="charaData"></param>
    public override void SetSelectCharaDetail(CharaData charaData) {
        base.SetSelectCharaDetail(charaData);

        txtEngagePoint.text = charaData.engagePoint.ToString();

        btnChooseChara.interactable = true;
    }
}
