using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public Slider hpSlider;

    public GameObject clearButton;
    public GameObject failButton;

    void Start()
    {
        currentHP = maxHP; 
        UpdateHPBar();
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Max(currentHP, 0);

        UpdateHPBar();

        if (currentHP == 0)
        {
            GameOver();
        }
    }

    void UpdateHPBar()
    {
        hpSlider.value = currentHP;
    }

    void GameOver()
    {
        ShowFailButton();
        PlayerController.moveSpeed = 0f;
        PlayerController.isLeft = 2;
        PlayerController.isRight = 2;
        Debug.Log("Game Over!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageObject"))
        {
            TakeDamage(20);

            other.gameObject.SetActive(false);
        }

        else if (other.CompareTag("goal")){
            PlayerController.moveSpeed = 0f;
            PlayerController.isLeft = 2;
            PlayerController.isRight = 2;
            ShowClearButton();
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

    public void ClearStage()
    {
        DBManager.NextSceneName = "Title";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }

    public void FailStage()
    {
        DBManager.NextSceneName = "Title";
        LoadingScene.LoadScene(DBManager.NextSceneName);
    }


}
