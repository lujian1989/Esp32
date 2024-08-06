using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TimelineViewer
{
    public class TimelineContentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector]
        [SerializeField] Image swimLaneBackground; 
        [HideInInspector]
        [SerializeField] Image swimLaneForeground;
        [HideInInspector]
        [SerializeField] Button deleteButton;
        [SerializeField] Color defaultColor;
        [SerializeField] Color hoverColor;

        TextMeshProUGUI textControl;

        Vector2 origTextSizeDelta;
        public void SetupTitle(string title)
        {
            swimLaneBackground.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
            textControl = GetComponentInChildren<TextMeshProUGUI>();
            textControl.text = title;

            deleteButton.onClick.AddListener(delegate { Destroy(gameObject); });
        }

        private void OnDestroy()
        {
            deleteButton.onClick.RemoveAllListeners();
        }

        public void AdjustLength(double multiplier)
        {
            float flMulti = (float)multiplier;
            float offsetMin = ((flMulti * 50f) - 50f);
            float width = 100f * flMulti;

            var bgRect = swimLaneBackground.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(width, bgRect.sizeDelta.y);
            bgRect.anchoredPosition = new Vector2(offsetMin, bgRect.anchoredPosition.y);

            var txtRect = textControl.GetComponent<RectTransform>();
            txtRect.sizeDelta = new Vector2(width, bgRect.sizeDelta.y);
            txtRect.anchoredPosition = new Vector2(offsetMin, bgRect.anchoredPosition.y);


            var btnRect = deleteButton.GetComponent<RectTransform>();
            btnRect.anchoredPosition = new Vector2(btnRect.anchoredPosition.x + (offsetMin * 2) + 80f, btnRect.anchoredPosition.y);

            //rect.localPosition = new Vector3(offsetMin, rect.localPosition.y, rect.localPosition.z);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {


            var txtRect = textControl.GetComponent<RectTransform>();
            origTextSizeDelta = txtRect.sizeDelta;

            if (txtRect.sizeDelta.x < 50f)
            {
                textControl.enableWordWrapping = false;
                textControl.overflowMode = TextOverflowModes.Overflow;
                txtRect.sizeDelta = new Vector2(100f, 65);
            }

            swimLaneBackground.color = new Color(hoverColor.r, hoverColor.g, hoverColor.b, hoverColor.a);
            deleteButton.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            textControl.enableWordWrapping = true;
            textControl.overflowMode = TextOverflowModes.Ellipsis;

            var txtRect = textControl.GetComponent<RectTransform>();
            txtRect.sizeDelta = origTextSizeDelta;

            swimLaneBackground.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
            deleteButton.gameObject.SetActive(false);
        }
    }
}