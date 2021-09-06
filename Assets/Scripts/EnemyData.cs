using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string enemyName;
    public int enemyNo;
    public int hp;
    public int attackPower;
    public int moveSpeed;
    public EnemyType enemyType;

    [Header("�A�C�e���h���b�v��")]
    public int itemDropRate;

    public AnimatorOverrideController enemyOverrideController;

    // TODO

}
