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

    [Header("アイテムドロップ率")]
    public int itemDropRate;

    // TODO

}
