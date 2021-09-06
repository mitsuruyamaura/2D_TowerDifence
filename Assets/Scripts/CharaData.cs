using UnityEngine;
//using UnityEditor.Animations;

/// <summary>
/// �L�����̏ڍ׃f�[�^
/// </summary>
[System.Serializable]
public class CharaData
{
    public int charaNo;
    public int cost;
    public Sprite charaSprite;
    //public Sprite[] charaSprites;
    //public AnimatorController charaAnim;
    public string charaName;

    public int attackPower;
    public AttackRangeType attackRange;
    public float intervalAttackTime;
    public int maxAttackCount;

    [Multiline]
    public string discription;

    public AnimationClip charaAnim;

    [HideInInspector]
    public int engagePoint;
    
    // TODO ���ɂ�����Βǉ�

}
