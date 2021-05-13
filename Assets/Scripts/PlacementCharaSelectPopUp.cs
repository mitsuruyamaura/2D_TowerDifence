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
    private Text txtPickupCharaMAxAttackCount;


    [SerializeField]
    private SelectCharaDetail selectCharaDetailPrefab;

    [SerializeField]
    private Transform selectCharaDetailTran;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private List<SelectCharaDetail> selectCharaDetailsList = new List<SelectCharaDetail>();

    private Vector3Int createCharaPos;

    private CharaData chooseCharaData;

    private CharaGenerator charaGenerator;

    /// <summary>
    /// �|�b�v�A�b�v�̐ݒ�
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="haveCharaDataList"></param>
    public void SetUpPlacementCharaSelectPopUp(Vector3Int gridPos,List<CharaData> haveCharaDataList, CharaGenerator charaGenerator) {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f, 0.5f);

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

        btnChooseChara.onClick.AddListener(OnClickSubmitChooseChara);
        btnClosePopUp.onClick.AddListener(OnClickClosePopUp);

        btnChooseChara.interactable = true;
        btnClosePopUp.interactable = true;
    }

    /// <summary>
    /// �\��
    /// </summary>
    public void DisplayPopUp(Vector3Int gridPos) {

        // �L�����̐����ʒu�̕ێ�
        createCharaPos = gridPos;

        canvasGroup.DOFade(1.0f, 0.5f);
    }

    /// <summary>
    /// �I�����ꂽ SelectCharaDetail �̏����|�b�v�A�b�v���̃s�b�N�A�b�v�ɕ\������
    /// </summary>
    /// <param name="charaData"></param>
    public void SetSelectCharaDetail(CharaData charaData) {
        chooseCharaData = charaData;
        
        // �e�l�̐ݒ�
        imgPickupChara.sprite = charaData.charaSprite;

        txtPickupCharaName.text = charaData.charaName;

        txtPickupCharaAttackPower.text = charaData.attackPower.ToString();

        txtPickupCharaAttackRangeType.text = charaData.attackRange.ToString();

        txtPickupCharaCost.text = charaData.cost.ToString();

        txtPickupCharaMAxAttackCount.text = charaData.maxAttackCount.ToString();
    }

    /// <summary>
    /// �I�����Ă���L����������
    /// </summary>
    private void OnClickSubmitChooseChara() {

        // �R�X�g�̎x�������\���ŏI�m�F
        if (chooseCharaData.cost > GameData.instance.CurrencyReactiveProperty.Value) {
            return;
        }

        charaGenerator.CreateChooseChara(createCharaPos, chooseCharaData);
        ClosePopUp();
    }

    /// <summary>
    /// �|�b�v�A�b�v�����
    /// </summary>
    private void OnClickClosePopUp() {
        ClosePopUp();
    }

    private void ClosePopUp() {
        for (int i = 0; i < selectCharaDetailsList.Count; i++) {
            selectCharaDetailsList[i].DisposeCurrency();
            Destroy(selectCharaDetailsList[i].gameObject);
        }
        selectCharaDetailsList.Clear();

        canvasGroup.DOFade(0, 0.5f).OnComplete(() => charaGenerator.DestroyPlacementCharaSelectPopUp());
    }
}
