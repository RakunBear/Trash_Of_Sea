using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int maxHP = 100; // 최대 HP
    public int currentHP; // 현재 HP

    public Slider hpSlider; // 연결할 HP 바 Slider

    void Start()
    {
        currentHP = maxHP; // 시작할 때 최대 HP로 초기화
        UpdateHPBar();
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP가 0보다 작으면 0으로 설정
        currentHP = Mathf.Max(currentHP, 0);

        UpdateHPBar();

        // 예시: HP가 0이면 게임 오버 처리
        if (currentHP == 0)
        {
            GameOver();
        }
    }

    void UpdateHPBar()
    {
        // HP 바 업데이트
        hpSlider.value = currentHP;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // 게임 오버에 대한 추가 로직을 여기에 추가할 수 있습니다.
    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "DamageObject" 태그를 가지고 있는지 확인
        if (other.CompareTag("DamageObject"))
        {
            // 플레이어의 HP 감소
            TakeDamage(50);

            // 오브젝트와 충돌하면 해당 오브젝트를 비활성화
            other.gameObject.SetActive(false);
        }
    }

}
