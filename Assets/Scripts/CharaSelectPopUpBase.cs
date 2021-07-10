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
    /// ポップアップの初期設定
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
    /// 各ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public virtual void SwithcActivateButtons(bool isSwitch) {
        btnChooseChara.interactable = isSwitch;
        btnClosePopUp.interactable = isSwitch;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public virtual void ShowPopUp() {

        // ポップアップの表示
        canvasGroup.DOFade(1.0f, 0.5f);
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 動作を決定するボタンを押した際の処理
    /// </summary>
    protected virtual void OnClickSubmitChooseChara() {

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// 動作を止めるボタンを押した際の処理
    /// </summary>
    protected virtual void OnClickClosePopUp() {

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    protected virtual void HidePopUp() {

        // ポップアップの非表示
        canvasGroup.DOFade(0, 0.5f);

        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 選択された SelectCharaDetail の情報を保持し、ポップアップ内のピックアップに表示
    /// </summary>
    /// <param name="charaData"></param>
    public virtual void SetSelectCharaDetail(CharaData charaData) {
        chooseCharaData = charaData;

        // 各値の設定
        imgPickupChara.sprite = charaData.charaSprite;

        txtPickupCharaName.text = charaData.charaName;

        txtPickupCharaAttackPower.text = charaData.attackPower.ToString();

        txtPickupCharaAttackRangeType.text = charaData.attackRange.ToString();

        txtPickupCharaCost.text = charaData.cost.ToString();

        txtPickupCharaMAxAttackCount.text = charaData.maxAttackCount.ToString();
    }
}
