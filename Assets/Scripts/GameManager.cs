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
}
