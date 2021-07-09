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

    private EngageCharaPopUp engageCharaPop;

    /// <summary>
    /// SelectCharaDetail の設定
    /// </summary>
    /// <param name="placementCharaSelectPop"></param>
    /// <param name="charaData"></param>
    public void SetUpSelectCharaDetail(PlacementCharaSelectPopUp placementCharaSelectPop, CharaData charaData) {
        this.placementCharaSelectPop = placementCharaSelectPop;
        this.charaData = charaData;

        ChangeActivateButton(false);

        imgChara.sprite = this.charaData.charaSprite;

        // ReactiveProperty を監視して、値が更新される度にコストが支払えるか確認する
        GameData.instance.CurrencyReactiveProperty.Subscribe(x => JudgePermissionCost(x));

        // ボタンにメソッドを登録
        btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

        // TODO 後程、コストに応じてボタンを押せるかどうかを切り替えるようにする
        ChangeActivateButton(true);
    }

    /// <summary>
    /// SelectCharaDetail を押したの処理
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO アニメ演出

        // タップした SelectCharaDetail の情報をポップアップに送る
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }

    // mi

    /// <summary>
    /// ボタンを押せる状態の切り替え
    /// </summary>
    public void ChangeActivateButton(bool isSwitch) {
        btnSelectCharaDetail.interactable = isSwitch;
    }

    /// <summary>
    /// コストが支払えるか確認する
    /// </summary>
    public bool JudgePermissionCost(int value) {

        //Debug.Log("コスト確認");

        // コストが支払える場合
        if (charaData.cost <= value) {

            // ボタンを押せる状態にする
            ChangeActivateButton(true);
            return true;
        }
        return false;
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
    /// 
    /// </summary>
    /// <param name="engageCharaPopUp"></param>
    /// <param name="charaData"></param>
    public void SetUpForEngagePopUp(EngageCharaPopUp engageCharaPopUp, CharaData charaData) {
        this.engageCharaPop = engageCharaPopUp;

        this.charaData = charaData;

        imgChara.sprite = this.charaData.charaSprite;

        // 契約可能な状態かを確認する
    }
}
