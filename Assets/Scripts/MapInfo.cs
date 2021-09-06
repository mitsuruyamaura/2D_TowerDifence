using UnityEngine;
using UnityEngine.Tilemaps;


public class MapInfo : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemaps;�@�@              �@ // Walk ���� Tilemap ���w�肷��

    [SerializeField]
    private Grid grid;                            // Base ���� Grid ���w�肷�� 

    [SerializeField]
    private Transform defenceBaseTran;            // DesenseBase �𐶐�����ʒu

    /// <summary>
    /// �o������G�l�~�[�P�̕��̏��p�N���X
    /// </summary>
    [System.Serializable]
    public class AppearEnemyInfo {
        [Header("x = �G�̔ԍ��B-1 �Ȃ烉���_��")]
        public int enemyNo;
        [Header("�G�̏o���n�_�̃����_�����Btrue �Ȃ烉���_��")]
        public bool isRandomPos;

        //[Header("x = �G�̔ԍ�,y = �o���n�_")]
        //public Vector2Int enemyInfo;              // x = �G�̒ʂ��ԍ��B-1 �̓����_���ȓG�By = �G�̏o���n�_�B-1 �̓����_���Ȓn�_
        public PathData enemyPathData;            // �ړ��o�H�̏��
    }

    public AppearEnemyInfo[] appearEnemyInfos;    // �����̏o������G�l�~�[�̏����Ǘ����邽�߂̔z��


    /// <summary>
    /// �}�b�v�̏����擾
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
