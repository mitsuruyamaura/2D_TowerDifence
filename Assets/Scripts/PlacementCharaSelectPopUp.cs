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
    public void SetUpPlacementCharaSelectPopUp(Vector3Int gridPos,List<CharaData> haveCharaDataList, CharaGenerator charaGenerator) {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f, 0.5f);

        this.charaGenerator = charaGenerator;

        btnChooseChara.interactable = false;
        btnClosePopUp.interactable = false;
        
        // キャラの生成位置の保持
        createCharaPos = gridPos;

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
    /// 表示
    /// </summary>
    public void DisplayPopUp(Vector3Int gridPos) {

        // キャラの生成位置の保持
        createCharaPos = gridPos;

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
    /// 選択しているキャラを決定
    /// </summary>
    private void OnClickSubmitChooseChara() {

        // コストの支払いが可能か最終確認
        if (chooseCharaData.cost > GameData.instance.CurrencyReactiveProperty.Value) {
            return;
        }

        charaGenerator.CreateChooseChara(createCharaPos, chooseCharaData);
        ClosePopUp();
    }

    /// <summary>
    /// ポップアップを閉じる
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
