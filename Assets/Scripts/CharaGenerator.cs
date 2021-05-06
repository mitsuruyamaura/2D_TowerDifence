using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject charaPrefab;

    [SerializeField]
    private Tilemap tilemaps;�@�@�@// Walk ���� Tilemap ���w�肷��

    [SerializeField]
    private Grid grid;     �@�@�@�@// Base ���� Tilemap ���w�肷�� 


    void Update()
    {
        // �^�b�v������
        if (Input.GetMouseButtonDown(0)) {

            // �^�b�v�����ʒu�� Ray ���΂�
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // �^�b�v�����ʒu�����[���h���W�ɕϊ�
            Vector3 touchPos = Input.mousePosition;
            //pos.z = 10.0f;

            //Vector3Int clickPos = tilemaps.WorldToCell(pos);

            Vector3Int gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(touchPos));


            // �^�b�v�����ʒu�̃^�C���̃R���C�_�[�̏����m�F����
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None) {
                GameObject chara = Instantiate(charaPrefab, gridPos, Quaternion.identity);

                // �ʒu�������� 0,0 �Ƃ��Ă���̂ŁA�����ɂ���悤�ɒ���
                chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

                chara.GetComponent<CharaController>().SetUpChara();
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
}
