using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string ReserveSceneName;
    public static Scene PrevScene;
    public static void LoadScene(string SceneName) {
        ReserveSceneName = SceneName;
        PrevScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    public GameObject BackgroundPrefab;
    private Image backgroundImg;

    private void Start() {
        StartCoroutine(LoadingReserveScene());
    }

    IEnumerator LoadingReserveScene() {
        Instantiate(BackgroundPrefab, transform).TryGetComponent(out backgroundImg);

        Sequence sequence = 
            backgroundImg.FadeInOut(1.0f, true)
            .Play()
            .OnComplete(()=> {
                if (PrevScene != null)
                {
                    // Debug.Log(PrevScene.name);
                    SceneManager.UnloadSceneAsync(PrevScene);
                }
            });

        while (sequence.IsPlaying()) {
            yield return null;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(ReserveSceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) {
            yield return null;
        }

        op.allowSceneActivation=true;

        while (!op.isDone) {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(ReserveSceneName));
        CamTest.Instance.CamInit();
        
        backgroundImg.FadeInOut(1.0f, false)
            .Play()
            .OnComplete(()=>SceneManager.UnloadSceneAsync("LoadingScene"));

    }
}
