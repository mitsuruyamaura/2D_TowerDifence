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
    private Tilemap tilemaps;�@�@�@// Walk ���� Tilemap ���w�肷��

    [SerializeField]
    private Grid grid;     �@�@�@�@// Base ���� Tilemap ���w�肷�� 

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

    [SerializeField, Header("�������Ă���L�����̃f�[�^")]
    private List<CharaData> charaDatasList = new List<CharaData>();


    //IEnumerator Start() {
    //    // �������Ă���L�����̃f�[�^�����X�g��
    //    yield return StartCoroutine(CreateHaveCharaDatasList());
    //}

    /// <summary>
    /// �ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpCharaGenerator(GameManager gameManager) {
        this.gameManager = gameManager;

        // �������Ă���L�����̃f�[�^�����X�g��
        yield return StartCoroutine(CreateHaveCharaDatasList());
    }

    /// <summary>
    /// �������Ă���L�����̃f�[�^�����X�g��
    /// </summary>
    private IEnumerator CreateHaveCharaDatasList() {
        yield return null;

        // �������Ă���L�����̔ԍ������ɃL�����̃f�[�^�̃��X�g���쐬
        for (int i = 0; i < GameData.instance.possessionCharaNosList.Count; i++) {
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList.Find(x => x.charaNo == GameData.instance.possessionCharaNosList[i]));
        }
    }


    void Update()
    {
        // �z�u�ł���ő�L�������ɒB���Ă���ꍇ�ɂ͔z�u�ł��Ȃ�
        if (GameData.instance.charaPlacementCount >= GameData.instance.maxCharaPlacementCount) {
            return;
        }

        // �^�b�v������ (���Q�[���v���C���Ȃ�)
        if (Input.GetMouseButtonDown(0) && gameManager.currentGameState == GameManager.GameState.Play) {

            // �^�b�v�����ʒu�� Ray ���΂�
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // �^�b�v�����ʒu�����[���h���W�ɕϊ�
            Vector3 touchPos = Input.mousePosition;
            //pos.z = 10.0f;

            //Vector3Int clickPos = tilemaps.WorldToCell(pos);

            Vector3Int gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(touchPos));


            // �^�b�v�����ʒu�̃^�C���̃R���C�_�[�̏����m�F����
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
                // �L�����z�u
                //CreateChara(gridPos);

                // �z�u�L�����I��p�|�b�v�A�b�v����
                CreatePlacementCharaSelectPopUp(gridPos);
            }



            //GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

            // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
            //chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

            //Vector3 complementPos = new Vector3(tilemaps.cellSize.x / 2, tilemaps.cellSize.y / 2);
            //Vector3 worldPos = tilemaps.CellToWorld(clickPos) + complementPos;
            //chara.transform.position = worldPos;

            //chara.transform.position = clickPos;



            //tilemaps.SetTile(clickPos, tileBas));

            //RaycastHit2D raycastHit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10.0f, LayerMask.NameToLayer("Default"));
            //Debug.Log(raycastHit.transform.position);

            // Ray �̔���




            // �ݒu�\�ȃQ�[���I�u�W�F�N�g�Ȃ�


            // TODO �L�����I��p�̃|�b�v�A�b�v�J��


            // �I�������L�������C���X�^���X����

            // https://qiita.com/keidroid/items/c4c57ca4f99e021e6ce1

            //�@https://baba-s.hatenablog.com/entry/2018/04/08/131500
        }
    }

    /// <summary>
    /// �L���������B�f�o�b�O�p
    /// </summary>
    /// <param name="gridPos"></param>
    private void CreateChara(Vector3Int gridPos) {
        // �^�b�v�����ʒu�̃^�C���̃R���C�_�[�̏����m�F����
        if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
            GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

            // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
            chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        }
    }

    /// <summary>
    /// �z�u�L�����I��p�̃|�b�v�A�b�v�̐���
    /// </summary>
    /// <param name="gridPos"></param>
    private void CreatePlacementCharaSelectPopUp(Vector3Int gridPos) {
        if (!placementCharaSelectPopUp) {

            gameManager.SetGameState(GameManager.GameState.Stop);

            gameManager.PauseEnemies();

            placementCharaSelectPopUp = Instantiate(placementCharaSelectPopUpPrefab, canvasTran, false);

            // TODO ��2�����͏������Ă���L�����̃��X�g�ɕύX����
            placementCharaSelectPopUp.SetUpPlacementCharaSelectPopUp(gridPos, charaDatasList, this);
        } 
    }

    /// <summary>
    /// �I�������L�����𐶐����Ĕz�u
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="charaData"></param>
    public void CreateChooseChara(Vector3Int gridPos, CharaData charaData) {

        // �R�X�g�x����
        GameData.instance.CurrencyReactiveProperty.Value -= charaData.cost;

        // �L�������J�E���g
        GameData.instance.charaPlacementCount++;

        CharaController chara = Instantiate(charaControllerPrefab, gridPos, Quaternion.identity);

        // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        chara.SetUpChara(charaData, gameManager);

        gameManager.AddCharasList(chara);
    }

    /// <summary>
    /// �z�u�L�����I��p�̃|�b�v�A�b�v�̔j��
    /// </summary>
    public void DestroyPlacementCharaSelectPopUp() {
        if (placementCharaSelectPopUp) {
            Destroy(placementCharaSelectPopUp.gameObject);
        }

        // GameUp �ł͂Ȃ��ꍇ
        if (gameManager.currentGameState == GameManager.GameState.Stop) {
            gameManager.SetGameState(GameManager.GameState.Play);

            gameManager.ResumeEnemies();
            StartCoroutine(gameManager.TimeToCurrency());
        }
    }
}
