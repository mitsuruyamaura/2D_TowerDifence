using UnityEngine;

/// <summary>
/// �L�����̏ڍ׃f�[�^
/// </summary>
[System.Serializable]
public class CharaData
{
    public int charaNo;
    public int cost;
    public Sprite charaSprite;
    public string charaName;

    public int attackPower;
    public AttackRangeType attackRange;
    public float intervalAttackTime;

    [Multiline]
    public string discription;
    
    // TODO ���ɂ�����Βǉ�

}
