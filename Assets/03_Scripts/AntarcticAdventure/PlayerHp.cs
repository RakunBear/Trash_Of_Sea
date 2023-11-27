using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int maxHP = 100; // �ִ� HP
    public int currentHP; // ���� HP

    public Slider hpSlider; // ������ HP �� Slider

    void Start()
    {
        currentHP = maxHP; // ������ �� �ִ� HP�� �ʱ�ȭ
        UpdateHPBar();
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP�� 0���� ������ 0���� ����
        currentHP = Mathf.Max(currentHP, 0);

        UpdateHPBar();

        // ����: HP�� 0�̸� ���� ���� ó��
        if (currentHP == 0)
        {
            GameOver();
        }
    }

    void UpdateHPBar()
    {
        // HP �� ������Ʈ
        hpSlider.value = currentHP;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // ���� ������ ���� �߰� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� "DamageObject" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("DamageObject"))
        {
            // �÷��̾��� HP ����
            TakeDamage(50);

            // ������Ʈ�� �浹�ϸ� �ش� ������Ʈ�� ��Ȱ��ȭ
            other.gameObject.SetActive(false);
        }
    }

}
