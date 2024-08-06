using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineViewer
{
    public class TimelineSwimlane : MonoBehaviour
    {
        [HideInInspector][SerializeField] GameObject swimLaneContent;
        [HideInInspector][SerializeField] GameObject swimLaneContentItemPrefab;
        [HideInInspector][SerializeField] GameObject swimLaneContentItemLabelPrefab;
        [HideInInspector][SerializeField] GameObject swimLaneRulePrefab;

        [HideInInspector][SerializeField] List<GameObject> swimlaneItems;

        public List<GameObject> rulerSections = new List<GameObject>();

        public SwimlaneItemLabel SetupSwimlane(string laneName, Transform parent, int timeCount)
        {
            var go = Instantiate(swimLaneContentItemLabelPrefab, parent, false);
            var swimlaneLabel = go.GetComponentInChildren<TextMeshProUGUI>();
            swimlaneLabel.text = laneName;

            for (int i = 0; i < timeCount; i++)
            {
                Instantiate(swimLaneRulePrefab, swimLaneContent.transform, false);
            }

            return go.GetComponent<SwimlaneItemLabel>();
        }

        public void AddTimelineItem(string title, double pos, double length)
        {
            int posInt = (int)Math.Truncate(pos);
            float posfl = (float)pos - posInt;

            var rulerItem = swimLaneContent.transform.GetChild(posInt);

            var item = Instantiate(swimLaneContentItemPrefab, rulerItem, false);

            item.transform.localPosition = item.transform.localPosition + new Vector3(posfl * 100f, 0, 0);
            var timelineItem = item.GetComponent<TimelineContentItem>();

            timelineItem.SetupTitle(title);
            timelineItem.AdjustLength(length);
        }

    }
}