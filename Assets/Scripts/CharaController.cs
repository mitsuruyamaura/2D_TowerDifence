using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    [SerializeField, Header("�U����")]
    private int attackPower = 1;

    [SerializeField, Header("�U������܂ł̑ҋ@����")]
    private float intervalAttackTime = 60.0f;

    [SerializeField]
    private bool isAttack;

    [SerializeField]
    private EnemyController enemy;

    private int attackCount = 0;     // TODO ���݂̍U���񐔂̎c�� Reactive Property �ɂ��Ă�����

    [SerializeField]
    private UnityEngine.UI.Text txtAttackCount;

    [SerializeField]
    private BoxCollider2D attackRangeArea;

    [SerializeField]
    private CharaData charaData;

    private GameManager gameManager;

    private SpriteRenderer spriteRenderer;

    private Animator anim;
    private string overrideClipName = "Chara_4"; // Motion �ɓo�^����Ă��� AnimationClip �̖��O��o�^����
    private AnimatorOverrideController overrideController;


    private void OnTriggerStay2D(Collider2D collision) {

        // �G�𖢔������A�U�����ł͂Ȃ��ꍇ
        if (!isAttack && !enemy) {

            Debug.Log("�G����");

            //Destroy(collision.gameObject);

            // �G�̏��(EnemyController)���擾����
            if (collision.gameObject.TryGetComponent(out enemy)) {

                // �擾�ł�����A�U���̏����ɓ���
                isAttack = true;
                StartCoroutine(PrepareteAttack());
            }
        }
    }

    /// <summary>
    /// �U������
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareteAttack() {
        Debug.Log("�U�������J�n");
        int timer = 0;

        while (isAttack) {

            // �Q�[���v���C���̂ݍU������
            if (gameManager.currentGameState == GameManager.GameState.Play) {

                timer++;
                if (timer > intervalAttackTime) {

                    timer = 0;
                    Attack();
                    attackCount--;

                    // �c��U���񐔂̕\���X�V
                    UpdateDisplayAttackCount();

                    // �U���񐔂��Ȃ��Ȃ�����
                    if (attackCount <= 0) {

                        // �L������j��
                        DestroyChara();
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// �U��
    /// </summary>
    private void Attack() {
        Debug.Log("�U��");

        // TODO �L�����̏�ɍU���G�t�F�N�g�𐶐�

        //Destroy(enemy.gameObject);

        enemy.CulcDamage(attackPower);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {

            Debug.Log("�G�Ȃ�");

            isAttack = false;
            enemy = null;

        }
    }

    /// <summary>
    /// �c��U���񐔂̕\���X�V
    /// </summary>
    private void UpdateDisplayAttackCount() {
        txtAttackCount.text = attackCount.ToString();
    }

    /// <summary>
    /// �L�����̐ݒ�
    /// </summary>
    /// <param name="charaData"></param>
    /// <param name="gameManager"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager) {
        this.charaData = charaData;
        this.gameManager = gameManager;

        if (TryGetComponent(out spriteRenderer)) {
            // �P���G�p
            //spriteRenderer.sprite = this.charaData.charaSprite;
        }

        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        attackRangeArea.size = DataBaseManager.instance.attackRangeSizeSO.GetAttackRangeSize(charaData.attackRange); //CharaDataSO.GetAttackRangeSize(this.charaData.attackRange);

        attackCount = this.charaData.maxAttackCount;

        // Editor �p
        //anim.runtimeAnimatorController = this.charaData.charaAnim;

        // �L�������Ƃ� AnimationClip ��ݒ�
        SetUpAnimation();

        UpdateDisplayAttackCount();
    }

    /// <summary>
    /// Motion �ɓo�^����Ă��� AnimationClip ��ύX
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
    /// �L�������^�b�v�����ۂ̏���(EventTrigger)
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }

    //mi

    /// <summary>
    /// �L�������j�󂳂ꂽ�ꍇ�̏���
    /// </summary>
    private void DestroyChara() {

        // �G�t�F�N�g
        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Destroy_Chara), transform.position, Quaternion.identity);

        // �L�����j��
        Destroy(gameObject);

        gameManager.RemoveCharasList(this);
    }
}
