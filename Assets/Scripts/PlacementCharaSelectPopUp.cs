using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlacementCharaSelectPopUp : MonoBehaviour   // ���ƂŃN���X�p������
{
    [SerializeField]
    private Button btnClosePopUp;

    [SerializeField]
    private Button btnChooseChara;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private CharaGenerator charaGenerator;

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
    private List<SelectCharaDetail> selectCharaDetailsList = new List<SelectCharaDetail>();

    //private Vector3Int createCharaPos;

    private CharaData chooseCharaData;



    /// <summary>
    /// �|�b�v�A�b�v�̐ݒ�
    /// </summary>
    /// <param name="charaGenerator"></param>
    /// <param name="haveCharaDataList"></param>
    public void SetUpPlacementCharaSelectPopUp(CharaGenerator charaGenerator, List<CharaData> haveCharaDataList) {

        this.charaGenerator = charaGenerator;

        canvasGroup.alpha = 0;
        ShowPopUp();

        //SwithcActivateButtons(false);

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

        SwithcActivateButtons(true);
    }

    /// <summary>
    /// �e�{�^���̃A�N�e�B�u��Ԃ̐؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwithcActivateButtons(bool isSwitch) {
        btnChooseChara.interactable = isSwitch;
        btnClosePopUp.interactable = isSwitch;
    }

    /// <summary>
    /// �|�b�v�A�b�v�̕\��
    /// </summary>
    public void ShowPopUp() {

        canvasGroup.DOFade(1.0f, 0.5f);
    }

    /// <summary>
    /// �I�����Ă���L������z�u����{�^�����������ۂ̏���
    /// </summary>
    private void OnClickSubmitChooseChara() {

        // �R�X�g�̎x�������\���ŏI�m�F
        if (chooseCharaData.cost > GameData.instance.CurrencyReactiveProperty.Value) {
            return;
        }

        // �L�����̐���
        charaGenerator.CreateChooseChara(chooseCharaData);

        // �|�b�v�A�b�v�̔�\��
        HidePopUp();
    }

    /// <summary>
    /// �z�u���~�߂�{�^�����������ۂ̏���
    /// </summary>
    private void OnClickClosePopUp() {

        // �|�b�v�A�b�v�̔�\��
        HidePopUp();
    }

    /// <summary>
    /// �|�b�v�A�b�v�̔�\��
    /// </summary>
    private void HidePopUp() {

        // �e�L�����̃{�^���̐���
        CheckAllCharaButtons();

        // �|�b�v�A�b�v�̔�\��
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => charaGenerator.InactivatePlacementCharaSelectPopUp());
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
    /// �R�X�g���x�����邩�ǂ����� �e SelectCharaDetail �Ŋm�F���ă{�^�������@�\��؂�ւ�
    /// </summary>
    private void CheckAllCharaButtons() {
        // �z�u�ł���L���������Ȃ��ꍇ�̂ݏ������s��
        if (selectCharaDetailsList.Count > 0) {
            // �e�L�����̃R�X�g�ƃJ�����V�[���m�F���āA�z�u�ł��邩�ǂ����𔻒肵�ă{�^���̉����L����ݒ�
            for (int i = 0; i < selectCharaDetailsList.Count; i++) {
                selectCharaDetailsList[i].ChangeActivateButton(selectCharaDetailsList[i].JudgePermissionCost(GameData.instance.CurrencyReactiveProperty.Value));
            }
        }
    }
    private void OnEnable() {

        // �R�X�g���x�����邩�ǂ����� �e SelectCharaDetail �Ŋm�F���ă{�^�������@�\��؂�ւ�
        CheckAllCharaButtons();
    }
}
