using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);

        btnSelectCharaDetail.interactable = true;
    }

    /// <summary>
    /// SelectCharaDetail を押したの処理
    /// </summary>
    private void OnClickSelectCharaDetail() {
        // TODO アニメ演出

        // タップした SelectCharaDetail の情報をポップアップに送る
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }
}
