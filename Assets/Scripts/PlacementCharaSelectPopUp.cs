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
    /// ポップアップの設定
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="haveCharaDataList"></param>
    public void SetUpPlacementCharaSelectPopUp(List<CharaData> haveCharaDataList, CharaGenerator charaGenerator) {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f, 0.5f);

        this.charaGenerator = charaGenerator;

        btnChooseChara.interactable = false;
        btnClosePopUp.interactable = false;

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

        btnChooseChara.interactable = true;
        btnClosePopUp.interactable = true;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp() {

        canvasGroup.DOFade(1.0f, 0.5f);
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
        for (int i = 0; i < selectCharaDetailsList.Count; i++) {
            selectCharaDetailsList[i].ChangeActivateButton(false);
        }

        // ポップアップの非表示
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => charaGenerator.InactivatePlacementCharaSelectPopUp());
    }
}
