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

    private int generateEnemyCount;
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

    


    /// <summary>
    /// �Q�[���̏��
    /// </summary>
    public enum GameState {
        Wait,
        Play,
        Stop,
        GameUp
    }

    public GameState currentGameState;

    
    void Start()
    {
        RefreshGameData();

        currentGameState = GameState.Wait;

        StartCoroutine(charaGenerator.SetUpCharaGenerator(this));

        defenseBase.SetUpDefenseBase(this);

        isEnemyGenerate = true;

        currentGameState = GameState.Play;

        // �G�̐�������
        StartCoroutine(SetUpEnemyGenerate());

        // �J�����V�[�̎����l��
        StartCoroutine(TimeToCurrency());
    }

    /// <summary>
    /// ������
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
    /// �G�̐�������
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetUpEnemyGenerate() {

        int timer = 0;

        while (isEnemyGenerate) {

            timer++;

            if (timer > generateIntervalTime) {
                timer = 0;

                // �G�̐����� List �ւ̒ǉ�
                enemiesList.Add(enemyGenerator.GenerateEnemy(this));
                generateEnemyCount++;

                // �ő吶�����𒴂�����
                if (generateEnemyCount >= maxEnemyCount) {
                    isEnemyGenerate = false;
                }
            }

            yield return null;
        }

        // TODO �����I��


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

            uiManager.CreateGameClearSet();

            // TODO �Q�[���N���A�̏�����ǉ�
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver() {

        GameUp();

        // �\��
        uiManager.CreateGameOverSet();

        // TODO �Q�[���I�[�o�[�̏�����ǉ�
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    private void GameUp() {

        currentGameState = GameState.GameUp;

        // �L�����z�u�p�̃|�b�v�A�b�v���J���Ă���ꍇ�ɂ͔j��
        charaGenerator.DestroyPlacementCharaSelectPopUp();

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

    public void PreparateCreateReturnCharaPopUp(CharaController chara) {

        // �Q�[����~
        currentGameState = GameState.Stop;

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

        // �Q�[���ĊJ
        currentGameState = GameState.Play;
    }

    /// <summary>
    /// ���Ԃ̌o�߂ɉ����ăJ�����V�[�����Z
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeToCurrency() {

        int timer = 0;

        while (true) {
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
}
