using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("�ϋv�l")]
    public int defenseBaseDurability;       // ��������ꍇ�ɂ͗��p���Ȃ�

    private int maxDefenseBaseDurability;

    private GameManager gameManager;
    private UIManager uiManager;


    // ��
    [SerializeField, HideInInspector]
    private GameObject damageEffectPrefab;  // �G���Əd������̂ň�U�Ȃ��ɂ��ĕۗ�  EffectManager �őΉ�

    [SerializeField]
    private UnityEngine.UI.Slider durabilityGauge;

    /// <summary>
    /// �ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager, int defenseBaseDurability, UIManager uiManager) {
        this.gameManager = gameManager;
        this.uiManager = uiManager;

        // �P�̏ꍇ
        //uiManager.SetDurabilityGauge(durabilityGauge);

        // �����̏ꍇ(���_��̃Q�[�W�͉B��)
        durabilityGauge.gameObject.SetActive(false);

        if (GameData.instance.isDebug) {
            maxDefenseBaseDurability = GameData.instance.defenseBaseDurability;
        } else {
            maxDefenseBaseDurability = defenseBaseDurability;
        }

        // �P�̏ꍇ
        //this.defenseBaseDurability = maxDefenseBaseDurability;
        //uiManager.UpdateDisplayDurabilityGauge(this.defenseBaseDurability, maxDefenseBaseDurability);

        // �����̏ꍇ
        GameData.instance.defenseBaseDurability = maxDefenseBaseDurability;
        uiManager.UpdateDipslayMultipleDefenseBaseDurabilityGauge(GameData.instance.defenseBaseDurability, maxDefenseBaseDurability);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {

            // �P�����Ȃ��ꍇ 
            //defenseBaseDurability = Mathf.Clamp(defenseBaseDurability - enemyController.attackPower, 0, maxDefenseBaseDurability);

            // ������ DefenseBase ������ꍇ
            GameData.instance.defenseBaseDurability = Mathf.Clamp(GameData.instance.defenseBaseDurability - enemyController.attackPower, 0, maxDefenseBaseDurability);

            // �_���[�W���o����(�ϋv�� 0 �ɂȂ����ꍇ�ŕʂ̃G�t�F�N�g��p�ӂ��Ă�����)
            CreateDamageEffect();

            // �P�����Ȃ��ꍇ 
            //uiManager.UpdateDisplayDurabilityGauge(defenseBaseDurability, maxDefenseBaseDurability);

            // ������ DefenseBase ������ꍇ
            uiManager.UpdateDipslayMultipleDefenseBaseDurabilityGauge(GameData.instance.defenseBaseDurability, maxDefenseBaseDurability);

            // �����̏ꍇ
            if (GameData.instance.defenseBaseDurability <= 0 && gameManager.currentGameState == GameManager.GameState.Play) {
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

        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Hit_DefenseBase), transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
