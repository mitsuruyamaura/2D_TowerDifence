using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンの種類
/// </summary>
public enum SceneType {
    //Main,
    Battle,
    World,
}

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    [SerializeField]
    private Fade fade;　　　　// FadeCanvas ゲームオブジェクトをアサインするための変数

    [SerializeField, Header("フェードの時間")]
    private float fadeDuration = 1.0f;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 引数で指定したシーンへのシーン遷移の準備
    　　/// シーン遷移を実行する場合は、このメソッドを利用する
    /// </summary>
    /// <param name="nextSceneType"></param>
    public void PreparateNextScene(SceneType nextSceneType) {

        // FadeCanvas の情報があるかないかを判断して、トランジションの機能を使うか、使わないかを切り替える
        if (!fade) {
            // FadeCanvas の情報がない場合、いままでと同じようにすぐにシーン遷移
            StartCoroutine(LoadNextScene(nextSceneType));
        } else {
            // FadeCanvas の情報がある場合、fadeDuration 変数秒の時間をかけてフェードインの処理を行ってから、引数で指定したシーンへ遷移
            fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadNextScene(nextSceneType)); });
        }
    }

    /// <summary>
    /// 引数で指定したシーンへ遷移
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadNextScene(SceneType nextLoadSceneName) {

        // シーン名を指定する引数には、enum である SceneType の列挙子を、 ToString メソッドを使って string 型へキャストして利用
        SceneManager.LoadScene(nextLoadSceneName.ToString());

        // フェードインしている場合には
        if (fade) {

            // 読み込んだ新しいシーンの情報を取得
            Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());

            // シーンの読み込み終了を待つ
            yield return new WaitUntil(() => scene.isLoaded);

            // シーンの読み込み終了してからフェードアウトして、場面転換を完了する
            fade.FadeOut(fadeDuration);
        }
    }
}
