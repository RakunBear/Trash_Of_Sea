using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private float distance; // 시작 위치와 목표 위치 사이의 거리
    private float duration = 2.6f; // 이동에 걸리는 시간
    private float speed; // 적절한 속도

    private Vector3 startPosition; // 초기 위치
    private Vector3 targetPosition; // 목표 위치

    private bool isMoving = true; // 이동 여부를 나타내는 변수


    void Start()
    {
        float yPos = 0f;

        if (gameObject.tag == "Cube_lt")
        {
            // 큐브의 초기 위치를 설정
            startPosition = new Vector3(-1.5f, yPos, 30);
            // 목표 위치를 설정
            targetPosition = new Vector3(-1.5f, yPos, -7);
        }

        if (gameObject.tag == "Cube_rt")
        {
            // 큐브의 초기 위치를 설정
            startPosition = new Vector3(1.5f, yPos, 30);
            // 목표 위치를 설정
            targetPosition = new Vector3(1.5f, yPos, -7);
        }

        transform.position = startPosition;

        // 시작 위치와 목표 위치 사이의 거리 계산
        distance = Vector3.Distance(startPosition, targetPosition);

        // 적절한 속도 계산
        speed = distance / duration;
    }

    void Update()
    {
        if (isMoving)
        {
            // 시작 위치에서 목표 위치까지 일정한 속도로 이동
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
