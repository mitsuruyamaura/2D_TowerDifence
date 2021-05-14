using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    public bool isEnemyGenerate;

    public int generateIntervalTime;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    public int generateEnemyCount;
    private int destroyEnemyCount;

    public int maxEnemyCount;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private DefenseBase defenseBase;

    [SerializeField]
    private CharaGenerator charaGenerator;

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();

    [SerializeField]
    private StageData currentStageData;

    /// <summary>
    /// �Q�[���̏��
    /// </summary>
    public enum GameState {
        Preparate,
        Play,
        Stop,
        GameUp
    }

    public GameState currentGameState;

    
    IEnumerator Start()
    {
        // �Q�[���̐i�s��Ԃ��������ɐݒ�
        SetGameState(GameState.Preparate);

        // �Q�[���f�[�^��������
        RefreshGameData();

        // �X�e�[�W�̐ݒ�
        SetUpStageData();

        // �L���������̐ݒ�
        StartCoroutine(charaGenerator.SetUpCharaGenerator(this));

        // ���_�̐ݒ�
        defenseBase.SetUpDefenseBase(this);

        // �I�[�v�j���O���o�Đ�
        yield return StartCoroutine(uiManager.Opening());

        isEnemyGenerate = true;

        // �Q�[���̐i�s��Ԃ��v���C���ɕύX
        SetGameState(GameState.Play);

        // �G�̐��������J�n
        StartCoroutine(enemyGenerator.PreparateEnemyGenerate(this, currentStageData));

        // �J�����V�[�̎����l�������̊J�n
        StartCoroutine(TimeToCurrency());
    }

    /// <summary>
    /// �Q�[���f�[�^��������
    /// </summary>
    private void RefreshGameData() {
        GameData.instance.charaPlacementCount = 0;

        // �Q�[���̓x�ɃC���X�^���X����
        GameData.instance.CurrencyReactiveProperty = new ReactiveProperty<int>();

        if (GameData.instance.isDebug) {
            GameData.instance.CurrencyReactiveProperty.Value = GameData.instance.maxCurrency;
        } else {
            GameData.instance.CurrencyReactiveProperty.Value = 0;
        }      
    }

    /// <summary>
    /// �X�e�[�W�f�[�^�̐ݒ�
    /// </summary>
    private void SetUpStageData() {
        currentStageData = DataBaseManager.instance.stageDataSO.stageDatasList[GameData.instance.stageNo];
        generateIntervalTime = currentStageData.generateIntervalTime;
        maxEnemyCount = currentStageData.enemys.Length;

        // TODO ���ɂ�����Βǉ�

    }

    /// <summary>
    /// �G�̏��� List �ɒǉ�
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyList(EnemyController enemy) {
        enemiesList.Add(enemy);
        generateEnemyCount++;
    }

    /// <summary>
    /// �G�̏��� List ����폜
    /// </summary>
    /// <param name="removeEnemy"></param>
    public void RemoveEnemyList(EnemyController removeEnemy) {
        enemiesList.Remove(removeEnemy);
    }

    /// <summary>
    /// �j�󂵂��G�̐����J�E���g
    /// </summary>
    public void CountUpDestoryEnemyCount() {
        destroyEnemyCount++;

        // �Q�[���N���A����
        JudgeGameClear();
    }

    /// <summary>
    /// �Q�[���N���A����
    /// </summary>
    public void JudgeGameClear() {
        // �������𒴂��Ă��邩
        if (destroyEnemyCount >= generateEnemyCount) {

            Debug.Log("�Q�[���N���A");

            GameUp();

            // TODO �Q�[���N���A���o
            uiManager.CreateGameClearSet();

            // �N���A�{�[�i�X�̊l��
            GameData.instance.totalClearPoint += currentStageData.clearPoint;

            // TODO �Q�[���N���A�̏�����ǉ�

        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver() {

        // �Q�[���I������
        GameUp();

        // �\��
        uiManager.CreateGameOverSet();

        // TODO �Q�[���I�[�o�[�̏�����ǉ�

    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    private void GameUp() {

        // �Q�[���̐i�s��Ԃ��Q�[���I���ɕύX
        SetGameState(GameState.GameUp);

        // �L�����z�u�p�̃|�b�v�A�b�v���J���Ă���ꍇ�ɂ͔j��
        charaGenerator.InactivatePlacementCharaSelectPopUp();

        // TODO �Q�[���I�����ɍs��������ǉ�

    }

    /// <summary>
    /// �I�������L�����̏��� List �ɒǉ�
    /// </summary>
    public void AddCharasList(CharaController chara) {
        charasList.Add(chara);
    }

    /// <summary>
    /// �I�������L������j�����A���� List ����폜
    /// </summary>
    /// <param name="chara"></param>
    public void RemoveCharasList(CharaController chara) {
        Destroy(chara.gameObject);
        charasList.Remove(chara);
    }

    /// <summary>
    /// �z�u������I������|�b�v�A�b�v�쐬�̏���
    /// </summary>
    /// <param name="chara"></param>
    public void PreparateCreateReturnCharaPopUp(CharaController chara) {

        // �Q�[���̐i�s��Ԃ��Q�[����~�ɕύX
        SetGameState(GameState.Stop);

        // ���ׂĂ̓G�̈ړ����ꎞ��~
        PauseEnemies();

        // �z�u������I������|�b�v�A�b�v���쐬
        uiManager.CreateReturnCharaPopUp(chara, this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void JudgeReturnChara(bool isReturnChara, CharaController chara) {

        // �L�����̔z�u����������ꍇ
        if (isReturnChara) {
            // �I�������L������j�����A���� List ����폜
            RemoveCharasList(chara);

            // �z�u�������Z
            GameData.instance.charaPlacementCount--;
        }

        //  �Q�[���̐i�s��Ԃ��v���C���ɕύX���āA�Q�[���ĊJ
        SetGameState(GameState.Play);

        // ���ׂĂ̓G�̈ړ����ĊJ
        ResumeEnemies();

        // �J�����V�[�̉��Z�������ĊJ
        StartCoroutine(TimeToCurrency());
    }

    /// <summary>
    /// ���Ԃ̌o�߂ɉ����ăJ�����V�[�����Z
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimeToCurrency() {

        int timer = 0;

        // �Q�[���v���C���̂݉��Z
        while (currentGameState == GameState.Play) {
            timer++;

            // �K��̎��Ԃ��o�߂��A�J�����V�[���ő�l�łȂ����
            if (timer > GameData.instance.getCurrencyIntervalTime && GameData.instance.CurrencyReactiveProperty.Value < GameData.instance.maxCurrency) {
                timer = 0;

                // �ő�l�ȉ��ɂȂ�悤�ɃJ�����V�[�����Z
                GameData.instance.CurrencyReactiveProperty.Value = Mathf.Clamp(GameData.instance.CurrencyReactiveProperty.Value += GameData.instance.addCurrencyPoint, 0, GameData.instance.maxCurrency);
            }

            yield return null;
        }
    }

    /// <summary>
    /// �G�̐������~���邩����
    /// </summary>
    public void JudgeGenerateEnemysEnd() {
        if (generateEnemyCount >= maxEnemyCount) {
            isEnemyGenerate = false;
        }
    }

    /// <summary>
    /// GameState �̕ύX
    /// </summary>
    /// <param name="nextGameState"></param>
    public void SetGameState(GameState nextGameState) {
        currentGameState = nextGameState;
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ꎞ��~
    /// </summary>
    public void PauseEnemies() {
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].PauseMove();
        }
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ĊJ
    /// </summary>
    public void ResumeEnemies() {
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].ResumeMove();
        }
    }
}
