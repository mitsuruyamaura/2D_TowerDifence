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
    /// シーン遷移の準備
    /// </summary>
    public void PreparateNextScene(SceneName nextSceneName) {
        StartCoroutine(NextScene(nextSceneName));
    }

    /// <summary>
    /// 指定したシーンへの遷移処理
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <returns></returns>
    public IEnumerator NextScene(SceneName nextSceneName) {
        yield return null;

        SceneManager.LoadScene(nextSceneName.ToString());
    }
}
