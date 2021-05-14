using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject charaPrefab;

    [SerializeField]
    private Tilemap tilemaps;　　　// Walk 側の Tilemap を指定する

    [SerializeField]
    private Grid grid;     　　　　// Base 側の Tilemap を指定する 

    //[SerializeField]
    //private CharaDataSO charaDataSO;

    [SerializeField]
    private PlacementCharaSelectPopUp placementCharaSelectPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private CharaController charaControllerPrefab;

    private PlacementCharaSelectPopUp placementCharaSelectPopUp;

    private GameManager gameManager;

    [SerializeField, Header("所持しているキャラのデータ")]
    private List<CharaData> charaDatasList = new List<CharaData>();

    private Vector3Int gridPos;

    //IEnumerator Start() {  // Debug用
    //    // 所持しているキャラのデータをリスト化
    //    yield return StartCoroutine(CreateHaveCharaDatasList());
    //}

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpCharaGenerator(GameManager gameManager) {
        this.gameManager = gameManager;

        // 所持しているキャラのデータをリスト化
        CreateHaveCharaDatasList();

        yield return StartCoroutine(CreatePlacementCharaSelectPopUp());
    }

    /// <summary>
    /// 所持しているキャラのデータをリスト化
    /// </summary>
    private void CreateHaveCharaDatasList() {

        // 所持しているキャラの番号を元にキャラのデータのリストを作成
        for (int i = 0; i < GameData.instance.possessionCharaNosList.Count; i++) {
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList.Find(x => x.charaNo == GameData.instance.possessionCharaNosList[i]));
        }
    }

    /// <summary>
    /// 配置キャラ選択用ポップアップ生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePlacementCharaSelectPopUp() {

        placementCharaSelectPopUp = Instantiate(placementCharaSelectPopUpPrefab, canvasTran, false);

        // TODO 第2引数は所持しているキャラのリストに変更する
        placementCharaSelectPopUp.SetUpPlacementCharaSelectPopUp(charaDatasList, this);

        placementCharaSelectPopUp.gameObject.SetActive(false);

        yield return null;
    }


    void Update()
    {
        // 配置できる最大キャラ数に達している場合には配置できない
        if (GameData.instance.charaPlacementCount >= GameData.instance.maxCharaPlacementCount) {
            return;
        }

        // タップしたら (かつゲームプレイ中で、配置キャラポップアップが非表示状態なら)
        if (Input.GetMouseButtonDown(0) && gameManager.currentGameState == GameManager.GameState.Play && !placementCharaSelectPopUp.gameObject.activeSelf) {

            // タップした位置をワールド座標に変換
            Vector3 touchPos = Input.mousePosition;

            gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(touchPos));


            // タップした位置のタイルのコライダーの情報を確認する
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {

                // キャラ配置(デバッグ用)
                //CreateChara(gridPos);

                // 配置キャラ選択用ポップアップの表示
                ActivatePlacementCharaSelectPopUp();
            }
        }
    }

    /// <summary>
    /// 配置キャラ選択用のポップアップの表示
    /// </summary>
    public void ActivatePlacementCharaSelectPopUp() {

        // ゲームの進行状態をゲーム停止に変更
        gameManager.SetGameState(GameManager.GameState.Stop);

        // すべての敵の移動を一時停止
        gameManager.PauseEnemies();

        // 配置キャラ選択用のポップアップの表示
        placementCharaSelectPopUp.gameObject.SetActive(true);
        placementCharaSelectPopUp.ShowPopUp();
    }

    /// <summary>
    /// キャラ生成。デバッグ用
    /// </summary>
    /// <param name="gridPos"></param>
    private void CreateChara(Vector3Int gridPos) {
        // タップした位置のタイルのコライダーの情報を確認する
        if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
            GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

            // 位置が左下を 0,0 としているので、中央にくるように調整
            chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);
        }
    }

    /// <summary>
    /// 選択したキャラを生成して配置
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="charaData"></param>
    public void CreateChooseChara(CharaData charaData) {

        // コスト支払い
        GameData.instance.CurrencyReactiveProperty.Value -= charaData.cost;

        // キャラ数カウント
        GameData.instance.charaPlacementCount++;

        // キャラをタップした位置に生成
        CharaController chara = Instantiate(charaControllerPrefab, gridPos, Quaternion.identity);

        // 位置が左下を 0,0 としているので、中央にくるように調整
        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        // キャラの設定
        chara.SetUpChara(charaData, gameManager);

        // キャラを List に追加
        gameManager.AddCharasList(chara);
    }

    /// <summary>
    /// 配置キャラ選択用のポップアップの非表示
    /// </summary>
    public void InactivatePlacementCharaSelectPopUp() {

        placementCharaSelectPopUp.gameObject.SetActive(false);

        // GameUp ではない場合
        if (gameManager.currentGameState == GameManager.GameState.Stop) {

            // ゲームの進行状態をプレイ中に変更して、ゲーム再開
            gameManager.SetGameState(GameManager.GameState.Play);

            // すべての敵の移動を再開
            gameManager.ResumeEnemies();

            // カレンシーの加算処理を再開
            StartCoroutine(gameManager.TimeToCurrency());
        }
    }




    // 未使用(タップ処理関連)

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
