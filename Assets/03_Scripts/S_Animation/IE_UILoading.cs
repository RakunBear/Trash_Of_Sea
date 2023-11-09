using UnityEngine;
using System.Collections;

public class IE_UILoading : MonoBehaviour
{
    public bool PlayOnEnable = true;

    public enum AnimationMode
    {
        Wave = 0,
    }

    public RectTransform[] m_Rect;
    public AnimationMode MeshAnimationMode = AnimationMode.Wave;
    public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 2.0f), new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f), new Keyframe(1, 0f));
    public float AngleMultiplier = 1.0f;
    [Min(1)]
    public int SpeedMultiplier = 1;
    public float CurveScale = 1.0f;
    private Coroutine cancelCoroutine;

    private void Awake() {
        cancelCoroutine = null;
    }

    void OnEnable()
    {
        if (PlayOnEnable)
            PlayAnimation(MeshAnimationMode);
    }

    private void OnDisable() {
        Stop();
    }
    private void OnDestroy() {
        Stop();
    }

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    private void Stop()
    {
        if (cancelCoroutine != null) {
            StopCoroutine(cancelCoroutine);
            cancelCoroutine = null;
        }
        ResetGeometry();
    }

    public void PlayAnimation(AnimationMode _mode)
    {
        Stop();

        switch (_mode)
        {
            case AnimationMode.Wave:
                cancelCoroutine = StartCoroutine(AnimateWave());
                break;
        }
    }

    void ResetGeometry()
    {
        for (int i = 0; i < m_Rect.Length; i++)
        {
            Vector3 pos = m_Rect[i].anchoredPosition;
            pos.y = 0;
            m_Rect[i].anchoredPosition = pos;
        }
    }

    private IEnumerator AnimateWave()
    {
        VertexCurve.preWrapMode = WrapMode.Loop;
        VertexCurve.postWrapMode = WrapMode.Loop;

        int loopCount = 0;

        while (true)
        {

            for (int i = 0; i < m_Rect.Length; i++)
            {
                float offsetY = VertexCurve.Evaluate((float)i / m_Rect.Length + loopCount / 50f) * CurveScale; // Random.Range(-0.25f, 0.25f);                    

                Vector3 pos = m_Rect[i].anchoredPosition;
                pos.y = offsetY;
                m_Rect[i].anchoredPosition = pos;
            }

            loopCount += 1;
            yield return new WaitForSeconds( SpeedMultiplier );
        }
    }
}