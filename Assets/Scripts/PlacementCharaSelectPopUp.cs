using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlacementCharaSelectPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnClosePopUp;

    [SerializeField]
    private Button btnChooseChara;

    [SerializeField]
    private Image imgPickupChara;

    [SerializeField]
    private Text txtPickupCharaName;

    [SerializeField]
    private Text txtPickupCharaAttackPower;

    [SerializeField]
    private Text txtPickupCharaAttackRangeType;

    [SerializeField]
    private Text txtPickupCharaCost;

    [SerializeField]
    private SelectCharaDetail selectCharaDetailPrefab;

    [SerializeField]
    private Transform selectCharaDetailTran;

    public List<SelectCharaDetail> selectCharaDetailsList = new List<SelectCharaDetail>();

    private Vector3Int createCharaPos;

    private CharaData chooseCharaData;

    private CharaGenerator charaGenerator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="haveCharaDataList"></param>
    public void SetUpPlacementCharaSelectPopUp(Vector3Int gridPos,List<CharaData> haveCharaDataList, CharaGenerator charaGenerator) {
        this.charaGenerator = charaGenerator;

        btnChooseChara.interactable = false;
        btnClosePopUp.interactable = false;
        
        // �L�����̐����ʒu�̕ێ�
        createCharaPos = gridPos;

        // �������Ă���L�������� SelectCharaDetail �𐶐�
        for (int i = 0; i < haveCharaDataList.Count; i++) {
            SelectCharaDetail selectCharaDetail = Instantiate(selectCharaDetailPrefab, selectCharaDetailTran, false);
            selectCharaDetail.SetUpSelectCharaDetail(this, haveCharaDataList[i]);
            selectCharaDetailsList.Add(selectCharaDetail);

            // �ŏ��ɐ������� SelectCharaDetail �������l�ɐݒ�
            if (i == 0) {
                SetSelectCharaDetail(haveCharaDataList[i]);
            }
        }

        btnChooseChara.onClick.AddListener(OnClickChooseChara);
        btnClosePopUp.onClick.AddListener(OnClickClosePopUp);

        btnChooseChara.interactable = true;
        btnClosePopUp.interactable = true;
    }

    /// <summary>
    /// �I�����ꂽ SelectCharaDetail �̏����|�b�v�A�b�v���̃s�b�N�A�b�v�ɕ\������
    /// </summary>
    /// <param name="charaData"></param>
    public void SetSelectCharaDetail(CharaData charaData) {
        chooseCharaData = charaData;


        imgPickupChara.sprite = charaData.charaSprite;

        txtPickupCharaName.text = charaData.charaName;

        txtPickupCharaAttackRangeType.text = charaData.attackRange.ToString();

        txtPickupCharaCost.text = charaData.cost.ToString();
    }

    /// <summary>
    /// �I�����Ă���L�����𐶐�
    /// </summary>
    private void OnClickChooseChara() {
        charaGenerator.CreateChooseChara(createCharaPos, chooseCharaData);
    }

    /// <summary>
    /// �|�b�v�A�b�v�����
    /// </summary>
    private void OnClickClosePopUp() {

    }
}
