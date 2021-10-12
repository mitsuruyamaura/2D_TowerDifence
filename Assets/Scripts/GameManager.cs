using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private CharaGenerator charaGenerator;

    public bool isEnemyGenerate;

    public int generateIntervalTime;

    public int generateEnemyCount;

    public int maxEnemyCount;

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

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();

    private int destroyEnemyCount;

    //[SerializeField]
    private DefenseBase defenseBase;
    private DefenseBase[] defenseBases;

    [SerializeField]
    private MapInfo currentMapInfo;

    [SerializeField]
    private DefenseBase defenseBasePrefab;

    [SerializeField]
    private StageData currentStageData;


    IEnumerator Start()
    {
        // �Q�[���̐i�s��Ԃ��������ɐݒ�
        SetGameState(GameState.Preparate);

        // �Q�[���f�[�^��������
        RefreshGameData();

        // �X�e�[�W�̐ݒ� + �X�e�[�W���Ƃ� PathData ��ݒ�
        SetUpStageData();

        // �L�����z�u�p�|�b�v�A�b�v�̐����Ɛݒ�
        StartCoroutine(charaGenerator.SetUpCharaGenerator(this, currentStageData));

        // ���_�̐ݒ�(�����ɂ����̂ŁASetUpStageData ���\�b�h���ōs��)
        //defenseBase.SetUpDefenseBase(this, currentStageData.defenseBaseDurability, uiManager);

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
    /// �G�̏��� List �ɒǉ�
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyList(EnemyController enemy) {
        enemiesList.Add(enemy);
        generateEnemyCount++;
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
    public void CountUpDestoryEnemyCount(EnemyController enemyController) {
        // �G�̏��� List ����폜
        RemoveEnemyList(enemyController);

        destroyEnemyCount++;

        Debug.Log("�j�󂵂��G�̐� : " + destroyEnemyCount);

        // �Q�[���N���A����
        JudgeGameClear();
    }

    /// <summary>
    /// �Q�[���N���A����
    /// </summary>
    public void JudgeGameClear() {

        // �h�q���_�̑ϋv�͂� 0 �ȉ��̏ꍇ
        if (GameData.instance.defenseBaseDurability <= 0) {

            // �Q�[���I�[�o�[
            StartCoroutine(GameOver());
            return;
        }

        // �������𒴂��Ă��邩
        if (destroyEnemyCount >= maxEnemyCount) {

            Debug.Log("�Q�[���N���A");

            // �Q�[���N���A�̏�����ǉ�(�N���A��V)
            StartCoroutine(GameClearAndResult());
        }
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
            if (timer > GameData.instance.currencyIntervalTime && GameData.instance.CurrencyReactiveProperty.Value < GameData.instance.maxCurrency) {
                timer = 0;

                // �ő�l�ȉ��ɂȂ�悤�ɃJ�����V�[�����Z
                GameData.instance.CurrencyReactiveProperty.Value = Mathf.Clamp(GameData.instance.CurrencyReactiveProperty.Value += GameData.instance.addCurrencyPoint, 0, GameData.instance.maxCurrency);
            }

            yield return null;
        }
    }

    /// <summary>
    /// �I�������L�����̏��� List �ɒǉ�
    /// </summary>
    public void AddCharasList(CharaController chara) {
        charasList.Add(chara);

        // TODO �L�������J�E���g
        GameData.instance.charaPlacementCount++;
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
    /// ���݂̔z�u���Ă���L�����̐��̎擾
    /// </summary>
    /// <returns></returns>
    public int GetPlacementCharaCount() {
        return charasList.Count;
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
    /// �I�������L�����̔z�u����
    /// </summary>
    /// <param name="isReturnChara"></param>
    /// <param name="chara"></param>
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
    /// �X�e�[�W�f�[�^�̐ݒ�
    /// </summary>
    private void SetUpStageData() {

        // GameData �� stageNo ���� StageData ���擾
        currentStageData = DataBaseManager.instance.stageDataSO.stageDatasList[GameData.instance.stageNo];
        generateIntervalTime = currentStageData.generateIntervalTime;
        maxEnemyCount = currentStageData.mapInfo.appearEnemyInfos.Length;

        // TODO ���ɂ�����Βǉ�
        currentMapInfo = Instantiate(currentStageData.mapInfo);

        //defenseBase = Instantiate(defenseBasePrefab, currentMapInfo.GetDefenseBaseTran());

        // DefenseBase �̈ʒu�����擾
        Transform[] defenseBaseTrans = currentMapInfo.GetMultipleDefenseBaseTrans();
        Debug.Log(defenseBaseTrans.Length);

        defenseBases = new DefenseBase[defenseBaseTrans.Length];

        // ������ DefenseBase �̐���(�P�̏ꍇ�ɂ��Ή�)
        for (int i = 0; i < defenseBaseTrans.Length; i++) {
            defenseBases[i] = Instantiate(defenseBasePrefab, defenseBaseTrans[i]);
            defenseBases[i].SetUpDefenseBase(this, currentStageData.defenseBaseDurability, uiManager);
        }

        // PathDatas �̐ݒ�
        PathData[] pathDatas = new PathData[currentStageData.mapInfo.appearEnemyInfos.Length];
        for (int i = 0; i < currentStageData.mapInfo.appearEnemyInfos.Length; i++) {
            pathDatas[i] = currentStageData.mapInfo.appearEnemyInfos[i].enemyPathData;
        }
        enemyGenerator.SetUpPathDatas(pathDatas);
    }

    /// <summary>
    /// �Q�[���N���A�ƕ�V����
    /// </summary>
    private IEnumerator GameClearAndResult() {

        // �Q�[���I��
        GameUpToCommon();

        // TODO �Q�[���N���A���o(����)
        //yield return StartCoroutine(uiManager.CreateGameClearSet());

        // ���S�ŉ��o
        yield return StartCoroutine(uiManager.GameClear());

        // �N���A�{�[�i�X�̊l��
        GameData.instance.totalClearPoint += currentStageData.clearPoint;

        GameData.instance.stageNo++;

        // ���N���A�ł���ꍇ
        if (!GameData.instance.clearedStageNosList.Contains(GameData.instance.stageNo)) {
            // ���̃X�e�[�W��o�^���ăX�e�[�W�V�[���ŕ\���ł���悤�ɂ���
            GameData.instance.clearedStageNosList.Add(GameData.instance.stageNo);
        }

        // �Z�[�u
        GameData.instance.SetSaveData();

        // �V�[���J��
        SceneStateManager.instance.PreparateNextScene(SceneType.World);
    }

    /// <summary>
    /// �Q�[���I�����̋��ʏ���
    /// </summary>
    private void GameUpToCommon() {

        // �Q�[���̐i�s��Ԃ��Q�[���I���ɕύX
        SetGameState(GameState.GameUp);

        // �L�����z�u�p�̃|�b�v�A�b�v���J���Ă���ꍇ�ɂ͔j��
        charaGenerator.InactivatePlacementCharaSelectPopUp();

        // TODO �Q�[���I�����ɁA�Q�[���N���A�ƃQ�[���I�[�o�[�̋��ʂ��鏈����ǉ�

    }

    /// <summary>
    /// �Q�[���f�[�^��������
    /// </summary>
    private void RefreshGameData() {
        // �f�o�b�O�p
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
    /// �Q�[���I�[�o�[����
    /// </summary>
    public IEnumerator GameOver() {

        // �Q�[���I������
        GameUpToCommon();

        // �\��
        uiManager.CreateGameOverSet();

        // TODO �Q�[���I�[�o�[���̏�����ǉ�


        yield return new WaitForSeconds(3.0f);

        // �V�[���J��
        SceneStateManager.instance.PreparateNextScene(SceneType.World);
    }
}
