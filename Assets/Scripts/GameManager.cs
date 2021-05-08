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

        // 敵の生成準備
        StartCoroutine(SetUpEnemyGenerate());
    }

    /// <summary>
    /// 敵の生成準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetUpEnemyGenerate() {

        int timer = 0;

        while (isEnemyGenerate) {

            timer++;

            if (timer > generateIntervalTime) {
                timer = 0;

                // 敵の生成と List への追加
                enemiesList.Add(enemyGenerator.GenerateEnemy(this));
            }

            yield return null;
        }

        // TODO 生成終了


    }

    /// <summary>
    /// 敵の情報を List から削除
    /// </summary>
    /// <param name="removeEnemy"></param>
    public void RemoveEnemyList(EnemyController removeEnemy) {
        enemiesList.Remove(removeEnemy);
    }
}
