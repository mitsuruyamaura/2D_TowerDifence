using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public int stageNo;
    public int generateIntervalTime;

    public int clearPoint;
    public int defenseBaseDurability;

    public Sprite stageSprite;

    public MapInfo mapInfo;

    public int maxCharaPlacementCount;
    // TODO

}
