using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpBase : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    protected CanvasGroup canvasGroup;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    public void SetUpPopUpBase() {
        btnSubmit.onClick.AddListener(OnClickSubmit);
    }


    /// <summary>
    /// �{�^�����������ۂ̏���
    /// </summary>
    private void OnClickSubmit() {
        canvasGroup.DOFade(0, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                SceneStateManager.instance.PreparateNextScene(SceneName.Main);
                Destroy(gameObject);
            });
    }
}
