using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private float distance; // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ�
    private float duration = 2.6f; // �̵��� �ɸ��� �ð�
    private float speed; // ������ �ӵ�

    private Vector3 startPosition; // �ʱ� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ

    private bool isMoving = true; // �̵� ���θ� ��Ÿ���� ����


    void Start()
    {


        if (gameObject.tag == "Cube_lt")
        {
            // ť���� �ʱ� ��ġ�� ����
            startPosition = new Vector3(-1.5f, 0, 30);
            // ��ǥ ��ġ�� ����
            targetPosition = new Vector3(-1.5f, 0, -7);
        }

        if (gameObject.tag == "Cube_rt")
        {
            // ť���� �ʱ� ��ġ�� ����
            startPosition = new Vector3(1.5f, 0, 30);
            // ��ǥ ��ġ�� ����
            targetPosition = new Vector3(1.5f, 0, -7);
        }

        transform.position = startPosition;

        // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ� ���
        distance = Vector3.Distance(startPosition, targetPosition);

        // ������ �ӵ� ���
        speed = distance / duration;
    }

    void Update()
    {
        if (isMoving)
        {
            // ���� ��ġ���� ��ǥ ��ġ���� ������ �ӵ��� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick_Red") && gameObject.tag == "Cube_lt")
        {
            isMoving = false;
        }

        if (other.gameObject.CompareTag("Stick_Blue") && gameObject.tag == "Cube_rt")
        {
            isMoving = false;
        }
    }
}
