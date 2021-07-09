using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectDetail : MonoBehaviour
{
    [SerializeField]
    private Text txtStageName;

    [SerializeField]
    private Button btnStageSelect;

    [SerializeField]
    private int stageNo;

    [SerializeField]
    private StageData stageData;

    private StageSelectPopUp stageSelectPopUp;


    void Start() {
        // StageSelect から呼び出すように変更する
        //SetUpStageSelectDetail();
    }

    /// <summary>
    /// ステージ選択ボタンの設定
    /// </summary>
    /// <param name="stageSelectPopUp"></param>
    public void SetUpStageSelectDetail(StageSelectPopUp stageSelectPopUp) {

        this.stageSelectPopUp = stageSelectPopUp;

        stageData = DataBaseManager.instance.stageDataSO.stageDatasList.Find(x => x.stageNo == stageNo);

        txtStageName.text = stageData.stageName;

        btnStageSelect.onClick.AddListener(OnClickStageSelectDetail);

        SwitchActivateStageSelectDetail(false);
    }

    /// <summary>
    /// ステージ選択ボタンを押下した際の処理
    /// </summary>
    private void OnClickStageSelectDetail() {
        stageSelectPopUp.ShowStageSelectPopUp(stageData);
    }

    /// <summary>
    /// ステージ番号の取得
    /// </summary>
    /// <returns></returns>
    public int GetStageNo() {
        return stageData.stageNo;
    }

    /// <summary>
    /// ゲームオブジェクトの表示切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateStageSelectDetail(bool isSwitch) {
        gameObject.SetActive(isSwitch);
    }
}
