using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingDataReciver : MonoBehaviour
{
    [SerializeField] Image _lImg;
    [SerializeField] Image _rImg;

    public Coroutine coroutine;

    private void OnDestroy() {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private void Start() {
        _lImg.enabled = false;
        _rImg.enabled = false;
        coroutine = StartCoroutine(CheckHand());
    }

    public IEnumerator CheckHand()
    {
        while (true)
        {
            if (TrackingDataSender.LeftHandStatus.IsActive)
            {
                int code = CheckPass(TrackingDataSender.LeftHandStatus.Position.z);
                if (code == 1)
                {
                    if (!_lImg.enabled)
                        _lImg.enabled = true;
                }
                else if (code == 2)
                {
                    if (_lImg.enabled)
                        _lImg.enabled = false;
                }

                
            }
            else
            {
                if (_lImg.enabled)
                        _lImg.enabled = false;
            }

            if (TrackingDataSender.RightHandStatus.IsActive)
            {
                int code = CheckPass(TrackingDataSender.RightHandStatus.Position.z);
                if (code == 1)
                {
                    if (!_rImg.enabled)
                        _rImg.enabled = true;
                }
                else if (code == 2)
                {
                    if (_rImg.enabled)
                        _rImg.enabled = false;
                }
            }
            else
            {
                if (_rImg.enabled)
                    _rImg.enabled = false;
            }

            yield return null;
        }
    }

    private int CheckPass(float depth)
    {
        Debug.Log("Depth: " + depth);
        if (0.1f < depth && depth < 0.5f)
        {
            return 1;
        }
        else if (depth > 0.5f)
        {
            return 2;
        }
        

        return -1;
    }
}
