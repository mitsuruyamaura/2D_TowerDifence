using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public int stageNo;
    public int generateIntervalTime;
    public Vector2Int[] enemys;            // x = �G�̒ʂ��ԍ��B-1 �̓����_���ȓG�By = �G�̏o���n�_�B-1 �̓����_���Ȓn�_
    public PathData[] enemyPathDatas;
    public int clearPoint;
    public int defenseBaseDurability;

    // TODO

}
