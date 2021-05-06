using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaController : MonoBehaviour
{
    public bool isAttack;

    public EnemyController enemy;


    public void SetUpChara() {
        

    }

    public IEnumerator PrepareteAttack() {
        Debug.Log("攻撃準備開始");
        int timer = 0;

        while (isAttack) {

            timer++;
            if(timer > 60) {

                timer = 0;
                Attack();
            }

            yield return null;
        }
    }

    private void Attack() {
        Debug.Log("攻撃");

        Destroy(enemy.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        // 敵を未発見かつ、攻撃中ではない場合
        if (collision.tag == "Enemy" && !isAttack) {

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
