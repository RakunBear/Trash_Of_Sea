using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform�� ����

    public float smoothSpeed = 0.125f; // ī�޶� �̵� �ӵ�
    public float zOffset = -10f; // �÷��̾�� ī�޶� ���� �Ÿ� (z ��)

    void LateUpdate()
    {
        float desiredZ = player.position.z + zOffset;
        float smoothedZ = Mathf.LerpUnclamped(transform.position.z, desiredZ, smoothSpeed);
        Vector3 smoothedPosition = new Vector3(transform.position.x, transform.position.y, smoothedZ);
        transform.position = smoothedPosition;

        // x �� �������� ������ ���� ī�޶� ������ �ʵ��� �մϴ�.
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + zOffset);

        /*transform.LookAt(player);*/
    }
}
