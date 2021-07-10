using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EngageCharaPopUp : CharaSelectPopUpBase
{
    [SerializeField]
    private Text txtEngagePoint;


    public override void SetUpPopUp() {

        base.SetUpPopUp();

        // データベース内のすべてのキャラのデータをボタンとして生成する
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++) {
            SelectCharaDetail selectCharaDetail = Instantiate(selectCharaDetailPrefab, selectCharaDetailTran, false);
            selectCharaDetail.SetUpForEngagePopUp(this, DataBaseManager.instance.charaDataSO.charaDatasList[i]);
            selectCharaDetailsList.Add(selectCharaDetail);

            // 初期ピックアップキャラの登録がなく、所持していないキャラで、かつ、契約料が支払えるキャラがいる場合は、初期ピックアップとして登録する
            if (chooseCharaData == null && selectCharaDetail.GetActivateButtonState()) {
                chooseCharaData = selectCharaDetail.GetCharaData();
            }
        }

        btnClosePopUp.interactable = true;
    }

    /// <summary>
    /// 選択しているキャラを配置するボタンを押した際の処理
    /// </summary>
    protected override void OnClickSubmitChooseChara() {

        // TODO 契約料の支払いが可能か最終確認
        //if (chooseCharaData.cost > GameData.instance.CurrencyReactiveProperty.Value) {
        //    return;
        //}

        // GameData にキャラ追加

        // 演出

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// 選択された SelectCharaDetail の情報をポップアップ内のピックアップに表示する
    /// </summary>
    /// <param name="charaData"></param>
    public override void SetSelectCharaDetail(CharaData charaData) {
        base.SetSelectCharaDetail(charaData);

        txtEngagePoint.text = charaData.engagePoint.ToString();

        btnChooseChara.interactable = true;
    }
}
