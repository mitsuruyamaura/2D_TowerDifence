using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    private string overrideClipName = "Chara_4";
    private AnimatorOverrideController overrideController;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private CharaData charaData;

    [SerializeField]
    private Text txtAttackCount;

    private GameManager gameManager;

    private int attackCount = 0;     // TODO Œ»İ‚ÌUŒ‚‰ñ”‚Ìc‚è Reactive Property ‚É‚µ‚Ä‚à‚¢‚¢


    /// <summary>
    /// ƒLƒƒƒ‰‚Ìİ’è
    /// </summary>
    /// <param name="charaData"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager) {
        this.charaData = charaData;
        this.gameManager = gameManager;

        // ‚P–‡ŠG—p
        //spriteRenderer.sprite = this.charaData.charaSprite;

        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        boxCollider.size = CharaDataSO.GetAttackRangeSize(this.charaData.attackRange);

        // Editor —p
        //anim.runtimeAnimatorController = this.charaData.charaAnim;

        // ƒLƒƒƒ‰‚²‚Æ‚Ì AnimationClip ‚ğİ’è
        SetUpAnimation();

        attackCount = this.charaData.maxAttackCount;

        UpdateDisplayAttackCount();
    }

    /// <summary>
    /// UŒ‚€”õ
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareteAttack() {
        Debug.Log("UŒ‚€”õŠJn");
        int timer = 0;

        while (isAttack) {

            // ƒQ[ƒ€ƒvƒŒƒC’†‚Ì‚İUŒ‚‚·‚é
            if (gameManager.currentGameState == GameManager.GameState.Play) {

                timer++;
                if (timer > intervalAttackTime) {

                    timer = 0;
                    Attack();
                    attackCount--;

                    // c‚èUŒ‚‰ñ”‚Ì•\¦XV
                    UpdateDisplayAttackCount();

                    // UŒ‚‰ñ”‚ª‚È‚­‚È‚Á‚½‚ç
                    if (attackCount <= 0) {
                        // ƒLƒƒƒ‰”j‰ó
                        gameManager.JudgeReturnChara(true, this);
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// UŒ‚
    /// </summary>
    private void Attack() {
        Debug.Log("UŒ‚");

        // TODO ƒLƒƒƒ‰‚Ìã‚ÉUŒ‚ƒGƒtƒFƒNƒg‚ğ¶¬

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
    /// ƒLƒƒƒ‰‚ğƒ^ƒbƒv‚µ‚½Û‚Ìˆ—(EventTrigger)
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }

    /// <summary>
    /// c‚èUŒ‚‰ñ”‚Ì•\¦XV
    /// </summary>
    private void UpdateDisplayAttackCount() {
        txtAttackCount.text = attackCount.ToString();
    }

    /// <summary>
    /// Motion ‚É“o˜^‚³‚ê‚Ä‚¢‚é AnimationClip ‚ğ•ÏX
    /// http://tsubakit1.hateblo.jp/entry/2016/11/18/234130
    /// </summary>
    private void SetUpAnimation() {
        overrideController = new AnimatorOverrideController();

        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
        anim.runtimeAnimatorController = overrideController;

        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];

        for (int i = 0; i < anim.layerCount; i++) {
            layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
        }

        overrideController[overrideClipName] = this.charaData.charaAnim;

        anim.runtimeAnimatorController = overrideController;

        anim.Update(0.0f);

        for (int i = 0; i < anim.layerCount; i++) {
            anim.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }
    }
}
