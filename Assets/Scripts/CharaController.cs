using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaController : MonoBehaviour
{
    public bool isAttack;

    public EnemyController enemy;

    [Header("�U����")]
    public int attackPower = 1;

    [Header("�U������܂ł̑ҋ@����")]
    public float intervalAttackTime = 60.0f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private CharaData charaData;

    [SerializeField]
    private Text txtAttackCount;

    private GameManager gameManager;

    private int attackCount = 0;     // TODO ���݂̍U���񐔂̎c�� Reactive Property �ɂ��Ă�����


    /// <summary>
    /// �L�����̐ݒ�
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

        attackCount = this.charaData.maxAttackCount;

        UpdateDisplayAttackCount();
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
                        // �L�����j��
                        gameManager.JudgeReturnChara(true, this);
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

    private void OnTriggerStay2D(Collider2D collision) {

        // �G�𖢔������A�U�����ł͂Ȃ��ꍇ
        if (collision.tag == "Enemy" && !isAttack && !enemy) {

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

    /// <summary>
    /// �L�������^�b�v�����ۂ̏���(EventTrigger)
    /// </summary>
    public void OnClickChara() {
        gameManager.PreparateCreateReturnCharaPopUp(this);
    }

    /// <summary>
    /// �c��U���񐔂̕\���X�V
    /// </summary>
    private void UpdateDisplayAttackCount() {
        txtAttackCount.text = attackCount.ToString();
    }
}
