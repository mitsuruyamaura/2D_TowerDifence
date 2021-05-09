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
    /// 
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager) {
        this.gameManager = gameManager;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController)) {
            defenseBaseDurability -= enemyController.attackPower;

            // �_���[�W���o����
            //CreateDamageEffect();

            if (defenseBaseDurability <= 0) {
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
