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

        // ReactiveProperty を監視して、値が更新される度にコストが支払えるか確認する
        GameData.instance.CurrencyReactiveProperty.Subscribe(x => JudgePermissionCost(x));

        // ボタンにメソッドを登録
        btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);
    }

    /// <summary>
    /// SelectCharaDetail を押したの処理
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO アニメ演出

        // タップした SelectCharaDetail の情報をポップアップに送る
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }

    /// <summary>
    /// コストが支払えるか確認する
    /// </summary>
    public void JudgePermissionCost(int value) {

        // コストが支払える場合
        if (charaData.cost <= value) {

            // ボタンを押せる状態にする
            ChangeActivateButton(true);
        }
    }

    /// <summary>
    /// このクラスでの購読を停止する
    /// </summary>
    public void DisposeCurrency() {

        // このクラスでの購読を停止する 
        GameData.instance.CurrencyReactiveProperty.Subscribe().Dispose();
        Debug.Log("購読 停止");
    }

    /// <summary>
    /// ボタンを押せる状態の切り替え
    /// </summary>
    public void ChangeActivateButton(bool isSwitch) {
        btnSelectCharaDetail.interactable = isSwitch;
    }
}
