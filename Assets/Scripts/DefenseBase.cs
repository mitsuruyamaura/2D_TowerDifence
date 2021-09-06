using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("�ϋv�l")]
    public int defenseBaseDurability;

    private int maxDefenseBaseDurability;


    // ��
    [SerializeField, HideInInspector]
    private GameObject damageEffectPrefab;  // �G���Əd������̂ň�U�Ȃ��ɂ��ĕۗ�  EffectManager �őΉ�

    [SerializeField]
    private UnityEngine.UI.Slider durabilityGauge;

    private GameManager gameManager;
    private UIManager uiManager;

    /// <summary>
    /// �ݒ�
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

            // �_���[�W���o����(�ϋv�� 0 �ɂȂ����ꍇ�ŕʂ̃G�t�F�N�g��p�ӂ��Ă�����)
            CreateDamageEffect();

            uiManager.UpdateDisplayDurabilityGauge(defenseBaseDurability, maxDefenseBaseDurability);


            if (defenseBaseDurability <= 0 && gameManager.currentGameState == GameManager.GameState.Play) {
                Debug.Log("Game Over");

                // TODO �Q�[���I�[�o�[����
                gameManager.GameOver();
            }

            // �G�̔j��
            enemyController.DestroyEnemy(false);
        }
    }

    /// <summary>
    /// �_���[�W���o����
    /// </summary>
    private void CreateDamageEffect() {
        //GameObject effect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);

        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectType.Hit_DefenseBase), transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
