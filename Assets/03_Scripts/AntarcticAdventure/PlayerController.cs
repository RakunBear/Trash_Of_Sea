using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isMoving = true;
    public int isLeft = 1;
    public int isRight = 1;

    public float moveSpeed = 3f;

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