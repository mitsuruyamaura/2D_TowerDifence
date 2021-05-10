using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public StageDataSO stageDataSO;
    public EnemyDataSO enemyDataSO;
    public CharaDataSO charaDataSO;


    void Awake() {
        if (instance = null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// AttackRangeType ‚©‚ç BoxCollier —p‚Ì Size ‚ðŽæ“¾
    /// </summary>
    /// <param name="attackRangeType"></param>
    /// <returns></returns>
    public static Vector2 GetAttackRangeSize(AttackRangeType attackRangeType) {
        switch (attackRangeType) {
            case AttackRangeType.Short:
                return Vector2.one * 2.0f;
            case AttackRangeType.Middle:
                return Vector2.one * 3.0f;
            case AttackRangeType.Long:
                return Vector2.one * 4.0f;
            default:
                return Vector2.one;
        }
    }
}
