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
    /// �g�[�^���N���A�|�C���g�̕\���X�V
    /// </summary>
    public override void UpdateDisplay() {
        txtTotalClearPoint.text = GameData.instance.totalClearPoint.ToString();
    }

    /// <summary>
    /// �X�e�[�W�I��p�̃|�b�v�A�b�v����
    /// </summary>
    private void GenerateStageSelectPopUp() {

        stageSelectPopUp = Instantiate(stageSelectPopUpPrefab, canvasTran, false);

        stageSelectPopUp.SetUpStageSelectPopUp(this);
    }

    /// <summary>
    /// StageSelectDetail �̐ݒ�ƕ\���؂�ւ�
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
    /// �o�g���̏���
    /// </summary>
    public void PraparateBattle() {
        SceneStateManager.instance.PreparateNextScene(SceneType.Battle);
    }

    /// <summary>
    /// �L�����_��p�̃|�b�v�A�b�v�̐���
    /// </summary>
    private void GenerateEngageCharaPopUp() {

        engageCharaPopUp = Instantiate(engageCharaPopUpPrefab, canvasTran, false);

        engageCharaPopUp.SetUpPopUp(this);
    }

    /// <summary>
    /// �G���Q�[�W�{�^�������������ۂ̏���
    /// </summary>
    private void OnClickEngage() {
        engageCharaPopUp.ShowPopUp();
    }
}
