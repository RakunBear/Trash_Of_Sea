using System.Collections;
using System.Collections.Generic;
using Intel.RealSense;
using UnityEngine;

public class RsStreamTextureAdder : MonoBehaviour
{
    [SerializeField] RsStreamTextureRenderer _rsStreamTextureRenderer;

    private void Start() {
        _rsStreamTextureRenderer.Source.OnNewSample += MediapipeTrackingGetDistance;
    }

    public void MediapipeTrackingGetDistance(Frame frame)
    {

        if (TrackingDataSender.LeftHandStatus.IsActive)
        {
            // Debug.Log("Run Left Get Distance");
            GetLDistance(frame, (int)((1-TrackingDataSender.LeftHandStatus.NomerizedPos.x) * 640), (int)(TrackingDataSender.LeftHandStatus.NomerizedPos.y * 480));
        }

        if (TrackingDataSender.RightHandStatus.IsActive)
        {
            // Debug.Log("Run Right Get Distance");
            GetRDistance(frame, (int)((1-TrackingDataSender.RightHandStatus.NomerizedPos.x) * 640), (int)(TrackingDataSender.RightHandStatus.NomerizedPos.x * 480));
        }
    }

    public void GetLDistance(Frame frame, int x, int y)
    {
        x = Mathf.Max(0, x);
        y = Mathf.Max(0,y);
        x = Mathf.Min(640, x);
        y = Mathf.Min(480, y);

        using (var fs = FrameSet.FromFrame(frame))
        using (var depth = fs.DepthFrame)
        {
            float depthValue = depth.GetDistance(x, y);
            TrackingDataSender.LeftHandStatus.Position.z = depthValue;
            Debug.Log($"L : [{x}, {y}] : {depthValue}");
        }
    }

    public void GetRDistance(Frame frame, int x, int y)
    {
        x = Mathf.Max(0, x);
        y = Mathf.Max(0,y);
        x = Mathf.Min(640, x);
        y = Mathf.Min(480, y);

        using (var fs = FrameSet.FromFrame(frame))
        using (var depth = fs.DepthFrame)
        {
            float depthValue = depth.GetDistance(x, y);
            Debug.Log($"R: [{x}, {y}] : {depthValue}");
            TrackingDataSender.RightHandStatus.Position.z = depthValue;
        }
    }
}
