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
        Debug.Log("�U�������J�n");
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
        Debug.Log("�U��");

        Destroy(enemy.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        // �G�𖢔������A�U�����ł͂Ȃ��ꍇ
        if (collision.tag == "Enemy" && !isAttack) {

            Debug.Log("�G����");

            if (collision.gameObject.TryGetComponent(out enemy)) {
                isAttack = true;
                StartCoroutine(PrepareteAttack());
            }
        }

    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {

            Debug.Log("�G�Ȃ�");

            isAttack = false;
            enemy = null;

        }
    }
}
