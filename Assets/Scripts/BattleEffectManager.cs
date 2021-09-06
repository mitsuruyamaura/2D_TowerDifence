using UnityEngine;

/// <summary>
/// エフェクトの種類
/// </summary>
public enum EffectType {
    Destroy_Chara,
    Destroy_Enemy,
    Hit_Enemy,
    Hit_DefenseBase,

    // TODO 他にもあれば追加
}

public class BattleEffectManager : MonoBehaviour
{
    public static BattleEffectManager instance;

    // キャラ 用
    public GameObject destroyCharaEffectPrefab;

    // エネミー用
    public GameObject destroyEnemyEffectPrefab;
    public GameObject hitEnemyEffectPrefab;

    // 拠点用
    public GameObject hitDefenseBaseEffectPrefab;


    void Awake() {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);  // シーン遷移する必要がないのでコメントアウト
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// EffectType で指定したエフェクトを取得
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
