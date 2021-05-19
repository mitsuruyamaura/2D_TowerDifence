using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("�ϋv�l")]
    public int defenseBaseDurability;

    [SerializeField, HideInInspector]
    private GameObject damageEffectPrefab;  // �G���Əd������̂ň�U�Ȃ��ɂ��ĕۗ�

    private GameManager gameManager;

    /// <summary>
    /// �ݒ�
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

            // �_���[�W���o����
            //CreateDamageEffect();

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
        GameObject effect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
}
