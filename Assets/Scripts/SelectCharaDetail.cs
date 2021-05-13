using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

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
        GameData.instance.CurrencyReactiveProperty.Subscribe(x => JudgePermissionCost(x));


        // �R�X�g���x�����邩�m�F����
        //if (this.charaData.cost <= GameData.instance.CurrencyReactiveProperty.Value) {


        //    btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

        //    btnSelectCharaDetail.interactable = true;
        //} else {

        //}

        Debug.Log("SetUp End");
    }

    /// <summary>
    /// SelectCharaDetail ���������̏���
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO �A�j�����o

        // ���̃N���X�ł̍w�ǂ��~���� 
        //GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();

        // �^�b�v���� SelectCharaDetail �̏����|�b�v�A�b�v�ɑ���
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }

    /// <summary>
    /// �R�X�g���x�����邩�m�F����
    /// </summary>
    private void JudgePermissionCost(int value) {
        if (charaData.cost <= value) {

            btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

            btnSelectCharaDetail.interactable = true;

            // ���̃N���X�ł̍w�ǂ��~���� 
            //GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();
            Debug.Log("Judge ��~");
        }
    }

    /// <summary>
    /// ���̃N���X�ł̍w�ǂ��~����
    /// </summary>
    public void DisposeCurrency() {
        // ���̃N���X�ł̍w�ǂ��~���� 
        GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();
        Debug.Log("��~");
    }
}
