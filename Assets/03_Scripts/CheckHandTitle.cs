using System.Runtime.ConstrainedExecution;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckHandTitle : MonoBehaviour
{
    [SerializeField]
    TitleGameMode titleGameMode;
    [SerializeField]
    TrackingDataReciver trackingDataReciver;

    [SerializeField]
    TextMeshProUGUI alterText;
    [SerializeField]
    GameObject rightCheck;
    [SerializeField]
    GameObject leftCheck;

    bool lCheck, rCheck;

    public void Start()
    {
        lCheck =false;
        rCheck = false;
        NearCheck();
    }

    IEnumerator WaitCheck(Action callback)
    {
        while (!lCheck || !rCheck)
        {
            yield return null;
        }

        callback.Invoke();
    }

    public void NearCheck()
    {
        trackingDataReciver.LeftOnValueChage += CheckNearHandL;
        trackingDataReciver.RightOnValueChange += CheckNearHandR;
        StartCoroutine(WaitCheck(FarCheck));
    }

    public void CheckNearHandL(bool isNear)
    {
        if (isNear)
        {
            trackingDataReciver.LeftOnValueChage -= CheckNearHandL;
            leftCheck.SetActive(true);
            lCheck = true;
        }
    }

    public void CheckNearHandR(bool isNear)
    {
        if(isNear)
        {
            trackingDataReciver.RightOnValueChange -= CheckNearHandR;
            rightCheck.SetActive(true);
            rCheck = true;
        }
    }

    public void FarCheck()
    {
        leftCheck.SetActive(false);
        lCheck = false;
        rightCheck.SetActive(false);
        rCheck = false;

        alterText.text = "Check Far";
        
        trackingDataReciver.LeftOnValueChage += CheckFarHandL;
        trackingDataReciver.RightOnValueChange += CheckFarHandR;
        StartCoroutine(WaitCheck(()=>
        {
            titleGameMode.Init();
            Destroy(gameObject);
        }));
    }

    public void CheckFarHandL(bool isNear)
    {
        if (!isNear)
        {
            trackingDataReciver.LeftOnValueChage -= CheckFarHandL;
            leftCheck.SetActive(true);
            lCheck = true;
        }
    }

    public void CheckFarHandR(bool isNear)
    {
        if(!isNear)
        {
            trackingDataReciver.RightOnValueChange -= CheckFarHandR;
            rightCheck.SetActive(true);
            rCheck = true;
        }
    }
}
