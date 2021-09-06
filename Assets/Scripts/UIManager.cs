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
    /// カレンシーの表示更新
    /// </summary>
    public void UpdateDisplayCurrency() {

        txtCost.text = GameData.instance.currency.ToString();
    }

    /// <summary>
    /// キャラの配置を解除する選択用のポップアップの生成
    /// </summary>
    public void CreateReturnCharaPopUp(CharaController charaController, GameManager gameManager) {
        ReturnSelectCharaPopUp returnSelectCharaPopUp = Instantiate(returnCharaPopUpPrefab, canvasTran, false);
        returnSelectCharaPopUp.SetUpReturnSelectCharaPopUp(charaController, gameManager);
    }


    // mi
    void Start() {
        // 購読開始
        GameData.instance.CurrencyReactiveProperty.Subscribe((x) => txtCost.text = x.ToString());
    }

    /// <summary>
    /// ゲームクリア表示生成(未使用)
    /// </summary>
    public IEnumerator CreateGameClearSet() {
        ResetSubscribe();

        // 文字で表示するクリア表示
        //InfoManager gameClearSet = Instantiate(gameClearSetPrefab, canvasTran, false);
        //gameClearSet.SetUpInfo();

        yield return StartCoroutine(GameClear());
    }

    /// <summary>
    /// ゲームオーバー表示生成
    /// </summary>
    public void CreateGameOverSet() {
        ResetSubscribe();
        InfoManager gameOverSet = Instantiate(gameOverSetPrefab, canvasTran, false);
        gameOverSet.SetUpInfo();
    }

    /// <summary>
    /// ReactiveProperty の購読を停止
    /// </summary>
    private void ResetSubscribe() {
        // 購読停止
        GameData.instance.CurrencyReactiveProperty.Dispose();
    }

    /// <summary>
    /// オープニング演出作成
    /// </summary>
    /// <returns></returns>
    public IEnumerator Opening() {

        LogoEffect opening = Instantiate(openingEffectPrefab, canvasTran, false);
        opening.PlayOpening();
        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// ゲームクリア演出作成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GameClear() {
        ResetSubscribe();

        LogoEffect gameClear = Instantiate(gameCearEffectPrefab, canvasTran, false);
        gameClear.PlayGameClear();

        yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// 拠点の耐久力のゲージ表示の更新
    /// </summary>
    /// <param name="durabilityValue"></param>
    public void UpdateDisplayDurabilityGauge(int durabilityValue, int maxDurabilityValue) {

        float value = (float)durabilityValue / maxDurabilityValue;

        sliderDurabilityGauge.DOValue(value, 0.25f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 耐久力ゲージをセット
    /// </summary>
    /// <returns></returns>
    public void SetDurabilityGauge(Slider slider) {

        sliderDurabilityGauge = slider;
    }
}
