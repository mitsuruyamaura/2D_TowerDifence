using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EngageCharaPopUp : CharaSelectPopUpBase
{
    [SerializeField]
    private Text txtEngagePoint;


    public override void SetUpPopUp() {

        base.SetUpPopUp();

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
    /// �I�����Ă���L������z�u����{�^�����������ۂ̏���
    /// </summary>
    protected override void OnClickSubmitChooseChara() {

        // TODO �_�񗿂̎x�������\���ŏI�m�F
        //if (chooseCharaData.cost > GameData.instance.CurrencyReactiveProperty.Value) {
        //    return;
        //}

        // GameData �ɃL�����ǉ�

        // ���o

        // �|�b�v�A�b�v�̔�\��
        HidePopUp();
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
