using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("耐久値")]
    public int defenseBaseDurability;

    private int maxDefenseBaseDurability;


    // 未
    [SerializeField, HideInInspector]
    private GameObject damageEffectPrefab;  // 敵側と重複するので一旦なしにして保留  EffectManager で対応

    [SerializeField]
    private UnityEngine.UI.Slider durabilityGauge;

    private GameManager gameManager;
    private UIManager uiManager;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager, int defenseBaseDurability, UIManager uiManager) {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
        uiManager.SetDurabilityGauge(durabilityGauge);

        if (GameData.instance.isDebug) {
            maxDefenseBaseDurability = GameData.instance.defenseBaseDurability;
        } else {
            maxDefenseBaseDurability = defenseBaseDurability;
        }

        this.defenseBaseDurability = maxDefenseBaseDurability;

        uiManager.UpdateDisplayDurabilityGauge(this.defenseBaseDurability, maxDefenseBaseDurability);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
            defenseBaseDurability = Mathf.Clamp(defenseBaseDurability - enemyController.attackPower, 0, maxDefenseBaseDurability);

            // ダメージ演出生成(耐久力 0 になった場合で別のエフェクトを用意してもいい)
            CreateDamageEffect();

            uiManager.UpdateDisplayDurabilityGauge(defenseBaseDurability, maxDefenseBaseDurability);


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
        //GameObject effect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);

        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectType.Hit_DefenseBase), transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
