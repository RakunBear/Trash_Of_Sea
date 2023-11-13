using System.Collections;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public float stickSpeed = 3f;
    private Vector3 initialPosition;
    private bool isSwingingLt = false;
    private bool isSwingingRt = false;


    private float targetAngle = -135f;
    private float swingTime = 0.3f;
    private float progress = 0f;

    TimingManager timingManager;

    void Start()
    {
        initialPosition = transform.position;
        timingManager = FindObjectOfType<TimingManager>();
        Debug.Log(this.gameObject.tag);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isSwingingLt && gameObject.tag == "Stick_Red")
        {
            targetAngle = -135f;
            StartCoroutine(SwingStickLt());
        }

        else if (Input.GetKeyDown(KeyCode.D) && !isSwingingRt && gameObject.tag == "Stick_Blue")
        {
            targetAngle = 135f;
            StartCoroutine(SwingStickRt());
        }
    }

    IEnumerator SwingStickLt()
    {
        isSwingingLt = true;
        progress = 0f;

        while (progress <= 1f)
        {
            progress += Time.deltaTime / swingTime * stickSpeed;

            if (progress <= 0.5f)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRotation, progress * 2);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, targetAngle), targetRotation, (progress - 0.5f) * 2);
            }

            yield return null;
        }

        isSwingingLt = false;
    }

    IEnumerator SwingStickRt()
    {
        isSwingingRt = true;
        progress = 0f;

        while (progress <= 1f)
        {
            progress += Time.deltaTime / swingTime * stickSpeed;

            if (progress <= 0.5f)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRotation, progress * 2);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, targetAngle), targetRotation, (progress - 0.5f) * 2);
            }

            yield return null;
        }

        isSwingingRt = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube_lt") && gameObject.tag == "Stick_Red")
        {
            Rigidbody cubeRb = other.gameObject.GetComponent<Rigidbody>();
            cubeRb.AddForce(Vector3.down * 10f, ForceMode.VelocityChange);
            timingManager.CheckTiming();
        }
        else if (other.gameObject.CompareTag("Cube_rt") && gameObject.tag == "Stick_Blue")
        {
            Rigidbody cubeRb = other.gameObject.GetComponent<Rigidbody>();
            cubeRb.AddForce(Vector3.down * 10f, ForceMode.VelocityChange);
            timingManager.CheckTiming();
        }

    }
}
