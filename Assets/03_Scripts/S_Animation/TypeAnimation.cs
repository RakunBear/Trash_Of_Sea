using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Transition
{
    public class TypeAnimation : MonoBehaviour
    {
        [Header("Metadata")]
        [SerializeField]
        TextMeshProUGUI text;

        [Header("Setting")]
        public float TypeDelay = 0.02f;

        WaitForSeconds typeWait => new WaitForSeconds(TypeDelay);

        public void Type()
        {
            if (text == null) TryGetComponent(out text);
            StartCoroutine(Typing());
        }

        IEnumerator Typing()
        {
            if (text == null) { Debug.LogError($"Not Found TextMeshPro in {gameObject.name}"); }

            text.ForceMeshUpdate();
            int maxLeng = text.textInfo.characterCount;
            int counter = 0;
            string fullText = text.text;
            text.text = "";

            while (counter < maxLeng)
            {
                //text.maxVisibleCharacters = counter++;
                text.text += fullText[counter++];
                yield return typeWait;
            }
            text.text = fullText;
        }
    }
}