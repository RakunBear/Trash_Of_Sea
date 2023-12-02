using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameManager : MonoBehaviour
{
    private static RhythmGameManager instance;
    public TextMeshProUGUI scoreText;

    public static RhythmGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RhythmGameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("RhythmGameManager");
                    instance = obj.AddComponent<RhythmGameManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField]
    private int score = 0;

    public int Score
    {
        get { return score; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateScoreText();
    }

    public void IncreaseScore()
    {
        score ++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void ResetGame()
    {
        score = 0;
    }
}
