using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyControllerPrefab;

    [SerializeField]
    private Transform[] enemyGenerateTrans;

    [SerializeField]
    private PathData[] enemyPathDatas;


    /// <summary>
    /// ìGÇÃê∂ê¨
    /// </summary>
    public EnemyController GenerateEnemy(GameManager gameManager) {
        int randomValue = Random.Range(0, enemyGenerateTrans.Length);
        EnemyController enemy = Instantiate(enemyControllerPrefab, enemyGenerateTrans[randomValue].position, Quaternion.identity);
        StartCoroutine(enemy.SetUpEnemyController(enemyPathDatas[randomValue], gameManager));
        return enemy;
    }
}
