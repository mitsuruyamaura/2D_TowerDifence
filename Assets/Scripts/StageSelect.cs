using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    private Text txtTotalClearPoint;

    [SerializeField]
    private List<StageSelectDetail> stageSelectDetailsList = new List<StageSelectDetail>();

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private StageSelectPopUp stageSelectPopUpPrefab;

    private StageSelectPopUp stageSelectPopUp;


    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplayTotalClearPoint();

        GenerateStageSelectPopUp();

        SetUpAllStageSelectDetails();
    }

    /// <summary>
    /// トータルクリアポイントの表示更新
    /// </summary>
    private void UpdateDisplayTotalClearPoint() {
        txtTotalClearPoint.text = GameData.instance.totalClearPoint.ToString();
    }

    /// <summary>
    /// ステージ選択用のポップアップ生成
    /// </summary>
    private void GenerateStageSelectPopUp() {

        stageSelectPopUp = Instantiate(stageSelectPopUpPrefab, canvasTran, false);

        stageSelectPopUp.SetUpStageSelectPopUp(this);
    }

    private void SetUpAllStageSelectDetails() {
        for (int i = 0; i < stageSelectDetailsList.Count; i++) {
            stageSelectDetailsList[i].SetUpStageSelectDetail(stageSelectPopUp);
        }
    }

    /// <summary>
    /// バトルの準備
    /// </summary>
    public void PraparateBattle() {
        SceneStateManager.instance.PreparateNextScene(SceneName.Battle);
    }
}
