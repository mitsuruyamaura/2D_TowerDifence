using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaController : MonoBehaviour
{
    public bool isAttack;

    public EnemyController enemy;

    [Header("攻撃力")]
    public int attackPower = 1;

    [Header("攻撃するまでの待機時間")]
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

    private int attackCount = 0;     // TODO 現在の攻撃回数の残り Reactive Property にしてもいい


    /// <summary>
    /// キャラの設定
    /// </summary>
    /// <param name="charaData"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager) {
        this.charaData = charaData;
        this.gameManager = gameManager;

        // １枚絵用
        //spriteRenderer.sprite = this.charaData.charaSprite;

        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        boxCollider.size = CharaDataSO.GetAttackRangeSize(this.charaData.attackRange);

        // Editor 用
        //anim.runtimeAnimatorController = this.charaData.charaAnim;

        // キャラごとの AnimationClip を設定
        SetUpAnimation();

        attackCount = this.charaData.maxAttackCount;

        UpdateDisplayAttackCount();
    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareteAttack() {
        Debug.Log("攻撃準備開始");
        int timer = 0;

        while (isAttack) {

            // ゲームプレイ中のみ攻撃する
            if (gameManager.currentGameState == GameManager.GameState.Play) {

                timer++;
                if (timer > intervalAttackTime) {

                    timer = 0;
                    Attack();
                    attackCount--;

                    // 残り攻撃回数の表示更新
                    UpdateDisplayAttackCount();

                    // 攻撃回数がなくなったら
                    if (attackCount <= 0) {
                        // キャラ破壊
                        gameManager.JudgeReturnChara(true, this);
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack() {
        Debug.Log("攻撃");

        // TODO キャラの上に攻撃エフェクトを生成

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

    /// <summary>
    /// キャラをタップした際の処理(EventTrigger)
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }

    /// <summary>
    /// 残り攻撃回数の表示更新
    /// </summary>
    private void UpdateDisplayAttackCount() {
        txtAttackCount.text = attackCount.ToString();
    }

    /// <summary>
    /// Motion に登録されている AnimationClip を変更
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
