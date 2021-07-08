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
        // StageSelect ����Ăяo���悤�ɕύX����
        //SetUpStageSelectDetail();
    }

    /// <summary>
    /// �X�e�[�W�I���{�^���̐ݒ�
    /// </summary>
    /// <param name="stageSelectPopUp"></param>
    public void SetUpStageSelectDetail(StageSelectPopUp stageSelectPopUp) {

        this.stageSelectPopUp = stageSelectPopUp;

        stageData = DataBaseManager.instance.stageDataSO.stageDatasList.Find(x => x.stageNo == stageNo);

        txtStageName.text = stageData.stageName;

        btnStageSelect.onClick.AddListener(OnClickStageSelectDetail);
    }

    /// <summary>
    /// �X�e�[�W�I���{�^�������������ۂ̏���
    /// </summary>
    private void OnClickStageSelectDetail() {
        stageSelectPopUp.ShowStageSelectPopUp(stageData);
    }
}
