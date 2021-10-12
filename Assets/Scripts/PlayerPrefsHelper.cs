using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �w�肵���N���X�� string �^�� Json �`���� PlayerPrefs �N���X�ɃZ�[�u�E���[�h���邽�߂̃w���p�[�N���X
/// </summary>
public static class PlayerPrefsHelper {

    /// <summary>
    /// �w�肵���L�[�̃f�[�^�����݂��Ă��邩�m�F
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ExistsData(string key) {

        // �w�肵���L�[�̃f�[�^�����݂��Ă��邩�m�F���āA���݂��Ă���ꍇ�� true �A���݂��Ă��Ȃ��ꍇ�ɂ� false ��߂�
        return PlayerPrefs.HasKey(key);
    }

    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̃f�[�^���Z�[�u
    /// </summary>
    /// <typeparam name="T">�Z�[�u����^</typeparam>
    /// <param name="key">�f�[�^�����ʂ��邽�߂̃L�[</param>
    /// <param name="obj">�Z�[�u������</param>
    public static void SaveSetObjectData<T>(string key, T obj) {

        // �I�u�W�F�N�g�̃f�[�^�� Json �`���ɕϊ�
        string json = JsonUtility.ToJson(obj);

        // �Z�b�g
        PlayerPrefs.SetString(key, json);

        // �Z�b�g���� Key �� json ���Z�[�u
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̃f�[�^�����[�h
    /// </summary>
    /// <typeparam name="T">���[�h����^</typeparam>
    /// <param name="key">�f�[�^�����ʂ��邽�߂̃L�[</param>
    /// <returns></returns>
    public static T LoadGetObjectData<T>(string key) {

        // �Z�[�u����Ă���f�[�^�����[�h
        string json = PlayerPrefs.GetString(key);

        // �ǂݍ��ތ^���w�肵�ĕϊ����Ď擾
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// �w�肳�ꂽ�L�[�̃f�[�^���폜
    /// </summary>
    /// <param name="key"></param>
    public static void RemoveObjectData(string key) {

        // �w�肳�ꂽ�L�[�̃f�[�^���폜
        PlayerPrefs.DeleteKey(key);

        Debug.Log("�Z�[�u�f�[�^���폜�@���s : " + key);
    }

    /// <summary>
    /// ���ׂẴZ�[�u�f�[�^���폜
    /// </summary>
    public static void AllClearSaveData() {

        // ���ׂẴZ�[�u�f�[�^���폜
        PlayerPrefs.DeleteAll();

        Debug.Log("�S�Z�[�u�f�[�^���폜�@���s");
    }

    /// <summary>
    /// int �^�� List �̒l���J���}��؂��1�s�� string �^�ɕϊ�
    /// </summary>
    /// <param name="listData"></param>
    /// <returns></returns>
    public static string ConvertListToString(List<int> listData) {
        return listData.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b);
    }

    /// <summary>
    /// �J���}��؂�ɂȂ��Ă���1�s�� string �^�̒l�� int �^�� List �ɕϊ�
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static List<int> ConvertStringToList(string str) {
        return str.Split(',').ToList().ConvertAll(x => int.Parse(x));
    }
}
