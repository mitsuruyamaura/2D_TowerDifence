using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackRangeSizeSO", menuName = "Create AttackRangeSizeSO")]
public class AttackRangeSizeSO : ScriptableObject
{
    public List<AttackRangeSize> attackRangeSizesList = new List<AttackRangeSize>();

    public Vector2 GetAttackRangeSize(AttackRangeType attackRangeType) {
        return attackRangeSizesList.Find(x => x.attackRangeType == attackRangeType).size;
    }
}
