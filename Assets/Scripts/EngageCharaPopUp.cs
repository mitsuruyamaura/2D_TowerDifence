using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EngageCharaPopUp : CharaSelectPopUpBase
{
    [SerializeField]
    private Text txtEngagePoint;

    /// <summary>
    /// ポップアップの設定
    /// </summary>
    /// <param name="sceneStateBase"></param>
    public override void SetUpPopUp(SceneStateBase sceneStateBase) {

        base.SetUpPopUp(sceneStateBase);

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
    /// ポップアップの表示と契約状態の確認
    /// </summary>
    public override void ShowPopUp() {
        
        // 契約状態の確認
        for (int i = 0; i < selectCharaDetailsList.Count; i++) {
            selectCharaDetailsList[i].CheckEngageState();
        }

        base.ShowPopUp();
    }

    /// <summary>
    /// 選択しているキャラを配置するボタンを押した際の処理
    /// </summary>
    protected override void OnClickSubmitChooseChara() {

        // 契約料の支払いが可能か最終確認
        if (chooseCharaData.engagePoint > GameData.instance.totalClearPoint) {
            return;
        }

        // 支払い
        GameData.instance.totalClearPoint -= chooseCharaData.engagePoint;

        // GameData にキャラ追加
        GameData.instance.engageCharaNosList.Add(chooseCharaData.charaNo);

        // 表示更新　UniRX でも
        sceneStateBase.UpdateDisplay();

        // 契約演出
        StartCoroutine(PreparateEngageEffect());
    }

    /// <summary>
    /// 契約演出の準備と待機
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreparateEngageEffect() {
        yield return StartCoroutine(GenerateEngageEffect());

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// 契約演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateEngageEffect() {
        yield return null;
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
