using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingDataReciver : MonoBehaviour
{
    [SerializeField] Image _lImg;
    [SerializeField] Image _rImg;
    [Header("Sensertive"), Range(0,1)]
    public float Sensertive = 0.4f;

    public Action<bool> LeftOnValueChage;
    public Action<bool> RightOnValueChange;

    private bool _isL_Click = false;
    public bool IsL_Click
    {
        get
        {
            return _isL_Click;
        }
    }
    private bool _isR_Click = false;
    public bool IsR_Click
    {
        get
        {
            return _isR_Click;
        }
    }


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
            // Debug.Log($"L : {TrackingDataSender.LeftHandStatus.IsActive}+{TrackingDataSender.LeftHandStatus.Position.z} - R : {TrackingDataSender.RightHandStatus.IsActive}");
            // 왼쪽손
            if (TrackingDataSender.LeftHandStatus.IsActive)
            {
                int code = CheckPass(TrackingDataSender.LeftHandStatus.Position.z);
                //인식 (가까이)
                if (code == 1)
                {
                    LeftHandMethod(true);
                }
                else if (code == 2) // depth 멀리
                {
                    LeftHandMethod(false);
                }

                
            }
            else
            {
                LeftHandMethod(false);
            }

            // 오른쪽 손
            if (TrackingDataSender.RightHandStatus.IsActive)
            {
                int code = CheckPass(TrackingDataSender.RightHandStatus.Position.z);

                if (code == 1)
                {
                    RightHandMethod(true);
                    
                }
                else if (code == 2)
                {
                    RightHandMethod(false);
                }
            }
            else
            {
                RightHandMethod(false);
            }

            yield return null;
        }
    }

    protected void LeftHandMethod(bool click)
    {
        if(_isL_Click == click) return;

        _lImg.enabled = click;
        _isL_Click = click;

        LeftOnValueChage?.Invoke(click);
    }
    protected void RightHandMethod(bool click)
    {
        if(_isR_Click == click) return;

        _rImg.enabled = click;
        _isR_Click = click;

        RightOnValueChange?.Invoke(click);
    }

    protected

    private int CheckPass(float depth)
    {
        if (0.1f < depth && depth < Sensertive)
        {
            return 1;
        }
        else if (depth > Sensertive)
        {
            return 2;
        }
        

        return -1;
    }
}
