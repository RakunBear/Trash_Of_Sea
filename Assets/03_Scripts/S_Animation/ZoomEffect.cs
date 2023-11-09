using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Transition
{
    public class ZoomEffect : MonoBehaviour
    {
        [SerializeField]
        GameObject targetObject;
        Image targetImage;
        [SerializeField]
        Transform movePoint_T;

        [Header("Setting Value")]
        public float zoomTime;
        public float zoomSize;
        public float fadeAccel;

        /* Origin Value */
        Vector3 _originSclae;
        Vector3 _originPosition;
        Color _originColor;

        #region ZoomIn
        public IEnumerator RunZoomIn()
        {
            SaveOrigin();

            yield return ZoomIn();
        }

        IEnumerator ZoomIn()
        {
            Debug.Log($"Run; {name}");
            // targetImage.color = new Color(
            //     targetImage.color.r, targetImage.color.g, targetImage.color.b, 0);

            float time_count = 0;
            Vector3 startScale = targetObject.transform.localScale;
            Vector3 endScale = targetObject.transform.localScale * zoomSize;
            Vector3 startPos = targetObject.transform.localPosition;
            Vector3 endPos = movePoint_T.localPosition * -1 * zoomSize;
            Color startColor = targetImage.color;
            Color endColor = new Color(
                targetImage.color.r, targetImage.color.g, targetImage.color.b, 1);

            while (time_count < 1)
            {
                time_count += Time.deltaTime / zoomTime;
                DoScaleUp(startScale, endScale, time_count);
                // DoFadeIn(endColor, startColor, time_count);
                DoMove(startPos, endPos, time_count);
                yield return null;
            }

            // End Set
            targetObject.transform.localPosition = endPos;
            targetImage.color = endColor;

        }

        private void DoScaleUp(Vector3 startScale, Vector3 endScale, float time)
        {
            targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, time);
        }
        // private void DoFadeIn(Color startColor, Color endColor, float time)
        // {
        //     targetImage.color = Color.Lerp(startColor, endColor, time * fadeAccel);
        // }
        private void DoMove(Vector3 startPos, Vector3 endPos, float time)
        {
            targetObject.transform.localPosition =
                    Vector2.Lerp(startPos, endPos, time);
        }
        #endregion

        #region Start&End
        public void SetEffect()
        {
            if (targetObject == null) targetObject = gameObject;

            _ = targetObject.TryGetComponent(out targetImage);
            if (targetImage == null) return;

            targetObject.SetActive(true);
        }
        private void SaveOrigin()
        {
            _originSclae = targetObject.transform.localScale;
            _originPosition = targetObject.transform.localPosition;
            _originColor = targetImage.color;
        }
        private void EndEffect()
        {
            if (targetObject == null) return;

            LoadOrigin();

            targetImage.raycastTarget = true;
            targetObject.SetActive(false);
        }
        private void LoadOrigin()
        {
            if (targetObject == null) return;

            targetObject.transform.localScale = _originSclae;
            targetObject.transform.localPosition = _originPosition;
            targetImage.color = _originColor;
        }
        #endregion

        private void OnDestroy()
        {
            EndEffect();
        }
    }
}