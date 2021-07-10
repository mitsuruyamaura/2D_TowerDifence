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
    /// �I�[�v�j���O���o�Đ�
    /// </summary>
    public void PlayOpening() {
        canvasGroup.alpha = 0.0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1.0f, 0.5f));
        sequence.Append(imgStart.DOFade(1.0f, 0.5f).OnComplete(() => shinyEffect.Play(0.5f)));  // OnComplete �̈ʒu�ɋC��t����B�O�ɂ����ƍĐ����Ō�ɂȂ�̂Ō����Ȃ�
        sequence.AppendInterval(1.0f);
        sequence.Append(canvasGroup.DOFade(0.0f, 0.5f)).OnComplete(() => Destroy(gameObject));        
    }

    /// <summary>
    /// �Q�[���N���A���o�Đ�
    /// </summary>
    public void PlayGameClear() {
        canvasGroup.alpha = 0.0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1.0f, 0.5f).OnComplete(() => shinyEffect.Play(1.0f)));

        // �{�^����������悤�ɂ���
        sequence.AppendInterval(1.0f).OnComplete(() => SetUpInfo());
    }
}
