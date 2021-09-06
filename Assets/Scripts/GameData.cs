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


    // ��
    public int stageNo;

    public int totalClearPoint;

    [Header("�_�񂵂ď������Ă���L�����̔ԍ�")]
    public List<int> engageCharaNosList = new List<int>();

    [Header("�\������X�e�[�W�̔ԍ�")]
    public List<int> clearedStageNosList = new List<int>();


    public int charaPlacementCount;       // �f�o�b�O�p(�s�v) 

    // TODO �G�̔j�󐔂̊Ǘ��� ReactiveProperty ���g��


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
