using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 契約演出用のクラス
/// </summary>
public class ContractDetail : MonoBehaviour
{
    [SerializeField]
    private Image imgChara;

    [SerializeField]
    private Text txtCharaName;

    [SerializeField]
    private Button btnSubmitContractStamp;

    [SerializeField]
    private Button btnFilter;

    [SerializeField]
    private CanvasGroup canvasGrouContractSet;

    [SerializeField]
    private CanvasGroup canvasGroupSubmitContractStamp;

    [SerializeField]
    private Image imgContractStamp;


    /// <summary>
    /// 契約演出の設定
    /// </summary>
    /// <param name="charaData"></param>
    public void SetUpContractDetail(CharaData charaData) {

        // 契約したキャラの画像を設定
        imgChara.sprite = charaData.charaSprite;

        // 契約したキャラの名前を決定
        txtCharaName.text = charaData.charaName;

        // 順番にボタンを押せるように、あとから表示するスタンプの画像を見えないように設定
        canvasGrouContractSet.alpha = 0;
        canvasGroupSubmitContractStamp.alpha = 0;
        canvasGroupSubmitContractStamp.blocksRaycasts = false;
        imgContractStamp.enabled = false;

        // ボタンにメソッドを登録
        btnFilter.onClick.AddListener(OnClickFilter);
        btnSubmitContractStamp.onClick.AddListener(OnClickSubmitContract);

        // 最初のボタン用の CanvasGruop を表示
        canvasGrouContractSet.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// スタンプ前にタップした際の処理
    /// </summary>
    private void OnClickFilter() {

        // スタンプを動かす
        imgContractStamp.transform.localScale = Vector3.one * 3;
        imgContractStamp.transform.eulerAngles = new Vector3(0, 0, Random.Range(-30.0f, 30.0f));
        imgContractStamp.enabled = true;

        canvasGroupSubmitContractStamp.alpha = 1.0f;

        // スタンプを元の大きさに戻す
        imgContractStamp.transform.DOScale(Vector3.one, 0.75f)
            .SetEase(Ease.OutBack, 1.0f)
            .OnComplete(() => 
            {
                // ボタンを押せるようにする
                canvasGroupSubmitContractStamp.blocksRaycasts = true;
            }
        );
    }

    /// <summary>
    /// スタンプ後にタップした際の処理
    /// </summary>
    private void OnClickSubmitContract() {
        // 契約演出を終了して、ポップアップも閉じる
        canvasGrouContractSet.DOFade(0.0f, 0.5f).SetEase(Ease.Linear).OnComplete(() => { Destroy(gameObject); });       
    }
}
