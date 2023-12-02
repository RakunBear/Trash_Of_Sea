using System.Collections;
using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField]
    TrackingDataReciver _trackingDataReciver;

    public float stickSpeed = 3f;
    private Vector3 initialPosition;
    private bool isSwingingLt = false;
    private bool isSwingingRt = false;

    private float targetAngle = -135f;
    private float swingTime = 0.3f;
    private float progress = 0f;

    Vector3 LTInitialPosition = new Vector3(-3f, 5f, -3f);
    Vector3 LTTargetPosition = new Vector3(-3f, 1f, -3f);
    Vector3 RTInitialPosition = new Vector3(1f, 5f, -3f);
    Vector3 RTTargetPosition = new Vector3(1f, 1f, -3f);

    public GameObject ltHook;
    public GameObject rtHook;

    TimingManager timingManager;

    private void OnDestroy() {
        _trackingDataReciver.LeftOnValueChage -= LSwing;
        _trackingDataReciver.RightOnValueChange -= RSwing;
    }

    void Start()
    {
        initialPosition = transform.position;
        timingManager = FindObjectOfType<TimingManager>();
        _trackingDataReciver.LeftOnValueChage += LSwing;
        _trackingDataReciver.RightOnValueChange += RSwing;
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

    public void LSwing(bool isSwing)
    {
        if (!(gameObject.tag == "Stick_Red")) return;
        if (!isSwing) return;

        targetAngle = -135f;
        StartCoroutine(SwingStickLt());
    }

    public void RSwing(bool isSwing)
    {
        if (!(!isSwingingRt && gameObject.tag == "Stick_Blue")) return;
        if (!isSwing) return;

        targetAngle = 135f;
        StartCoroutine(SwingStickRt());
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
                ltHook.transform.position = Vector3.Lerp(LTInitialPosition, LTTargetPosition, progress * 2);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, targetAngle), targetRotation, (progress - 0.5f) * 2);
                ltHook.transform.position = Vector3.Lerp(LTTargetPosition, LTInitialPosition, (progress - 0.5f) * 2);

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
                rtHook.transform.position = Vector3.Lerp(RTInitialPosition, RTTargetPosition, progress * 2);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, targetAngle), targetRotation, (progress - 0.5f) * 2);
                rtHook.transform.position = Vector3.Lerp(RTTargetPosition, RTInitialPosition, (progress - 0.5f) * 2);

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
