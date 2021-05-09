using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �V�[���J�ڂ̏���
    /// </summary>
    public void PreparateNextScene(SceneName nextSceneName) {
        StartCoroutine(NextScene(nextSceneName));
    }

    /// <summary>
    /// �w�肵���V�[���ւ̑J�ڏ���
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <returns></returns>
    public IEnumerator NextScene(SceneName nextSceneName) {
        yield return null;

        SceneManager.LoadScene(nextSceneName.ToString());
    }
}
