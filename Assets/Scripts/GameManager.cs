using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    void Start()
    {
        isEnemyGenerate = true;

        // �G�̐�������
        StartCoroutine(SetUpEnemyGenerate());
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
            // TODO �Q�[���N���A�̏�����ǉ�

            Debug.Log("�Q�[���N���A");

            uiManager.CreateGameClearSet();
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver() {
        // �\��
        uiManager.CreateGameOverSet();

        // TODO �Q�[���I�[�o�[�̏�����ǉ�
    }
}
