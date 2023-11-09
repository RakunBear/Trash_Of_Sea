using UnityEngine;
using System.Collections;
using TMPro;
using System.Threading;
using System.Threading.Tasks;

public class IE_TMProTextAnimation : MonoBehaviour
{
    public bool PlayOnEnable = true;

    public enum AnimationMode
    {
        Wave = 0,
    }

    public AnimationMode MeshAnimationMode = AnimationMode.Wave;
    public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 2.0f), new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f), new Keyframe(1, 0f));
    public float AngleMultiplier = 1.0f;
    public int SpeedMultiplier = 1;
    public float CurveScale = 1.0f;

    private bool hasTextChanged;
    private TMP_Text m_TextComponent;
    private Coroutine cancleCorountine;

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

    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        cancleCorountine = null;
    }

    private void Play()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        PlayAnimation(MeshAnimationMode);
    }

    private void Stop()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        if (cancleCorountine != null)
            StopCoroutine(cancleCorountine);
        ResetText();
    }

    public void PlayAnimation(AnimationMode _mode)
    {
        Stop();

        switch (_mode)
        {
            case AnimationMode.Wave:
                cancleCorountine = StartCoroutine(AnimateWave());
                break;
        }
    }

    void ResetText()
    {
        if (m_TextComponent.mesh == null) 
            return;
        ResetGeometry();
    }

    void ResetGeometry()
    {
        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var newVertexPositions = textInfo.meshInfo[i].vertices;
            
            // Upload the mesh with the revised information
            UpdateMesh(newVertexPositions, 0);
        }

        m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
    }

    private void UpdateMesh(Vector3[] _vertex, int index)
    {
        m_TextComponent.mesh.vertices = _vertex;
        m_TextComponent.mesh.uv = m_TextComponent.textInfo.meshInfo[index].uvs0;
        m_TextComponent.mesh.uv2 = m_TextComponent.textInfo.meshInfo[index].uvs2;
        m_TextComponent.mesh.colors32 = m_TextComponent.textInfo.meshInfo[index].colors32;
    }

    private IEnumerator AnimateWave()
    {
        VertexCurve.preWrapMode = WrapMode.Loop;
        VertexCurve.postWrapMode = WrapMode.Loop;

        Vector3[] newVertexPositions;
        //Matrix4x4 matrix;

        int loopCount = 0;

        while (true)
        {

            m_TextComponent.renderMode = TextRenderFlags.DontRender; // Instructing TextMesh Pro not to upload the mesh as we will be modifying it.
            //m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

            ResetGeometry();

            TMP_TextInfo textInfo = m_TextComponent.textInfo;
            int characterCount = textInfo.characterCount;

            newVertexPositions = textInfo.meshInfo[0].vertices;

            for (int i = 0; i < characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f) * CurveScale; // Random.Range(-0.25f, 0.25f);                    

                newVertexPositions[vertexIndex + 0].y += offsetY;
                newVertexPositions[vertexIndex + 1].y += offsetY;
                newVertexPositions[vertexIndex + 2].y += offsetY;
                newVertexPositions[vertexIndex + 3].y += offsetY;
            }

            loopCount += 1;

            // Upload the mesh with the revised information
            //m_TextComponent.mesh.vertices = newVertexPositions;
            //m_TextComponent.mesh.uv = m_TextComponent.textInfo.meshInfo[0].uvs0;
            //m_TextComponent.mesh.uv2 = m_TextComponent.textInfo.meshInfo[0].uvs2;
            //m_TextComponent.mesh.colors32 = m_TextComponent.textInfo.meshInfo[0].colors32;

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return new WaitForSeconds( SpeedMultiplier );
        }
    }
}