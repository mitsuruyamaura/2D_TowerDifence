using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("耐久値")]
    public int defenseBaseDurability;

    [SerializeField, HideInInspector]
    private GameObject damageEffectPrefab;  // 敵側と重複するので一旦なしにして保留

    private GameManager gameManager;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager, int defenseBaseDurability) {
        this.gameManager = gameManager;

        if (GameData.instance.isDebug) {
            defenseBaseDurability = GameData.instance.defenseBaseDurability;
        } else {
            this.defenseBaseDurability = defenseBaseDurability;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
            defenseBaseDurability -= enemyController.attackPower;

            // ダメージ演出生成
            //CreateDamageEffect();

            if (defenseBaseDurability <= 0 && gameManager.currentGameState == GameManager.GameState.Play) {
                Debug.Log("Game Over");

                // TODO ゲームオーバー処理
                gameManager.GameOver();
            }

            // 敵の破壊
            enemyController.DestroyEnemy(false);
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
