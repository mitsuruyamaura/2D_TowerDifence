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

    [SerializeField]
    private Image imgLock;

    /// <summary>
    /// SelectCharaDetail の設定
    /// </summary>
    /// <param name="placementCharaSelectPop"></param>
    /// <param name="charaData"></param>
    public void SetUpSelectCharaDetail(PlacementCharaSelectPopUp placementCharaSelectPop, CharaData charaData) {   // TODO 引数を Base クラスに変更する
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
    /// SetUp メソッドはあとで Base クラスに変えて統一する。多態性の学習につかう
    /// </summary>
    /// <param name="engageCharaPopUp"></param>
    /// <param name="charaData"></param>
    public void SetUpForEngagePopUp(EngageCharaPopUp engageCharaPopUp, CharaData charaData) {
        this.engageCharaPop = engageCharaPopUp;

        this.charaData = charaData;

        imgChara.sprite = this.charaData.charaSprite;

        // 契約可能な状態かを確認する
        CheckEngageState();
    }

    /// <summary>
    /// 契約状態の確認
    /// </summary>
    public void CheckEngageState() {
        ChangeActivateButton(false);
        imgLock.enabled = false;

        // 所持しているキャラの場合
        if (GameData.instance.engageCharaNosList.Contains(this.charaData.charaNo)) {
            return;
        
        } else if (!GameData.instance.engageCharaNosList.Contains(this.charaData.charaNo) && CheckPaymentEngagePoint()) {
            //所持していないキャラの場合で、契約料が支払える場合はボタンを押せる状態にする
            ChangeActivateButton(true);

            // ボタンにメソッドを登録
            btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetailFotEngage);
        } else {
            // 契約できない場合には、ロック画像を表示
            imgLock.enabled = true;
        }
    }

    /// <summary>
    /// 契約料が支払えて契約可能な状態か確認
    /// </summary>
    /// <returns></returns>
    public bool CheckPaymentEngagePoint() {
        return charaData.engagePoint <= GameData.instance.totalClearPoint ? true : false;
    }

    /// <summary>
    /// ボタンの状態の取得
    /// </summary>
    /// <returns></returns>
    public bool GetActivateButtonState() {
        return btnSelectCharaDetail.interactable;
    }

    /// <summary>
    /// CharaData の取得
    /// </summary>
    /// <returns></returns>
    public CharaData GetCharaData() {
        return charaData;
    }

    /// <summary>
    /// キャラ選択用のメソッド。あとで多態性を利用する処理に変える
    /// </summary>
    private void OnClickSelectCharaDetailFotEngage() {

        // ピックアップに登録
        engageCharaPop.SetSelectCharaDetail(charaData);
    }
}
