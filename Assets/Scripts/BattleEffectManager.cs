using UnityEngine;

/// <summary>
/// �G�t�F�N�g�̎��
/// </summary>
public enum EffectType {
    Destroy_Chara,
    Destroy_Enemy,
    Hit_Enemy,
    Hit_DefenseBase,

    // TODO ���ɂ�����Βǉ�
}

public class BattleEffectManager : MonoBehaviour
{
    public static BattleEffectManager instance;

    // �L���� �p
    public GameObject destroyCharaEffectPrefab;

    // �G�l�~�[�p
    public GameObject destroyEnemyEffectPrefab;
    public GameObject hitEnemyEffectPrefab;

    // ���_�p
    public GameObject hitDefenseBaseEffectPrefab;


    void Awake() {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);  // �V�[���J�ڂ���K�v���Ȃ��̂ŃR�����g�A�E�g
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// EffectType �Ŏw�肵���G�t�F�N�g���擾
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>

    public GameObject GetEffect(EffectType effectType) {
        return effectType switch {
            EffectType.Destroy_Chara => destroyCharaEffectPrefab,
            EffectType.Destroy_Enemy => destroyEnemyEffectPrefab,
            EffectType.Hit_Enemy => hitEnemyEffectPrefab,
            EffectType.Hit_DefenseBase => hitDefenseBaseEffectPrefab,
            _ => destroyCharaEffectPrefab,
        };
    }
}
