using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [HideInInspector]
    public ReactiveProperty<int> CurrencyReactiveProperty = new ReactiveProperty<int>();

    [Header("�R�X�g�p�̒ʉ�")]
    public int currency;

    [Header("�J�����V�[�̍ő�l")]
    public int maxCurrency;

    [Header("���Z�܂ł̑ҋ@����")]
    public int currencyIntervalTime;

    [Header("���Z�l")]
    public int addCurrencyPoint;

    [HideInInspector]
    public int maxCharaPlacementCount;    // �f�o�b�O�p

    [Header("�f�o�b�O���[�h�̐؂�ւ�")]
    public bool isDebug;�@�@�@�@�@�@�@�@�@// true �̏ꍇ�A�f�o�b�O���[�h�Ƃ���

    public int defenseBaseDurability;     // �f�o�b�O�p

    public int stageNo;

    public int totalClearPoint;

    [Header("�_�񂵂ď������Ă���L�����̔ԍ�")]
    public List<int> engageCharaNosList = new List<int>();

    [Header("�\������X�e�[�W�̔ԍ�")]
    public List<int> clearedStageNosList = new List<int>();



    [System.Serializable]
    public class SaveData {
        public int clearPoint;
        public string engageCharaNosListString;
        public string clearedStageNosListString;
    }

    private const string SAVE_KEY = "SaveData";


    // ��
    public int charaPlacementCount;       // �f�o�b�O�p(�s�v) 

    //public string save;     // �f�o�b�O�p(�s�v)

    // TODO �G�̔j�󐔂̊Ǘ��� ReactiveProperty ���g��


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        //���[�U�[�f�[�^�̏�����
        Initialize();

        // ���[�U�[�f�[�^�̏������p�̃��[�J�����\�b�h
        void Initialize() {

            // SaveData ���Z�[�u����Ă��邩�m�F
            if (PlayerPrefsHelper.ExistsData(SAVE_KEY)) {
                // �Z�[�u����Ă���ꍇ�̂݃��[�h
                GetSaveData();

            } else {
                // �Z�[�u����Ă��Ȃ���Ώ����l�ݒ�
                totalClearPoint = 0;

                if (!engageCharaNosList.Contains(0)) {
                    engageCharaNosList.Add(0);
                }

                if (!clearedStageNosList.Contains(0)) {
                    clearedStageNosList.Add(0);
                }
            }
        }

        // �f�o�b�O�p(�s�v)
        //save = PlayerPrefsHelper.ConvertListToString(clearedStageNosList);

        //clearedStageNosList = PlayerPrefsHelper.ConvertStringToList(save);
    }

    /// <summary>
    /// �Z�[�u����l�� SaveData �ɐݒ肵�ăZ�[�u
    /// �Z�[�u����^�C�~���O�́A�X�e�[�W�N���A���A�L�����_��
    /// </summary>
    public void SetSaveData() {

        // �Z�[�u�p�̃f�[�^���쐬
        SaveData saveData = new SaveData {
            clearPoint = totalClearPoint,

            // �e List �� string �^�ɕϊ�
            engageCharaNosListString = PlayerPrefsHelper.ConvertListToString(engageCharaNosList),
            clearedStageNosListString = PlayerPrefsHelper.ConvertListToString(clearedStageNosList)
        };

        // SaveData �N���X�Ƃ��� SAVE_KEY �̖��O�ŃZ�[�u
        PlayerPrefsHelper.SaveSetObjectData(SAVE_KEY, saveData);
    }

    /// <summary>
    /// SaveData �����[�h���āA�e�l�ɐݒ�
    /// </summary>
    public void GetSaveData() {
        SaveData saveData = PlayerPrefsHelper.LoadGetObjectData<SaveData>(SAVE_KEY);

        totalClearPoint = saveData.clearPoint;

        engageCharaNosList = PlayerPrefsHelper.ConvertStringToList(saveData.engageCharaNosListString);

        clearedStageNosList = PlayerPrefsHelper.ConvertStringToList(saveData.clearedStageNosListString);
    }
}
