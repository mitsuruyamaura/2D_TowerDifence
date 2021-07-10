using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
    /// 設定
    /// </summary>
    /// <param name="charaData"></param>
    public void SetUpContractDetail(CharaData charaData) {

        imgChara.sprite = charaData.charaSprite;

        txtCharaName.text = charaData.charaName;

        canvasGrouContractSet.alpha = 0;
        canvasGroupSubmitContractStamp.alpha = 0;
        canvasGroupSubmitContractStamp.blocksRaycasts = false;
        imgContractStamp.enabled = false;

        btnFilter.onClick.AddListener(OnClickFilter);
        btnSubmitContractStamp.onClick.AddListener(OnClickSubmitContract);


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

        imgContractStamp.transform.DOScale(Vector3.one, 0.75f)
            .SetEase(Ease.OutBack, 1.0f)
            .OnComplete(() => 
            {
                canvasGroupSubmitContractStamp.blocksRaycasts = true;
            }
        );
    }

    /// <summary>
    /// スタンプ後にタップした際の処理
    /// </summary>
    private void OnClickSubmitContract() {
        canvasGrouContractSet.DOFade(0.0f, 0.5f).SetEase(Ease.Linear).OnComplete(() => { Destroy(gameObject); });       
    }
}
