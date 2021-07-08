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
    /// �g�[�^���N���A�|�C���g�̕\���X�V
    /// </summary>
    private void UpdateDisplayTotalClearPoint() {
        txtTotalClearPoint.text = GameData.instance.totalClearPoint.ToString();
    }

    /// <summary>
    /// �X�e�[�W�I��p�̃|�b�v�A�b�v����
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
    /// �o�g���̏���
    /// </summary>
    public void PraparateBattle() {
        SceneStateManager.instance.PreparateNextScene(SceneName.Battle);
    }
}
