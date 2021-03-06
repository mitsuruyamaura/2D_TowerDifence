using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InfoManager : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    protected CanvasGroup canvasGroup;

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpInfo() {
        btnSubmit.onClick.AddListener(OnClickSubmit);
    }


    /// <summary>
    /// ボタン押下時の処理
    /// </summary>
    private void OnClickSubmit() {
        canvasGroup.DOFade(0, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                SceneStateManager.instance.PreparateNextScene(SceneType.World);
                Destroy(gameObject);
            });
    }
}
