using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : SceneStateBase
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

    [SerializeField]
    private EngageCharaPopUp engageCharaPopUpPrefab;

    private EngageCharaPopUp engageCharaPopUp;

    [SerializeField]
    private Button btnEngage;


    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();

        GenerateStageSelectPopUp();

        if (!GameData.instance.clearedStageNosList.Contains(0)) {
            GameData.instance.clearedStageNosList.Add(0);
        }

        SetUpAllStageSelectDetails();

        GenerateEngageCharaPopUp();

        btnEngage.onClick.AddListener(OnClickEngage);
    }

    /// <summary>
    /// トータルクリアポイントの表示更新
    /// </summary>
    public override void UpdateDisplay() {
        txtTotalClearPoint.text = GameData.instance.totalClearPoint.ToString();
    }

    /// <summary>
    /// ステージ選択用のポップアップ生成
    /// </summary>
    private void GenerateStageSelectPopUp() {

        stageSelectPopUp = Instantiate(stageSelectPopUpPrefab, canvasTran, false);

        stageSelectPopUp.SetUpStageSelectPopUp(this);
    }

    /// <summary>
    /// StageSelectDetail の設定と表示切り替え
    /// </summary>
    private void SetUpAllStageSelectDetails() {
        for (int i = 0; i < stageSelectDetailsList.Count; i++) {
            stageSelectDetailsList[i].SetUpStageSelectDetail(stageSelectPopUp);

            if (GameData.instance.clearedStageNosList.Contains(stageSelectDetailsList[i].GetStageNo())) {
                stageSelectDetailsList[i].SwitchActivateStageSelectDetail(true);
            }
        }
    }

    /// <summary>
    /// バトルの準備
    /// </summary>
    public void PraparateBattle() {
        SceneStateManager.instance.PreparateNextScene(SceneType.Battle);
    }

    /// <summary>
    /// キャラ契約用のポップアップの生成
    /// </summary>
    private void GenerateEngageCharaPopUp() {

        engageCharaPopUp = Instantiate(engageCharaPopUpPrefab, canvasTran, false);

        engageCharaPopUp.SetUpPopUp(this);
    }

    /// <summary>
    /// エンゲージボタンを押下した際の処理
    /// </summary>
    private void OnClickEngage() {
        engageCharaPopUp.ShowPopUp();
    }
}
