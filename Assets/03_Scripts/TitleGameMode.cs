using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Transition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGameMode : MonoBehaviour
{
    public TextMeshProUGUI StartText;
    public float StartDelay = 1.0f;
    public float FadeTime = 1.0f;
    public ZoomEffect zoomEffect;

    [Space(10), Header("Scene")]
    public string SceneName;

    Sequence sequence;

    private void Start() {
        zoomEffect.SetEffect();
        StartCoroutine(StartingAnimation());
    }

    public void PressStart() {
        StartCoroutine(NextSceneLoading());
    }

    IEnumerator StartingAnimation() {
        sequence= DotweenAnimation.FadeInOut(StartText, FadeTime, true, StartDelay);
        sequence.Play();
        yield return null;
    }

    IEnumerator NextSceneLoading() {
        if (DBManager.Manager == null) {
            Debug.LogError("Missing DBManager");
        }
        DBManager.Manager.TargetBookName = "Book1";
        DBManager.Manager.NextSceneName = "CrainGame";
        yield return zoomEffect.RunZoomIn();
        LoadingScene.LoadScene(SceneName);
    }
}
