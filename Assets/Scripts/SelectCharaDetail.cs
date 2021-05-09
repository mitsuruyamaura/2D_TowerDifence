using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectCharaDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnSelectCharaDetail;

    [SerializeField]
    private Image imgChara;

    private PlacementCharaSelectPopUp placementCharaSelectPop;

    private CharaData charaData;

    /// <summary>
    /// SelectCharaDetail �̐ݒ�
    /// </summary>
    /// <param name="placementCharaSelectPop"></param>
    /// <param name="charaData"></param>
    public void SetUpSelectCharaDetail(PlacementCharaSelectPopUp placementCharaSelectPop, CharaData charaData) {
        this.placementCharaSelectPop = placementCharaSelectPop;
        this.charaData = charaData;

        btnSelectCharaDetail.interactable = false;

        imgChara.sprite = this.charaData.charaSprite;

        // �R�X�g���x�����邩�m�F����
        if (this.charaData.cost <= GameData.instance.CurrencyReactiveProperty.Value) {


            btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

            btnSelectCharaDetail.interactable = true;
        } else {

        }


    }

    /// <summary>
    /// SelectCharaDetail ���������̏���
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO �A�j�����o

        // �^�b�v���� SelectCharaDetail �̏����|�b�v�A�b�v�ɑ���
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }
}
