using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpMode : MonoBehaviour
{
    private void Awake() {
        SceneManager.LoadScene("Hand Tracking", LoadSceneMode.Additive);
    }

    private void Start() {
        LoadingScene.LoadScene("Title");
    }
}
