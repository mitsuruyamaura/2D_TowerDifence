using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    [SerializeField, Header("UŒ‚—Í")]
    private int attackPower = 1;

    [SerializeField, Header("UŒ‚‚·‚é‚Ü‚Å‚Ì‘Ò‹@ŠÔ")]
    private float intervalAttackTime = 60.0f;

    [SerializeField]
    private bool isAttack;

    [SerializeField]
    private EnemyController enemy;

    private int attackCount = 0;     // TODO Œ»İ‚ÌUŒ‚‰ñ”‚Ìc‚è Reactive Property ‚É‚µ‚Ä‚à‚¢‚¢

    [SerializeField]
    private UnityEngine.UI.Text txtAttackCount;

    [SerializeField]
    private BoxCollider2D attackRangeArea;

    [SerializeField]
    private CharaData charaData;

    private GameManager gameManager;

    private SpriteRenderer spriteRenderer;

    private Animator anim;
    private string overrideClipName = "Chara_4"; // Motion ‚É“o˜^‚³‚ê‚Ä‚¢‚é AnimationClip ‚Ì–¼‘O‚ğ“o˜^‚·‚é
    private AnimatorOverrideController overrideController;


    private void OnTriggerStay2D(Collider2D collision) {

        // “G‚ğ–¢”­Œ©‚©‚ÂAUŒ‚’†‚Å‚Í‚È‚¢ê‡
        if (!isAttack && !enemy) {

            Debug.Log("“G”­Œ©");

            //Destroy(collision.gameObject);

            // “G‚Ìî•ñ(EnemyController)‚ğæ“¾‚·‚é
            if (collision.gameObject.TryGetComponent(out enemy)) {

                // æ“¾‚Å‚«‚½‚çAUŒ‚‚Ì€”õ‚É“ü‚é
                isAttack = true;
                StartCoroutine(PrepareteAttack());
            }
        }
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

                        // ƒLƒƒƒ‰‚ğ”j‰ó
                        DestroyChara();
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

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {

            Debug.Log("“G‚È‚µ");

            isAttack = false;
            enemy = null;

        }
    }

    /// <summary>
    /// c‚èUŒ‚‰ñ”‚Ì•\¦XV
    /// </summary>
    private void UpdateDisplayAttackCount() {
        txtAttackCount.text = attackCount.ToString();
    }

    /// <summary>
    /// ƒLƒƒƒ‰‚Ìİ’è
    /// </summary>
    /// <param name="charaData"></param>
    /// <param name="gameManager"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager) {
        this.charaData = charaData;
        this.gameManager = gameManager;

        if (TryGetComponent(out spriteRenderer)) {
            // ‚P–‡ŠG—p
            //spriteRenderer.sprite = this.charaData.charaSprite;
        }

        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        attackRangeArea.size = DataBaseManager.instance.attackRangeSizeSO.GetAttackRangeSize(charaData.attackRange); //CharaDataSO.GetAttackRangeSize(this.charaData.attackRange);

        attackCount = this.charaData.maxAttackCount;

        // Editor —p
        //anim.runtimeAnimatorController = this.charaData.charaAnim;

        // ƒLƒƒƒ‰‚²‚Æ‚Ì AnimationClip ‚ğİ’è
        SetUpAnimation();

        UpdateDisplayAttackCount();
    }

    /// <summary>
    /// Motion ‚É“o˜^‚³‚ê‚Ä‚¢‚é AnimationClip ‚ğ•ÏX
    /// http://tsubakit1.hateblo.jp/entry/2016/11/18/234130
    /// </summary>
    private void SetUpAnimation() {
        if (TryGetComponent(out anim)) {

            overrideController = new AnimatorOverrideController();

            overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
            anim.runtimeAnimatorController = overrideController;

            AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];

            for (int i = 0; i < anim.layerCount; i++) {
                layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
            }

            overrideController[overrideClipName] = this.charaData.charaAnim;
            //  overrideController["Chara_4"] = AnimationClip

            anim.runtimeAnimatorController = overrideController;

            anim.Update(0.0f);

            for (int i = 0; i < anim.layerCount; i++) {
                anim.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
            }
        }
    }

    /// <summary>
    /// ƒLƒƒƒ‰‚ğƒ^ƒbƒv‚µ‚½Û‚Ìˆ—(EventTrigger)
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }

    //mi

    /// <summary>
    /// ƒLƒƒƒ‰‚ª”j‰ó‚³‚ê‚½ê‡‚Ìˆ—
    /// </summary>
    private void DestroyChara() {

        // ƒGƒtƒFƒNƒg
        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Destroy_Chara), transform.position, Quaternion.identity);

        // ƒLƒƒƒ‰”j‰ó
        Destroy(gameObject);

        gameManager.RemoveCharasList(this);
    }
}
