using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Transition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGameMode : MonoBehaviour
{
    [SerializeField]
    TrackingDataReciver _trackingDataReciver;
    public TextMeshProUGUI StartText;
    public float StartDelay = 1.0f;
    public float FadeTime = 1.0f;
    public ZoomEffect zoomEffect;

    [Space(10), Header("Scene")]
    public string SceneName;

    Sequence sequence;
    Coroutine coroutine;

    private void Awake() {
        SceneManager.LoadScene("Hand Tracking", LoadSceneMode.Additive);
    }

    public  void Init() {
        zoomEffect.SetEffect();
        StartCoroutine(StartingAnimation());
        _trackingDataReciver.LeftOnValueChage += PressStart;
        _trackingDataReciver.RightOnValueChange += PressStart;
    }

    private void OnDestroy() {
        _trackingDataReciver.LeftOnValueChage -= PressStart;
        _trackingDataReciver.RightOnValueChange -= PressStart;
    }

    private void PressStart(bool isNear)
    {
        if (isNear)
        {
            PressStart();
        }
    }
    public void PressStart() {
        if (coroutine != null) return;

        coroutine = StartCoroutine(NextSceneLoading());
    }

    IEnumerator StartingAnimation() {
        sequence= DotweenAnimation.FadeInOut(StartText, FadeTime, true, StartDelay);
        sequence.Play();
        yield return null;
    }

    IEnumerator NextSceneLoading() {

        DBManager.TargetBookName = "Book1";
        DBManager.NextSceneName = "CrainGame_HandTrack";
        yield return zoomEffect.RunZoomIn();
        LoadingScene.LoadScene(SceneName);
    }
}
