using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CamTest : MonoBehaviour
{
    public static CamTest Instance;

    [SerializeField]
    public static Canvas _canvas;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void Start()
    {
        CamInit();
    }
    public void CamInit()
    {
        StartCoroutine(GettingCamera());
        
    }

    IEnumerator GettingCamera()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            if (t >= 2.0f)
                break;
            _canvas.worldCamera = Camera.main;
            if (_canvas.worldCamera != null)
            {
                var stackCameraDatas =  _canvas.worldCamera.GetUniversalAdditionalCameraData().cameraStack;
                if (stackCameraDatas.Count > 0)
                {
                    _canvas.worldCamera = stackCameraDatas[0];
                }
            }
            
            yield return null;
        }
    }
}
