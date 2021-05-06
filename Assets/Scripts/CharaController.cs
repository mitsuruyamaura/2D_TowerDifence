using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaController : MonoBehaviour
{
    public bool isAttack;

    public EnemyController enemy;

    [Header("攻撃力")]
    public int attackPower = 1;

    [Header("攻撃するまでの待機時間")]
    public float intervalAttackTime = 60.0f;


    public void SetUpChara() {
        

    }

    public IEnumerator PrepareteAttack() {
        Debug.Log("攻撃準備開始");
        int timer = 0;

        while (isAttack) {

            timer++;
            if(timer > intervalAttackTime) {

                timer = 0;
                Attack();
            }

            yield return null;
        }
    }

    private void Attack() {
        Debug.Log("攻撃");

        //Destroy(enemy.gameObject);

        enemy.CulcDamage(attackPower);
    }

    private void OnTriggerStay2D(Collider2D collision) {

        // 敵を未発見かつ、攻撃中ではない場合
        if (collision.tag == "Enemy" && !isAttack && !enemy) {

            Debug.Log("敵発見");

            if (collision.gameObject.TryGetComponent(out enemy)) {
                isAttack = true;
                StartCoroutine(PrepareteAttack());
            }
        }

    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {

            Debug.Log("敵なし");

            isAttack = false;
            enemy = null;

        }
    }
}
