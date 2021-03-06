using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    [SerializeField, Header("攻撃力")]
    private int attackPower = 1;

    [SerializeField, Header("攻撃するまでの待機時間")]
    private float intervalAttackTime = 60.0f;

    [SerializeField]
    private bool isAttack;

    [SerializeField]
    private EnemyController enemy;

    private int attackCount = 0;     // TODO 現在の攻撃回数の残り Reactive Property にしてもいい

    [SerializeField]
    private UnityEngine.UI.Text txtAttackCount;

    [SerializeField]
    private BoxCollider2D attackRangeArea;

    [SerializeField]
    private CharaData charaData;

    private GameManager gameManager;

    private SpriteRenderer spriteRenderer;

    private Animator anim;
    private string overrideClipName = "Chara_4"; // Motion に登録されている AnimationClip の名前を登録する
    private AnimatorOverrideController overrideController;


    private void OnTriggerStay2D(Collider2D collision) {

        // 敵を未発見かつ、攻撃中ではない場合
        if (!isAttack && !enemy) {

            Debug.Log("敵発見");

            //Destroy(collision.gameObject);

            // 敵の情報(EnemyController)を取得する
            if (collision.gameObject.TryGetComponent(out enemy)) {

                // 取得できたら、攻撃の準備に入る
                isAttack = true;
                StartCoroutine(PrepareteAttack());
            }
        }
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

                        // キャラを破壊
                        DestroyChara();
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

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {

            Debug.Log("敵なし");

            isAttack = false;
            enemy = null;

        }
    }

    /// <summary>
    /// 残り攻撃回数の表示更新
    /// </summary>
    private void UpdateDisplayAttackCount() {
        txtAttackCount.text = attackCount.ToString();
    }

    /// <summary>
    /// キャラの設定
    /// </summary>
    /// <param name="charaData"></param>
    /// <param name="gameManager"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager) {
        this.charaData = charaData;
        this.gameManager = gameManager;

        if (TryGetComponent(out spriteRenderer)) {
            // １枚絵用
            //spriteRenderer.sprite = this.charaData.charaSprite;
        }

        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        attackRangeArea.size = DataBaseManager.instance.attackRangeSizeSO.GetAttackRangeSize(charaData.attackRange); //CharaDataSO.GetAttackRangeSize(this.charaData.attackRange);

        attackCount = this.charaData.maxAttackCount;

        // Editor 用
        //anim.runtimeAnimatorController = this.charaData.charaAnim;

        // キャラごとの AnimationClip を設定
        SetUpAnimation();

        UpdateDisplayAttackCount();
    }

    /// <summary>
    /// Motion に登録されている AnimationClip を変更
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
    /// キャラをタップした際の処理(EventTrigger)
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }

    //mi

    /// <summary>
    /// キャラが破壊された場合の処理
    /// </summary>
    private void DestroyChara() {

        // エフェクト
        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Destroy_Chara), transform.position, Quaternion.identity);

        // キャラ破壊
        Destroy(gameObject);

        gameManager.RemoveCharasList(this);
    }
}
