using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;


public class LogoEffect : InfoManager 
{
    [SerializeField]
    private Image imgStart;

    [SerializeField]
    private ShinyEffectForUGUI shinyEffect;

    /// <summary>
    /// オープニング演出再生
    /// </summary>
    public void PlayOpening() {
        canvasGroup.alpha = 0.0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1.0f, 0.5f));
        sequence.Append(imgStart.DOFade(1.0f, 0.5f).OnComplete(() => shinyEffect.Play(0.5f)));  // OnComplete の位置に気を付ける。外にかくと再生が最後になるので見えない
        sequence.AppendInterval(1.0f);
        sequence.Append(canvasGroup.DOFade(0.0f, 0.5f)).OnComplete(() => Destroy(gameObject));        
    }

    /// <summary>
    /// ゲームクリア演出再生
    /// </summary>
    public void PlayGameClear() {
        canvasGroup.alpha = 0.0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1.0f, 0.5f).OnComplete(() => shinyEffect.Play(1.0f)));

        // ボタンを押せるようにする
        sequence.AppendInterval(1.0f).OnComplete(() => SetUpInfo());
    }
}
