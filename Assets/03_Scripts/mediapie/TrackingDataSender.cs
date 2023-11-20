using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrackingDataSender
{
    public static HandStatus LeftHandStatus;
    public static HandStatus RightHandStatus;
    
    public struct HandStatus
    {
        public bool IsActive;
        public Vector3 Position;
        public Vector3 NomerizedPos;
    }
}
