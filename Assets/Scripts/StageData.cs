using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public int stageNo;
    public int generateIntervalTime;
    public Vector2Int[] enemys;            // x = 敵の通し番号。-1 はランダムな敵。y = 敵の出現地点。-1 はランダムな地点
    public PathData[] enemyPathDatas;
    public int clearPoint;
    public int defenseBaseDurability;

    // TODO

}
