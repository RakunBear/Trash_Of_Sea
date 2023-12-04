using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutBook : MonoBehaviour
{
    public Image[] CutImgs;
    public float FadeTime = 2.0f;
    private int cutIndex;

    public void Init() {
        cutIndex = 0;
        foreach(Image cut in CutImgs) {
            cut.ZeroColor(true);
        }
    }

    public IEnumerator Playing() {
        Init();

        while(cutIndex < CutImgs.Length) {
            CutImgs[cutIndex++].FadeInOut(FadeTime, true, 0);
            yield return new WaitForSeconds(FadeTime);
        }

    }
}
