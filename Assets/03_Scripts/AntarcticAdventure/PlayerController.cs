using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    TrackingDataReciver _trackingDataReciver;

    private bool isMoving = true;
    public static int isLeft = 1;
    public static int isRight = 1;
    public static float moveSpeed = 10f;

    private void OnDestroy()
    {
        _trackingDataReciver.LeftOnValueChage -= MoveLeft;
        _trackingDataReciver.RightOnValueChange -= MoveRight;
    }


    private void Awake()
    {
        _trackingDataReciver.LeftOnValueChage += MoveLeft;
        _trackingDataReciver.RightOnValueChange += MoveRight;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        if (isMoving)
        {
            if (Input.GetKey(KeyCode.A) && isLeft < 2)
            {
                MoveSideways(-3f);
                isLeft++;
                isRight--;
            }

            if (Input.GetKey(KeyCode.D) && isRight < 2)
            {
                MoveSideways(3f);
                isLeft--;
                isRight++;
            }
        }
    }

    public void MoveLeft(bool isSwing)
    {
        if (isMoving && isLeft < 2)
        {
            MoveSideways(-3f);
            isLeft++;
            isRight--;
        }
    }

    public void MoveRight(bool isSwing)
    {
        if (isMoving && isRight < 2)
        {
            MoveSideways(3f);
            isLeft--;
            isRight++;
        }
    }

    void MoveSideways(float amount)
    {
        isMoving = false;
        Vector3 targetPosition = new Vector3(transform.position.x + amount, transform.position.y, transform.position.z);
        StartCoroutine(MoveToPosition(targetPosition, 0.5f));
        Invoke("EnableInput", 0.6f);

    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    void EnableInput()
    {
        isMoving = true;
    }
}