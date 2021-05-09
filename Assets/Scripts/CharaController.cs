using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;  // EventTrigger —˜—p 

public class CharaController : MonoBehaviour
{
    public bool isAttack;

    public EnemyController enemy;

    [Header("UŒ‚—Í")]
    public int attackPower = 1;

    [Header("UŒ‚‚·‚é‚Ü‚Å‚Ì‘Ò‹@ŠÔ")]
    public float intervalAttackTime = 60.0f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private CharaData charaData;

    private GameManager gameManager;


    /// <summary>
    /// ƒLƒƒƒ‰‚Ìİ’è
    /// </summary>
    /// <param name="charaData"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager) {
        this.charaData = charaData;
        this.gameManager = gameManager;

        spriteRenderer.sprite = this.charaData.charaSprite;

        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        boxCollider.size = CharaDataSO.GetAttackRangeSize(charaData.attackRange);

        anim.runtimeAnimatorController = this.charaData.charaAnim;
    }

    /// <summary>
    /// UŒ‚€”õ
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareteAttack() {
        Debug.Log("UŒ‚€”õŠJn");
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

    /// <summary>
    /// UŒ‚
    /// </summary>
    private void Attack() {
        Debug.Log("UŒ‚");

        //Destroy(enemy.gameObject);

        enemy.CulcDamage(attackPower);
    }

    private void OnTriggerStay2D(Collider2D collision) {

        // “G‚ğ–¢”­Œ©‚©‚ÂAUŒ‚’†‚Å‚Í‚È‚¢ê‡
        if (collision.tag == "Enemy" && !isAttack && !enemy) {

            Debug.Log("“G”­Œ©");

            if (collision.gameObject.TryGetComponent(out enemy)) {
                isAttack = true;
                StartCoroutine(PrepareteAttack());
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {

            Debug.Log("“G‚È‚µ");

            isAttack = false;
            enemy = null;

        }
    }

    /// <summary>
    /// ƒLƒƒƒ‰‚ğƒ^ƒbƒv‚µ‚½Û‚Ìˆ—
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }
}
