using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TimelineViewer
{
    public class TimelineHeaderItem : MonoBehaviour
    {
        [HideInInspector][SerializeField] TextMeshProUGUI date;
        [HideInInspector][SerializeField] TextMeshProUGUI time;
        [HideInInspector][SerializeField] GameObject plumbline;

        public RectTransform PlumblineVector { get { return plumbline.GetComponent<RectTransform>(); } }

        public void SetTextColor(Color color)
        {
            date.color = new Color(color.r, color.g, color.b, color.a);
            time.color = new Color(color.r, color.g, color.b, color.a);
        }

        public void SetDateTime(int month, int day, int hour, int min = 0,int second = 0)
        {
            if (min == 0)
            {
                date.text = $"{month.ToString("00")}/{day.ToString("00")}";
            }
            else
            {
                date.text = "";
            }
            time.text = $"{min.ToString("00")}:{second.ToString("00")}";
        }
    }
}