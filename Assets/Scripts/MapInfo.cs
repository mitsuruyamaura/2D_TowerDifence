using UnityEngine;
using UnityEngine.Tilemaps;


public class MapInfo : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemaps;�@�@�@// Walk ���� Tilemap ���w�肷��

    [SerializeField]
    private Grid grid;         // Base ���� Grid ���w�肷�� 

    [SerializeField]
    private Transform defenceBaseTran;

    public AppearEnemyInfo[] appearEnemyInfos;

    [System.Serializable]
    public class AppearEnemyInfo {
        public int enemyNo;    // enemyInfo �𗘗p���Ȃ��ꍇ�Ɏg��
        public int pathNo;     // enemyInfo �𗘗p���Ȃ��ꍇ�Ɏg��
        public Vector2Int enemyInfo;            // x = �G�̒ʂ��ԍ��B-1 �̓����_���ȓG�By = �G�̏o���n�_�B-1 �̓����_���Ȓn�_
        public PathData enemyPathData;
    }

    /// <summary>
    /// �X�e�[�W�̏����擾
    /// </summary>
    /// <returns></returns>
    public (Tilemap, Grid) GetMapInfo() {
        return (tilemaps, grid);
    }

    /// <summary>
    /// �h�q���_�̏����擾
    /// </summary>
    /// <returns></returns>
    public Transform GetDefenseBaseTran() {
        return defenceBaseTran;
    }
}
