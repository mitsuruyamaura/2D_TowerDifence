using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���̎��
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
    private Fade fade;�@�@�@�@// FadeCanvas �Q�[���I�u�W�F�N�g���A�T�C�����邽�߂̕ϐ�

    [SerializeField, Header("�t�F�[�h�̎���")]
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
    /// �����Ŏw�肵���V�[���ւ̃V�[���J�ڂ̏���
    �@�@/// �V�[���J�ڂ����s����ꍇ�́A���̃��\�b�h�𗘗p����
    /// </summary>
    /// <param name="nextSceneType"></param>
    public void PreparateNextScene(SceneType nextSceneType) {

        // FadeCanvas �̏�񂪂��邩�Ȃ����𔻒f���āA�g�����W�V�����̋@�\���g�����A�g��Ȃ�����؂�ւ���
        if (!fade) {
            // FadeCanvas �̏�񂪂Ȃ��ꍇ�A���܂܂łƓ����悤�ɂ����ɃV�[���J��
            StartCoroutine(LoadNextScene(nextSceneType));
        } else {
            // FadeCanvas �̏�񂪂���ꍇ�AfadeDuration �ϐ��b�̎��Ԃ������ăt�F�[�h�C���̏������s���Ă���A�����Ŏw�肵���V�[���֑J��
            fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadNextScene(nextSceneType)); });
        }
    }

    /// <summary>
    /// �����Ŏw�肵���V�[���֑J��
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadNextScene(SceneType nextLoadSceneName) {

        // �V�[�������w�肷������ɂ́Aenum �ł��� SceneType �̗񋓎q���A ToString ���\�b�h���g���� string �^�փL���X�g���ė��p
        SceneManager.LoadScene(nextLoadSceneName.ToString());

        // �t�F�[�h�C�����Ă���ꍇ�ɂ�
        if (fade) {

            // �ǂݍ��񂾐V�����V�[���̏����擾
            Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());

            // �V�[���̓ǂݍ��ݏI����҂�
            yield return new WaitUntil(() => scene.isLoaded);

            // �V�[���̓ǂݍ��ݏI�����Ă���t�F�[�h�A�E�g���āA��ʓ]������������
            fade.FadeOut(fadeDuration);
        }
    }
}
