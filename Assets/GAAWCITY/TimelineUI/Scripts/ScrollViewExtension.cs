using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineViewer
{
    public class ScrollViewExtension : MonoBehaviour
    {
        [SerializeField] RectTransform contentContentsRect;
        [SerializeField] float contentContentsPaddingY;

        public void OnAdjacentScroll(RectTransform rect)
        {
            if (rect != null && contentContentsRect != null)
            {
                contentContentsRect.offsetMin = new Vector2(contentContentsRect.offsetMin.x, rect.offsetMin.y + contentContentsPaddingY);
                contentContentsRect.offsetMax = new Vector2(contentContentsRect.offsetMax.x, rect.offsetMax.y + contentContentsPaddingY);
            }
        }
    }
}