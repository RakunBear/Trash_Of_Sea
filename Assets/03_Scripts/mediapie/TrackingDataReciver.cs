using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDataReciver : MonoBehaviour
{
    private void Update() {
        GetLeftHandPos();
        GetRightHandPos();
    }

    public Vector3 GetLeftHandPos() {
        Vector3 pos = TrackingDataSender.leftHandPos;
        Debug.Log("Left: " +pos);

        return TrackingDataSender.leftHandPos;
    }

    public Vector3 GetRightHandPos() {
        Vector3 pos = TrackingDataSender.rightHandPos;
        Debug.Log("Right: " + pos);

        return pos;
    }
}
