using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TimelineViewer
{
    public class SwimlaneItemLabel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector]
        [SerializeField] Image swimLaneBackground;
        [HideInInspector]
        [SerializeField] TextMeshProUGUI swimLaneText;
        [HideInInspector]
        [SerializeField] Button deleteButton;
        [SerializeField] Color defaultTextColor;
        [SerializeField] Color defaultBackgroundColor;
        [SerializeField] Color hoverTextColor;
        [SerializeField] Color hoverBackgroundColor;

        public string SwimlaneName { get { return swimLaneText.text; } }

        [HideInInspector]
        public int swimLaneIndex = 0;

        // Start is called before the first frame update
        public void Awake()
        {
            swimLaneBackground.color = new Color(defaultBackgroundColor.r, defaultBackgroundColor.g, defaultBackgroundColor.b, defaultBackgroundColor.a);
            swimLaneText.color = new Color(defaultTextColor.r, defaultTextColor.g, defaultTextColor.b, defaultTextColor.a);

            deleteButton.onClick.AddListener(delegate { DeleteSwimlane(); });

            swimLaneIndex = TimelineSwimlaneController.SwimLaneCount;
        }

        private void DeleteSwimlane()
        {
            TimelineSwimlaneController.DeleteSwimlane(swimLaneIndex);
        }

        private void OnDestroy()
        {
            deleteButton.onClick.RemoveAllListeners();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            swimLaneBackground.color = new Color(hoverBackgroundColor.r, hoverBackgroundColor.g, hoverBackgroundColor.b, hoverBackgroundColor.a);
            swimLaneText.color = new Color(hoverTextColor.r, hoverTextColor.g, hoverTextColor.b, hoverTextColor.a);
            deleteButton.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            swimLaneBackground.color = new Color(defaultBackgroundColor.r, defaultBackgroundColor.g, defaultBackgroundColor.b, defaultBackgroundColor.a);
            swimLaneText.color = new Color(defaultTextColor.r, defaultTextColor.g, defaultTextColor.b, defaultTextColor.a);
            deleteButton.gameObject.SetActive(false);
        }
    }
}