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

    /// <summary>
    /// SelectCharaDetail の設定
    /// </summary>
    /// <param name="placementCharaSelectPop"></param>
    /// <param name="charaData"></param>
    public void SetUpSelectCharaDetail(PlacementCharaSelectPopUp placementCharaSelectPop, CharaData charaData) {
        this.placementCharaSelectPop = placementCharaSelectPop;
        this.charaData = charaData;

        btnSelectCharaDetail.interactable = false;

        imgChara.sprite = this.charaData.charaSprite;

        // コストが支払えるか確認する
        GameData.instance.CurrencyReactiveProperty.Subscribe(x => JudgePermissionCost(x));


        // コストが支払えるか確認する
        //if (this.charaData.cost <= GameData.instance.CurrencyReactiveProperty.Value) {


        //    btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

        //    btnSelectCharaDetail.interactable = true;
        //} else {

        //}

        Debug.Log("SetUp End");
    }

    /// <summary>
    /// SelectCharaDetail を押したの処理
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO アニメ演出

        // このクラスでの購読を停止する 
        //GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();

        // タップした SelectCharaDetail の情報をポップアップに送る
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }

    /// <summary>
    /// コストが支払えるか確認する
    /// </summary>
    private void JudgePermissionCost(int value) {
        if (charaData.cost <= value) {

            btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

            btnSelectCharaDetail.interactable = true;

            // このクラスでの購読を停止する 
            //GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();
            Debug.Log("Judge 停止");
        }
    }

    /// <summary>
    /// このクラスでの購読を停止する
    /// </summary>
    public void DisposeCurrency() {
        // このクラスでの購読を停止する 
        GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();
        Debug.Log("停止");
    }
}
