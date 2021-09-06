using UnityEngine;
using UnityEngine.Tilemaps;


public class MapInfo : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemaps;　　              　 // Walk 側の Tilemap を指定する

    [SerializeField]
    private Grid grid;                            // Base 側の Grid を指定する 

    [SerializeField]
    private Transform defenceBaseTran;            // DesenseBase を生成する位置

    /// <summary>
    /// 出現するエネミー１体分の情報用クラス
    /// </summary>
    [System.Serializable]
    public class AppearEnemyInfo {
        [Header("x = 敵の番号。-1 ならランダム")]
        public int enemyNo;
        [Header("敵の出現地点のランダム化。true ならランダム")]
        public bool isRandomPos;

        //[Header("x = 敵の番号,y = 出現地点")]
        //public Vector2Int enemyInfo;              // x = 敵の通し番号。-1 はランダムな敵。y = 敵の出現地点。-1 はランダムな地点
        public PathData enemyPathData;            // 移動経路の情報
    }

    public AppearEnemyInfo[] appearEnemyInfos;    // 複数の出現するエネミーの情報を管理するための配列


    /// <summary>
    /// マップの情報を取得
    /// </summary>
    /// <returns></returns>
    public (Tilemap, Grid) GetMapInfo() {
        return (tilemaps, grid);
    }

    /// <summary>
    /// 防衛拠点の情報を取得
    /// </summary>
    /// <returns></returns>
    public Transform GetDefenseBaseTran() {
        return defenceBaseTran;
    }
}
