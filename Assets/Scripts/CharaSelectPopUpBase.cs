using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaSelectPopUpBase : MonoBehaviour
{
    [SerializeField]
    protected Button btnClosePopUp;

    [SerializeField]
    protected Button btnChooseChara;

    [SerializeField]
    protected CanvasGroup canvasGroup;

    [SerializeField]
    protected Image imgPickupChara;

    [SerializeField]
    protected Text txtPickupCharaName;

    [SerializeField]
    protected Text txtPickupCharaAttackPower;

    [SerializeField]
    protected Text txtPickupCharaAttackRangeType;

    [SerializeField]
    protected Text txtPickupCharaCost;

    [SerializeField]
    protected Text txtPickupCharaMAxAttackCount;

    [SerializeField]
    protected SelectCharaDetail selectCharaDetailPrefab;

    [SerializeField]
    protected Transform selectCharaDetailTran;

    [SerializeField]
    protected List<SelectCharaDetail> selectCharaDetailsList = new List<SelectCharaDetail>();

    protected CharaData chooseCharaData;

    protected SceneStateBase sceneStateBase;

    /// <summary>
    /// �|�b�v�A�b�v�̏����ݒ�
    /// </summary>
    public virtual void SetUpPopUp(SceneStateBase sceneStateBase) {

        this.sceneStateBase = sceneStateBase; 

        canvasGroup.alpha = 0;
        HidePopUp();

        SwithcActivateButtons(false);

        btnChooseChara.onClick.AddListener(OnClickSubmitChooseChara);
        btnClosePopUp.onClick.AddListener(OnClickClosePopUp);
    }

    /// <summary>
    /// �e�{�^���̃A�N�e�B�u��Ԃ̐؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public virtual void SwithcActivateButtons(bool isSwitch) {
        btnChooseChara.interactable = isSwitch;
        btnClosePopUp.interactable = isSwitch;
    }

    /// <summary>
    /// �|�b�v�A�b�v�̕\��
    /// </summary>
    public virtual void ShowPopUp() {

        // �|�b�v�A�b�v�̕\��
        canvasGroup.DOFade(1.0f, 0.5f);
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// ��������肷��{�^�����������ۂ̏���
    /// </summary>
    protected virtual void OnClickSubmitChooseChara() {

        // �|�b�v�A�b�v�̔�\��
        HidePopUp();
    }

    /// <summary>
    /// ������~�߂�{�^�����������ۂ̏���
    /// </summary>
    protected virtual void OnClickClosePopUp() {

        // �|�b�v�A�b�v�̔�\��
        HidePopUp();
    }

    /// <summary>
    /// �|�b�v�A�b�v�̔�\��
    /// </summary>
    protected virtual void HidePopUp() {

        // �|�b�v�A�b�v�̔�\��
        canvasGroup.DOFade(0, 0.5f);

        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// �I�����ꂽ SelectCharaDetail �̏���ێ����A�|�b�v�A�b�v���̃s�b�N�A�b�v�ɕ\��
    /// </summary>
    /// <param name="charaData"></param>
    public virtual void SetSelectCharaDetail(CharaData charaData) {
        chooseCharaData = charaData;

        // �e�l�̐ݒ�
        imgPickupChara.sprite = charaData.charaSprite;

        txtPickupCharaName.text = charaData.charaName;

        txtPickupCharaAttackPower.text = charaData.attackPower.ToString();

        txtPickupCharaAttackRangeType.text = charaData.attackRange.ToString();

        txtPickupCharaCost.text = charaData.cost.ToString();

        txtPickupCharaMAxAttackCount.text = charaData.maxAttackCount.ToString();
    }
}
