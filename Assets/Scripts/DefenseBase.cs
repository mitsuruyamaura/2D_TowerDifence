using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("耐久値")]
    public int defenseBaseDurability;

    [SerializeField]
    private GameObject damageEffectPrefab;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
            defenseBaseDurability -= enemyController.attackPower;

            // ダメージ演出生成
            CreateDamageEffect();

            if (defenseBaseDurability <= 0) {
                Debug.Log("Game Over");

                // TODO ゲームオーバー処理
            }

            // 敵の破壊
            enemyController.DestroyEnemy();
        }
    }

    /// <summary>
    /// ダメージ演出生成
    /// </summary>
    private void CreateDamageEffect() {
        GameObject effect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
