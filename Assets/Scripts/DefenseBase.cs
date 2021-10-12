using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("耐久値")]
    public int defenseBaseDurability;       // 複数ある場合には利用しない

    private int maxDefenseBaseDurability;

    private GameManager gameManager;
    private UIManager uiManager;


    // 未
    [SerializeField, HideInInspector]
    private GameObject damageEffectPrefab;  // 敵側と重複するので一旦なしにして保留  EffectManager で対応

    [SerializeField]
    private UnityEngine.UI.Slider durabilityGauge;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager, int defenseBaseDurability, UIManager uiManager) {
        this.gameManager = gameManager;
        this.uiManager = uiManager;

        // １つの場合
        //uiManager.SetDurabilityGauge(durabilityGauge);

        // 複数の場合(拠点上のゲージは隠す)
        durabilityGauge.gameObject.SetActive(false);

        if (GameData.instance.isDebug) {
            maxDefenseBaseDurability = GameData.instance.defenseBaseDurability;
        } else {
            maxDefenseBaseDurability = defenseBaseDurability;
        }

        // １つの場合
        //this.defenseBaseDurability = maxDefenseBaseDurability;
        //uiManager.UpdateDisplayDurabilityGauge(this.defenseBaseDurability, maxDefenseBaseDurability);

        // 複数の場合
        GameData.instance.defenseBaseDurability = maxDefenseBaseDurability;
        uiManager.UpdateDipslayMultipleDefenseBaseDurabilityGauge(GameData.instance.defenseBaseDurability, maxDefenseBaseDurability);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {

            // １つしかない場合 
            //defenseBaseDurability = Mathf.Clamp(defenseBaseDurability - enemyController.attackPower, 0, maxDefenseBaseDurability);

            // 複数の DefenseBase がある場合
            GameData.instance.defenseBaseDurability = Mathf.Clamp(GameData.instance.defenseBaseDurability - enemyController.attackPower, 0, maxDefenseBaseDurability);

            // ダメージ演出生成(耐久力 0 になった場合で別のエフェクトを用意してもいい)
            CreateDamageEffect();

            // １つしかない場合 
            //uiManager.UpdateDisplayDurabilityGauge(defenseBaseDurability, maxDefenseBaseDurability);

            // 複数の DefenseBase がある場合
            uiManager.UpdateDipslayMultipleDefenseBaseDurabilityGauge(GameData.instance.defenseBaseDurability, maxDefenseBaseDurability);

            // 複数の場合
            if (GameData.instance.defenseBaseDurability <= 0 && gameManager.currentGameState == GameManager.GameState.Play) {
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

        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Hit_DefenseBase), transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
