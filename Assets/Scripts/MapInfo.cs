using UnityEngine;
using UnityEngine.Tilemaps;


public class MapInfo : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemaps;　　　// Walk 側の Tilemap を指定する

    [SerializeField]
    private Grid grid;         // Base 側の Grid を指定する 

    [SerializeField]
    private Transform defenceBaseTran;

    public AppearEnemyInfo[] appearEnemyInfos;

    [System.Serializable]
    public class AppearEnemyInfo {
        public int enemyNo;    // enemyInfo を利用しない場合に使う
        public int pathNo;     // enemyInfo を利用しない場合に使う
        public Vector2Int enemyInfo;            // x = 敵の通し番号。-1 はランダムな敵。y = 敵の出現地点。-1 はランダムな地点
        public PathData enemyPathData;
    }

    /// <summary>
    /// ステージの情報を取得
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
