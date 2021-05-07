using UnityEngine;
using UnityEditor.Animations;

/// <summary>
/// キャラの詳細データ
/// </summary>
[System.Serializable]
public class CharaData
{
    public int charaNo;
    public int cost;
    public Sprite charaSprite;
    public AnimatorController charaAnim;
    public string charaName;

    public int attackPower;
    public AttackRangeType attackRange;
    public float intervalAttackTime;

    [Multiline]
    public string discription;
    
    // TODO 他にもあれば追加

}
