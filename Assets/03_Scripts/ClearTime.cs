using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTime: MonoBehaviour
{
    public float gameTime = 5f; 
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
    }

    public void ClearStage1()
    {
        DBManager.NextSceneName = "Antarctic_Adventure";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }

    public void ClearStage2()
    {
        DBManager.NextSceneName = "Title";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }
}
