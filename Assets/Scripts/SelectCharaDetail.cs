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

    private EngageCharaPopUp engageCharaPop;

    [SerializeField]
    private Image imgLock;

    /// <summary>
    /// SelectCharaDetail �̐ݒ�
    /// </summary>
    /// <param name="placementCharaSelectPop"></param>
    /// <param name="charaData"></param>
    public void SetUpSelectCharaDetail(PlacementCharaSelectPopUp placementCharaSelectPop, CharaData charaData) {   // TODO ������ Base �N���X�ɕύX����
        this.placementCharaSelectPop = placementCharaSelectPop;
        this.charaData = charaData;

        ChangeActivateButton(false);

        imgChara.sprite = this.charaData.charaSprite;

        // ReactiveProperty ���Ď����āA�l���X�V�����x�ɃR�X�g���x�����邩�m�F����
        GameData.instance.CurrencyReactiveProperty.Subscribe(x => JudgePermissionCost(x));

        // �{�^���Ƀ��\�b�h��o�^
        btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

        // TODO ����A�R�X�g�ɉ����ă{�^���������邩�ǂ�����؂�ւ���悤�ɂ���
        ChangeActivateButton(true);
    }

    /// <summary>
    /// SelectCharaDetail ���������̏���
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO �A�j�����o

        // �^�b�v���� SelectCharaDetail �̏����|�b�v�A�b�v�ɑ���
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }

    // mi

    /// <summary>
    /// �{�^�����������Ԃ̐؂�ւ�
    /// </summary>
    public void ChangeActivateButton(bool isSwitch) {
        btnSelectCharaDetail.interactable = isSwitch;
    }

    /// <summary>
    /// �R�X�g���x�����邩�m�F����
    /// </summary>
    public bool JudgePermissionCost(int value) {

        //Debug.Log("�R�X�g�m�F");

        // �R�X�g���x������ꍇ
        if (charaData.cost <= value) {

            // �{�^�����������Ԃɂ���
            ChangeActivateButton(true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// ���̃N���X�ł̍w�ǂ��~����
    /// </summary>
    public void DisposeCurrency() {

        // ���̃N���X�ł̍w�ǂ��~���� 
        GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();
        Debug.Log("�w�� ��~");
    }

    /// <summary>
    /// SetUp ���\�b�h�͂��Ƃ� Base �N���X�ɕς��ē��ꂷ��B���Ԑ��̊w�K�ɂ���
    /// </summary>
    /// <param name="engageCharaPopUp"></param>
    /// <param name="charaData"></param>
    public void SetUpForEngagePopUp(EngageCharaPopUp engageCharaPopUp, CharaData charaData) {
        this.engageCharaPop = engageCharaPopUp;

        this.charaData = charaData;

        ChangeActivateButton(false);

        imgChara.sprite = this.charaData.charaSprite;

        // �_��\�ȏ�Ԃ����m�F����
        // �������Ă��Ȃ��L�����̏ꍇ�ŁA�_�񗿂��x������ꍇ�̓{�^�����������Ԃɂ���
        if (!GameData.instance.possessionCharaNosList.Contains(this.charaData.charaNo) && CheckEngage()) {
            ChangeActivateButton(true);

            // �{�^���Ƀ��\�b�h��o�^
            btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetailFotEngage);
        } else {
            imgLock.enabled = true;
        }
    }

    /// <summary>
    /// �_�񗿂��x�����Č_��\�ȏ�Ԃ��m�F
    /// </summary>
    /// <returns></returns>
    private bool CheckEngage() {
        return charaData.engagePoint <= GameData.instance.totalClearPoint ? true : false;
    }

    /// <summary>
    /// �{�^���̏�Ԃ̎擾
    /// </summary>
    /// <returns></returns>
    public bool GetActivateButtonState() {
        return btnSelectCharaDetail.interactable;
    }

    /// <summary>
    /// CharaData �̎擾
    /// </summary>
    /// <returns></returns>
    public CharaData GetCharaData() {
        return charaData;
    }

    /// <summary>
    /// �L�����I��p�̃��\�b�h�B���Ƃő��Ԑ��𗘗p���鏈���ɕς���
    /// </summary>
    private void OnClickSelectCharaDetailFotEngage() {

        // �s�b�N�A�b�v�ɓo�^
        engageCharaPop.SetSelectCharaDetail(charaData);
    }
}
