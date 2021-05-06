using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject charaPrefab;

    [SerializeField]
    private Tilemap tilemaps;　　　// Walk 側の Tilemap を指定する

    [SerializeField]
    private Grid grid;     　　　　// Base 側の Tilemap を指定する 


    void Update()
    {
        // タップしたら
        if (Input.GetMouseButtonDown(0)) {

            // タップした位置に Ray を飛ばす
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // タップした位置をワールド座標に変換
            Vector3 touchPos = Input.mousePosition;
            //pos.z = 10.0f;

            //Vector3Int clickPos = tilemaps.WorldToCell(pos);

            Vector3Int gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(touchPos));


            // タップした位置のタイルのコライダーの情報を確認する
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
                GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

                // 位置が左下を 0,0 としているので、中央にくるように調整
                chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

                chara.GetComponent<CharaController>().SetUpChara();
            }



            //GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

            // 位置が左下を 0,0 としているので、中央にくるように調整
            //chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

            //Vector3 complementPos = new Vector3(tilemaps.cellSize.x / 2, tilemaps.cellSize.y / 2);
            //Vector3 worldPos = tilemaps.CellToWorld(clickPos) + complementPos;
            //chara.transform.position = worldPos;

            //chara.transform.position = clickPos;



            //tilemaps.SetTile(clickPos, tileBas));

            //RaycastHit2D raycastHit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10.0f, LayerMask.NameToLayer("Default"));
            //Debug.Log(raycastHit.transform.position);

            // Ray の判定




            // 設置可能なゲームオブジェクトなら


            // TODO キャラ選択用のポップアップ開く


            // 選択したキャラをインスタンスする

            // https://qiita.com/keidroid/items/c4c57ca4f99e021e6ce1

            //　https://baba-s.hatenablog.com/entry/2018/04/08/131500
        }
    }
}
