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
    public GameObject failButton;

    void Update()
    {
        if (!isGameClear)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= gameTime)
            {
                isGameClear = true;

                if(RhythmGameManager.Instance.Score > 10)
                {
                    ShowClearButton();
                }

                else if(RhythmGameManager.Instance.Score < 10)
                {
                    ShowFailButton();
                }

            }
        }
    }

    void ShowClearButton()
    {
        clearButton.SetActive(true);
    }

    void ShowFailButton()
    {
        failButton.SetActive(true);
    }

    public void ClearStage1()
    {
        DBManager.NextSceneName = "Antarctic_Adventure";
        RhythmGameManager.Instance.ResetGame();
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }

    public void FailStage1()
    {
        DBManager.NextSceneName = "Title";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }

    public void ClearStage2()
    {
        DBManager.NextSceneName = "Title";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }


}
