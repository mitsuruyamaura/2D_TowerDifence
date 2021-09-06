using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtCost;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private ReturnSelectCharaPopUp returnCharaPopUpPrefab;


    // mi
    [SerializeField]
    private InfoManager gameClearSetPrefab;

    [SerializeField]
    private InfoManager gameOverSetPrefab;

    [SerializeField]
    private LogoEffect openingEffectPrefab;

    [SerializeField]
    private LogoEffect gameCearEffectPrefab;

    [SerializeField]
    private Slider sliderDurabilityGauge;


    /// <summary>
    /// �J�����V�[�̕\���X�V
    /// </summary>
    public void UpdateDisplayCurrency() {

        txtCost.text = GameData.instance.currency.ToString();
    }

    /// <summary>
    /// �L�����̔z�u����������I��p�̃|�b�v�A�b�v�̐���
    /// </summary>
    public void CreateReturnCharaPopUp(CharaController charaController, GameManager gameManager) {
        ReturnSelectCharaPopUp returnSelectCharaPopUp = Instantiate(returnCharaPopUpPrefab, canvasTran, false);
        returnSelectCharaPopUp.SetUpReturnSelectCharaPopUp(charaController, gameManager);
    }


    // mi
    void Start() {
        // �w�ǊJ�n
        GameData.instance.CurrencyReactiveProperty.Subscribe((x) => txtCost.text = x.ToString());
    }

    /// <summary>
    /// �Q�[���N���A�\������(���g�p)
    /// </summary>
    public IEnumerator CreateGameClearSet() {
        ResetSubscribe();

        // �����ŕ\������N���A�\��
        //InfoManager gameClearSet = Instantiate(gameClearSetPrefab, canvasTran, false);
        //gameClearSet.SetUpInfo();

        yield return StartCoroutine(GameClear());
    }

    /// <summary>
    /// �Q�[���I�[�o�[�\������
    /// </summary>
    public void CreateGameOverSet() {
        ResetSubscribe();
        InfoManager gameOverSet = Instantiate(gameOverSetPrefab, canvasTran, false);
        gameOverSet.SetUpInfo();
    }

    /// <summary>
    /// ReactiveProperty �̍w�ǂ��~
    /// </summary>
    private void ResetSubscribe() {
        // �w�ǒ�~
        GameData.instance.CurrencyReactiveProperty.Dispose();
    }

    /// <summary>
    /// �I�[�v�j���O���o�쐬
    /// </summary>
    /// <returns></returns>
    public IEnumerator Opening() {

        LogoEffect opening = Instantiate(openingEffectPrefab, canvasTran, false);
        opening.PlayOpening();
        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// �Q�[���N���A���o�쐬
    /// </summary>
    /// <returns></returns>
    public IEnumerator GameClear() {
        ResetSubscribe();

        LogoEffect gameClear = Instantiate(gameCearEffectPrefab, canvasTran, false);
        gameClear.PlayGameClear();

        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// ���_�̑ϋv�͂̃Q�[�W�\���̍X�V
    /// </summary>
    /// <param name="durabilityValue"></param>
    public void UpdateDisplayDurabilityGauge(int durabilityValue, int maxDurabilityValue) {

        float value = (float)durabilityValue / maxDurabilityValue;

        sliderDurabilityGauge.DOValue(value, 0.25f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// �ϋv�̓Q�[�W���Z�b�g
    /// </summary>
    /// <returns></returns>
    public void SetDurabilityGauge(Slider slider) {

        sliderDurabilityGauge = slider;
    }
}
