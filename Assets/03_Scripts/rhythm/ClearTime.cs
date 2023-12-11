using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearTime: MonoBehaviour
{
    public float gameTime = 20f; 
    private float currentTime = 0f; 
    private bool isGameClear = false; 

    public GameObject clearButton;

    void Update()
    {
        if (!isGameClear)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= gameTime)
            {
                isGameClear = true;
                    ShowClearButton();
            }
        }
    }

    void ShowClearButton()
    {
        clearButton.SetActive(true);
        Invoke("ClearStage1", 1.0f);
    }


    public void ClearStage1()
    {
        RhythmGameManager.Instance.ResetGame();
        DBManager.TargetBookName = "Book2";
        DBManager.NextSceneName = "Antarctic_Adventure";
        LoadingScene.LoadScene("CutScene");
 /*       LoadingScene.LoadScene(DBManager.NextSceneName);*/
    }

    public void FailStage1()
    {
        DBManager.NextSceneName = "Title";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }


}
