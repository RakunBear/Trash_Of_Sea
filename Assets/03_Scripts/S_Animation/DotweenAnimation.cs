using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public static class DotweenAnimation
{
    #region  Sequence
    public static Sequence FadeInOut<T>(this T target, float duration, bool isFadeIn, float startDelay = 0.0f, Ease ease = Ease.Linear) where T : Graphic {
        Sequence sequence = DOTween.Sequence();

        if (isFadeIn) {
            sequence.OnStart(() => ZeroColor(target, true))
                .Append(target.DOFade(1, duration).SetEase(ease).SetDelay(startDelay));
        } else {
            sequence.OnStart(() => ZeroColor(target, false))
                .Append(target.DOFade(0, duration).SetEase(ease).SetDelay(startDelay));
        }

        return sequence;
    }

    public static Sequence Blink<T>(this T target, float duration, float blinkRate, Ease ease = Ease.Linear) where T : Graphic {
        Sequence sequence = DOTween.Sequence();
        
        WaitForSeconds waitTime = new WaitForSeconds(blinkRate);

        bool origin = target.enabled;

        IEnumerator Blinking() {
            float time = 0.0f;
            while (time > duration) {
                yield return waitTime;

                time += Time.deltaTime;
                if (time > duration) break;

                target.enabled = target.enabled ? false : true;
            }
            yield return null;
        }

        target.enabled = origin;

        return sequence;
    }
    #endregion

    #region Init
    public static void ZeroColor<T>(this T target, bool isZero) where T : Graphic {
        float alpha = isZero ? 0 : 1;
        target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
    }
    #endregion
}
