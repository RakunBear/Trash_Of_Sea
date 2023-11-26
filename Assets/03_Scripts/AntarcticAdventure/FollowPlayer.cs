using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 연결

    public float smoothSpeed = 0.125f; // 카메라 이동 속도
    public float zOffset = -10f; // 플레이어와 카메라 간의 거리 (z 축)

    void LateUpdate()
    {
        float desiredZ = player.position.z + zOffset;
        float smoothedZ = Mathf.LerpUnclamped(transform.position.z, desiredZ, smoothSpeed);
        Vector3 smoothedPosition = new Vector3(transform.position.x, transform.position.y, smoothedZ);
        transform.position = smoothedPosition;

        // x 축 방향으로 움직일 때는 카메라가 따라가지 않도록 합니다.
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + zOffset);

        /*transform.LookAt(player);*/
    }
}
