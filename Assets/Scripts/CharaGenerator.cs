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
    private Grid grid;         // Base ���� Grid ���w�肷�� 

    [SerializeField]
    private Tilemap tilemaps;�@�@�@// Walk ���� Tilemap ���w�肷��

    //[SerializeField]
    //private CharaDataSO charaDataSO;

    [SerializeField]
    private PlacementCharaSelectPopUp placementCharaSelectPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField, Header("�L�����̃f�[�^���X�g")]
    private List<CharaData> charaDatasList = new List<CharaData>();

    private PlacementCharaSelectPopUp placementCharaSelectPopUp;
    private GameManager gameManager;
    private Vector3Int gridPos;


    // mi
    private int maxCharaPlacementCount;


    //IEnumerator Start() {  // Debug�p
    //    // �������Ă���L�����̃f�[�^�����X�g��
    //    yield return StartCoroutine(CreateHaveCharaDatasList());
    //}


    void Update()
    {
        // �z�u�ł���ő�L�������ɒB���Ă���ꍇ�ɂ͔z�u�ł��Ȃ�
        if (gameManager.GetPlacementCharaCount() >= maxCharaPlacementCount) {
            return;
        }

        // ��ʂ��^�b�v(�}�E�X�N���b�N)������ (���Q�[���v���C���ŁA�z�u�L�����|�b�v�A�b�v����\����ԂȂ�)
        if (Input.GetMouseButtonDown(0) && !placementCharaSelectPopUp.gameObject.activeSelf && gameManager.currentGameState == GameManager.GameState.Play ) {

            // �^�b�v(�}�E�X�N���b�N)�̈ʒu���擾
            //Vector3 touchPos = Input.mousePosition;

            // �^�b�v(�}�E�X�N���b�N)�̈ʒu���擾���ă��[���h���W�ɕϊ����A���������Ƀ^�C���̍��W�ɕϊ�
            gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // �^�b�v�����ʒu�̃^�C���̃R���C�_�[�̏����m�F����
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {

                // �L�����z�u(�f�o�b�O�p)
                //CreateChara(gridPos);

                // �z�u�L�����I��p�|�b�v�A�b�v�̕\��
                ActivatePlacementCharaSelectPopUp();
            }
        }
    }

    ///// <summary>
    ///// �L���������B�f�o�b�O�p
    ///// </summary>
    ///// <param name="gridPos"></param>
    //private void CreateChara(Vector3Int gridPos) {
    //    // �^�b�v�����ʒu�̃^�C���̃R���C�_�[�̏����m�F����
    //    //if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
    //        GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

    //        // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
    //        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);
    //    //}
    //}

    /// <summary>
    /// �ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpCharaGenerator(GameManager gameManager, StageData stageData) {
        this.gameManager = gameManager;

        // TODO �X�e�[�W�̃f�[�^���擾
        //(tilemaps, grid) = mapInfo.GetMapInfo();

        // TODO �������Ă���L�����̃f�[�^�����X�g��
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
    /// �z�u�L�����I��p�|�b�v�A�b�v����
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePlacementCharaSelectPopUp() {

        placementCharaSelectPopUp = Instantiate(placementCharaSelectPopUpPrefab, canvasTran, false);

        // TODO ��2�����͏������Ă���L�����̃��X�g�ɕύX����
        placementCharaSelectPopUp.SetUpPlacementCharaSelectPopUp(this, charaDatasList);

        placementCharaSelectPopUp.gameObject.SetActive(false);

        yield return null;
    }

    /// <summary>
    /// �z�u�L�����I��p�̃|�b�v�A�b�v�̕\��
    /// </summary>
    public void ActivatePlacementCharaSelectPopUp() {

        // TODO �Q�[���̐i�s��Ԃ��Q�[����~�ɕύX
        gameManager.SetGameState(GameManager.GameState.Stop);

        // TODO ���ׂĂ̓G�̈ړ����ꎞ��~
        gameManager.PauseEnemies();

        // �z�u�L�����I��p�̃|�b�v�A�b�v�̕\��
        placementCharaSelectPopUp.gameObject.SetActive(true);
        placementCharaSelectPopUp.ShowPopUp();
    }

    /// <summary>
    /// �z�u�L�����I��p�̃|�b�v�A�b�v�̔�\��
    /// </summary>
    public void InactivatePlacementCharaSelectPopUp() {

        placementCharaSelectPopUp.gameObject.SetActive(false);

        // GameUp �ł͂Ȃ��ꍇ
        if (gameManager.currentGameState == GameManager.GameState.Stop) {

            // �Q�[���̐i�s��Ԃ��v���C���ɕύX���āA�Q�[���ĊJ
            gameManager.SetGameState(GameManager.GameState.Play);

            // ���ׂĂ̓G�̈ړ����ĊJ
            gameManager.ResumeEnemies();

            // �J�����V�[�̉��Z�������ĊJ
            StartCoroutine(gameManager.TimeToCurrency());
        }
    }

    /// <summary>
    /// �������Ă���L�����̃f�[�^�����X�g��
    /// </summary>
    private void CreateHaveCharaDatasList() {

        // �������Ă���L�����̔ԍ������ɃL�����̃f�[�^�̃��X�g���쐬
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++) {  // GameData.instance.possessionCharaNosList
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList[i]);  // .Find(x => x.charaNo == GameData.instance.possessionCharaNosList[i])
        }

        // CharaNo �̒Ⴂ���Ƀ\�[�g
        charaDatasList = charaDatasList.OrderBy(x => x.charaNo).ToList();
    }

    /// <summary>
    /// �I�������L�����𐶐����Ĕz�u
    /// </summary>
    /// <param name="charaData"></param>
    public void CreateChooseChara(CharaData charaData) {

        // TODO �R�X�g�x����
        GameData.instance.CurrencyReactiveProperty.Value -= charaData.cost;

        // �L�������^�b�v�����ʒu�ɐ���
        CharaController chara = Instantiate(charaControllerPrefab, gridPos, Quaternion.identity);

        // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        // �L�����̐ݒ�
        chara.SetUpChara(charaData, gameManager);

        // �L������ List �ɒǉ�
        gameManager.AddCharasList(chara);
    }




    // ���g�p(�^�b�v�����֘A)

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
