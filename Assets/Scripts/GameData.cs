using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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

    /// <summary>
    /// �Z�[�u�E���[�h�p�̃N���X
    /// </summary>
    [System.Serializable]
    public class SaveData {
        public int clearPoint;
        public List<int> engageList = new List<int>();
        public List<int> clearedStageList = new List<int>();
        //public string engageCharaNosListString;
        //public string clearedStageNosListString;
    }

    //private const string CLEAR_POINT_KEY = "clearPoint";
    //private const string ENGAGE_CHARA_KEY = "engageCharaNosList";
    //private const string CLEARED_STAGE_KEY = "clearedStageNosList";

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

        // �f�o�b�O�p(�s�v)
        //SetSaveData();
        //GetSaveData();

        //SaveEngageCharaList();
        //LoadEngageCharaList();

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

            // �e�l��ݒ�
            clearPoint = totalClearPoint,
            engageList = engageCharaNosList,
            clearedStageList = clearedStageNosList

            // �e List �� string �^�ɕϊ�(���Ȃ��Ă��V���A���C�Y�ł����̂ŁA�s�v)
            //engageCharaNosListString = PlayerPrefsHelper.ConvertListToString(engageCharaNosList),
            //clearedStageNosListString = PlayerPrefsHelper.ConvertListToString(clearedStageNosList)
        };

        // SaveData �N���X�Ƃ��� SAVE_KEY �̖��O�ŃZ�[�u
        PlayerPrefsHelper.SaveSetObjectData(SAVE_KEY, saveData);
    }

    /// <summary>
    /// SaveData �����[�h���āA�e�l�ɐݒ�
    /// </summary>
    public void GetSaveData() {

        // SaveData �Ƃ��ă��[�h
        SaveData saveData = PlayerPrefsHelper.LoadGetObjectData<SaveData>(SAVE_KEY);

        // �e�l�� SaveData ���̒l��ݒ�
        totalClearPoint = saveData.clearPoint;

        engageCharaNosList = saveData.engageList; //PlayerPrefsHelper.ConvertStringToList(saveData.engageCharaNosListString);

        clearedStageNosList = saveData.clearedStageList; //PlayerPrefsHelper.ConvertStringToList(saveData.clearedStageNosListString);
    }

    ///// <summary>
    ///// engageCharaNosList �̒l���Z�[�u
    ///// </summary>
    //public void SaveEngageCharaList() {

    //    // �V�����쐬���镶����
    //    string engageCharaListString = "";

    //    // �_�񂵂��L������ List ���J���}��؂�̂P�s�̕�����ɂ���
    //    for (int i = 0; i < engageCharaNosList.Count; i++) {
    //        engageCharaListString += engageCharaNosList[i].ToString() + ",";
    //    }

    //    // ��������Z�b�g���ăZ�[�u
    //    PlayerPrefs.SetString(ENGAGE_CHARA_KEY, engageCharaListString);
    //    PlayerPrefs.Save();
    //}

    ///// <summary>
    ///// engageCharaNosList �̒l�����[�h
    ///// </summary>
    //public void LoadEngageCharaList() {

    //    // ������Ƃ��ă��[�h
    //    string engageCharaListString = PlayerPrefs.GetString(ENGAGE_CHARA_KEY, "");

    //    // ���[�h���������񂪂���ꍇ
    //    if (!string.IsNullOrEmpty(engageCharaListString)) {

    //        // �J���}�̈ʒu�ŋ�؂��āA������̔z����쐬�B���̍ہA�Ō�ɂł���󔒂̕�������폜
    //        string[] strArray = engageCharaListString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
    //        Debug.Log(strArray.Length);

    //        // �z��̐������_�񂵂��L�����̏�񂪂���̂�
    //        for (int i = 0; i < strArray.Length; i++) {
    //            Debug.Log(strArray[i]);

    //            // �z��̕�����̒l�� int �^�ɕϊ����� List �ɒǉ����āA�_��L������ List �𕜌�
    //            engageCharaNosList.Add(int.Parse(strArray[i]));
    //        }
    //    }
    //}

    ///// <summary>
    ///// TotalClearPoint �̒l���Z�[�u
    ///// </summary>
    //public void SaveClearPoint() {

    //    PlayerPrefs.SetInt(CLEAR_POINT_KEY, totalClearPoint);

    //    PlayerPrefs.Save();

    //    Debug.Log("�Z�[�u : " + CLEAR_POINT_KEY + " : " + totalClearPoint);
    //}

    ///// <summary>
    ///// TotalClearPoint �̒l�����[�h
    ///// </summary>
    //public void LoadClearPoint() {

    //    totalClearPoint = PlayerPrefs.GetInt(CLEAR_POINT_KEY, 0);

    //    Debug.Log("���[�h : " + CLEAR_POINT_KEY + " : " + totalClearPoint);
    //}
}
