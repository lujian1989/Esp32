using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineViewer
{
    public class ScrollRectContentResizer : MonoBehaviour
    {
        [SerializeField] RectTransform rectTransform;

        private void OnEnable()
        {
            StartCoroutine(changeRectTransformWidth());
        }

        IEnumerator changeRectTransformWidth()
        {
            yield return new WaitWhile(() => rectTransform.rect.width == 0);

            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width);
        }
    }
}