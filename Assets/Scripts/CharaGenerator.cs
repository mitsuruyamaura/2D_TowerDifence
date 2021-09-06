using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private GameObject charaPrefab;

    [SerializeField]
    private CharaController charaControllerPrefab;

    [SerializeField]
    private Grid grid;         // Base 側の Grid を指定する 

    [SerializeField]
    private Tilemap tilemaps;　　　// Walk 側の Tilemap を指定する

    //[SerializeField]
    //private CharaDataSO charaDataSO;

    [SerializeField]
    private PlacementCharaSelectPopUp placementCharaSelectPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField, Header("キャラのデータリスト")]
    private List<CharaData> charaDatasList = new List<CharaData>();

    private PlacementCharaSelectPopUp placementCharaSelectPopUp;
    private GameManager gameManager;
    private Vector3Int gridPos;


    // mi
    private int maxCharaPlacementCount;


    //IEnumerator Start() {  // Debug用
    //    // 所持しているキャラのデータをリスト化
    //    yield return StartCoroutine(CreateHaveCharaDatasList());
    //}


    void Update()
    {
        // 配置できる最大キャラ数に達している場合には配置できない
        if (gameManager.GetPlacementCharaCount() >= maxCharaPlacementCount) {
            return;
        }

        // 画面をタップ(マウスクリック)したら (かつゲームプレイ中で、配置キャラポップアップが非表示状態なら)
        if (Input.GetMouseButtonDown(0) && !placementCharaSelectPopUp.gameObject.activeSelf && gameManager.currentGameState == GameManager.GameState.Play ) {

            // タップ(マウスクリック)の位置を取得
            //Vector3 touchPos = Input.mousePosition;

            // タップ(マウスクリック)の位置を取得してワールド座標に変換し、それをさらにタイルの座標に変換
            gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // タップした位置のタイルのコライダーの情報を確認する
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {

                // キャラ配置(デバッグ用)
                //CreateChara(gridPos);

                // 配置キャラ選択用ポップアップの表示
                ActivatePlacementCharaSelectPopUp();
            }
        }
    }

    ///// <summary>
    ///// キャラ生成。デバッグ用
    ///// </summary>
    ///// <param name="gridPos"></param>
    //private void CreateChara(Vector3Int gridPos) {
    //    // タップした位置のタイルのコライダーの情報を確認する
    //    //if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
    //        GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

    //        // 位置が左下を 0,0 としているので、中央にくるように調整
    //        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);
    //    //}
    //}

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpCharaGenerator(GameManager gameManager, StageData stageData) {
        this.gameManager = gameManager;

        // TODO ステージのデータを取得
        //(tilemaps, grid) = mapInfo.GetMapInfo();

        // TODO 所持しているキャラのデータをリスト化
        CreateHaveCharaDatasList();

        yield return StartCoroutine(CreatePlacementCharaSelectPopUp());

        // mi
        if (GameData.instance.isDebug) {
            maxCharaPlacementCount = GameData.instance.maxCharaPlacementCount;
        } else {
            maxCharaPlacementCount = stageData.maxCharaPlacementCount;
        }
    }

    /// <summary>
    /// 配置キャラ選択用ポップアップ生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePlacementCharaSelectPopUp() {

        placementCharaSelectPopUp = Instantiate(placementCharaSelectPopUpPrefab, canvasTran, false);

        // TODO 第2引数は所持しているキャラのリストに変更する
        placementCharaSelectPopUp.SetUpPlacementCharaSelectPopUp(this, charaDatasList);

        placementCharaSelectPopUp.gameObject.SetActive(false);

        yield return null;
    }

    /// <summary>
    /// 配置キャラ選択用のポップアップの表示
    /// </summary>
    public void ActivatePlacementCharaSelectPopUp() {

        // TODO ゲームの進行状態をゲーム停止に変更
        gameManager.SetGameState(GameManager.GameState.Stop);

        // TODO すべての敵の移動を一時停止
        gameManager.PauseEnemies();

        // 配置キャラ選択用のポップアップの表示
        placementCharaSelectPopUp.gameObject.SetActive(true);
        placementCharaSelectPopUp.ShowPopUp();
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

    /// <summary>
    /// 所持しているキャラのデータをリスト化
    /// </summary>
    private void CreateHaveCharaDatasList() {

        // 所持しているキャラの番号を元にキャラのデータのリストを作成
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++) {  // GameData.instance.possessionCharaNosList
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList[i]);  // .Find(x => x.charaNo == GameData.instance.possessionCharaNosList[i])
        }

        // CharaNo の低い順にソート
        charaDatasList = charaDatasList.OrderBy(x => x.charaNo).ToList();
    }

    /// <summary>
    /// 選択したキャラを生成して配置
    /// </summary>
    /// <param name="charaData"></param>
    public void CreateChooseChara(CharaData charaData) {

        // TODO コスト支払い
        GameData.instance.CurrencyReactiveProperty.Value -= charaData.cost;

        // キャラをタップした位置に生成
        CharaController chara = Instantiate(charaControllerPrefab, gridPos, Quaternion.identity);

        // 位置が左下を 0,0 としているので、中央にくるように調整
        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        // キャラの設定
        chara.SetUpChara(charaData, gameManager);

        // キャラを List に追加
        gameManager.AddCharasList(chara);
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
