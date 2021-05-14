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

    private Vector3Int gridPos;

    //IEnumerator Start() {  // Debug�p
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
        CreateHaveCharaDatasList();

        yield return StartCoroutine(CreatePlacementCharaSelectPopUp());
    }

    /// <summary>
    /// �������Ă���L�����̃f�[�^�����X�g��
    /// </summary>
    private void CreateHaveCharaDatasList() {

        // �������Ă���L�����̔ԍ������ɃL�����̃f�[�^�̃��X�g���쐬
        for (int i = 0; i < GameData.instance.possessionCharaNosList.Count; i++) {
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList.Find(x => x.charaNo == GameData.instance.possessionCharaNosList[i]));
        }
    }

    /// <summary>
    /// �z�u�L�����I��p�|�b�v�A�b�v����
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePlacementCharaSelectPopUp() {

        placementCharaSelectPopUp = Instantiate(placementCharaSelectPopUpPrefab, canvasTran, false);

        // TODO ��2�����͏������Ă���L�����̃��X�g�ɕύX����
        placementCharaSelectPopUp.SetUpPlacementCharaSelectPopUp(charaDatasList, this);

        placementCharaSelectPopUp.gameObject.SetActive(false);

        yield return null;
    }


    void Update()
    {
        // �z�u�ł���ő�L�������ɒB���Ă���ꍇ�ɂ͔z�u�ł��Ȃ�
        if (GameData.instance.charaPlacementCount >= GameData.instance.maxCharaPlacementCount) {
            return;
        }

        // �^�b�v������ (���Q�[���v���C���ŁA�z�u�L�����|�b�v�A�b�v����\����ԂȂ�)
        if (Input.GetMouseButtonDown(0) && gameManager.currentGameState == GameManager.GameState.Play && !placementCharaSelectPopUp.gameObject.activeSelf) {

            // �^�b�v�����ʒu�����[���h���W�ɕϊ�
            Vector3 touchPos = Input.mousePosition;

            gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(touchPos));


            // �^�b�v�����ʒu�̃^�C���̃R���C�_�[�̏����m�F����
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {

                // �L�����z�u(�f�o�b�O�p)
                //CreateChara(gridPos);

                // �z�u�L�����I��p�|�b�v�A�b�v�̕\��
                ActivatePlacementCharaSelectPopUp();
            }
        }
    }

    /// <summary>
    /// �z�u�L�����I��p�̃|�b�v�A�b�v�̕\��
    /// </summary>
    public void ActivatePlacementCharaSelectPopUp() {

        // �Q�[���̐i�s��Ԃ��Q�[����~�ɕύX
        gameManager.SetGameState(GameManager.GameState.Stop);

        // ���ׂĂ̓G�̈ړ����ꎞ��~
        gameManager.PauseEnemies();

        // �z�u�L�����I��p�̃|�b�v�A�b�v�̕\��
        placementCharaSelectPopUp.gameObject.SetActive(true);
        placementCharaSelectPopUp.ShowPopUp();
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
    /// �I�������L�����𐶐����Ĕz�u
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="charaData"></param>
    public void CreateChooseChara(CharaData charaData) {

        // �R�X�g�x����
        GameData.instance.CurrencyReactiveProperty.Value -= charaData.cost;

        // �L�������J�E���g
        GameData.instance.charaPlacementCount++;

        // �L�������^�b�v�����ʒu�ɐ���
        CharaController chara = Instantiate(charaControllerPrefab, gridPos, Quaternion.identity);

        // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        // �L�����̐ݒ�
        chara.SetUpChara(charaData, gameManager);

        // �L������ List �ɒǉ�
        gameManager.AddCharasList(chara);
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
