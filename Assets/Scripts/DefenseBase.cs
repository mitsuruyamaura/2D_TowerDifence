using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [Header("�ϋv�l")]
    public int defenseBaseDurability;

    [SerializeField]
    private GameObject damageEffectPrefab;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
            defenseBaseDurability -= enemyController.attackPower;

            // �_���[�W���o����
            CreateDamageEffect();

            if (defenseBaseDurability <= 0) {
                Debug.Log("Game Over");

                // TODO �Q�[���I�[�o�[����
            }

            // �G�̔j��
            enemyController.DestroyEnemy();
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
