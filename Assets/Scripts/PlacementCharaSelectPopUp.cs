using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlacementCharaSelectPopUp : MonoBehaviour   // あとでクラス継承する
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
    /// ポップアップの設定
    /// </summary>
    /// <param name="charaGenerator"></param>
    /// <param name="haveCharaDataList"></param>
    public void SetUpPlacementCharaSelectPopUp(CharaGenerator charaGenerator, List<CharaData> haveCharaDataList) {

        this.charaGenerator = charaGenerator;

        canvasGroup.alpha = 0;
        ShowPopUp();

        //SwithcActivateButtons(false);

        // 所持しているキャラ分の SelectCharaDetail を生成
        for (int i = 0; i < haveCharaDataList.Count; i++) {
            SelectCharaDetail selectCharaDetail = Instantiate(selectCharaDetailPrefab, selectCharaDetailTran, false);
            selectCharaDetail.SetUpSelectCharaDetail(this, haveCharaDataList[i]);
            selectCharaDetailsList.Add(selectCharaDetail);

            // 最初に生成した SelectCharaDetail を初期値に設定
            if (i == 0) {
                SetSelectCharaDetail(haveCharaDataList[i]);
            }
        }

        btnChooseChara.onClick.AddListener(OnClickSubmitChooseChara);
        btnClosePopUp.onClick.AddListener(OnClickClosePopUp);

        SwithcActivateButtons(true);
    }

    /// <summary>
    /// 各ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwithcActivateButtons(bool isSwitch) {
        btnChooseChara.interactable = isSwitch;
        btnClosePopUp.interactable = isSwitch;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp() {

        canvasGroup.DOFade(1.0f, 0.5f);
    }

    /// <summary>
    /// 選択しているキャラを配置するボタンを押した際の処理
    /// </summary>
    private void OnClickSubmitChooseChara() {

        // コストの支払いが可能か最終確認
        if (chooseCharaData.cost > GameData.instance.CurrencyReactiveProperty.Value) {
            return;
        }

        // キャラの生成
        charaGenerator.CreateChooseChara(chooseCharaData);

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// 配置を止めるボタンを押した際の処理
    /// </summary>
    private void OnClickClosePopUp() {

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    private void HidePopUp() {

        // 各キャラのボタンの制御
        CheckAllCharaButtons();

        // ポップアップの非表示
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => charaGenerator.InactivatePlacementCharaSelectPopUp());
    }

    /// <summary>
    /// 選択された SelectCharaDetail の情報をポップアップ内のピックアップに表示する
    /// </summary>
    /// <param name="charaData"></param>
    public void SetSelectCharaDetail(CharaData charaData) {
        chooseCharaData = charaData;

        // 各値の設定
        imgPickupChara.sprite = charaData.charaSprite;

        txtPickupCharaName.text = charaData.charaName;

        txtPickupCharaAttackPower.text = charaData.attackPower.ToString();

        txtPickupCharaAttackRangeType.text = charaData.attackRange.ToString();

        txtPickupCharaCost.text = charaData.cost.ToString();

        txtPickupCharaMAxAttackCount.text = charaData.maxAttackCount.ToString();
    }

    /// <summary>
    /// コストが支払えるかどうかを 各 SelectCharaDetail で確認してボタン押下機能を切り替え
    /// </summary>
    private void CheckAllCharaButtons() {
        // 配置できるキャラがいない場合のみ処理を行う
        if (selectCharaDetailsList.Count > 0) {
            // 各キャラのコストとカレンシーを確認して、配置できるかどうかを判定してボタンの押下有無を設定
            for (int i = 0; i < selectCharaDetailsList.Count; i++) {
                selectCharaDetailsList[i].ChangeActivateButton(selectCharaDetailsList[i].JudgePermissionCost(GameData.instance.CurrencyReactiveProperty.Value));
            }
        }
    }
    private void OnEnable() {

        // コストが支払えるかどうかを 各 SelectCharaDetail で確認してボタン押下機能を切り替え
        CheckAllCharaButtons();
    }
}
