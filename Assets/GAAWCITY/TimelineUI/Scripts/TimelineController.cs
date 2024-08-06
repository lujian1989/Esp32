using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace TimelineViewer
{

    public enum TimelineIncrements
    {
        Hours,
        HalfHour,
        QuaterHours,
        TenthHours,
        FifthHours,
        Minutes,
        Seconds,
    }

    public class TimelineController : MonoBehaviour
    {


        public enum ClockType
        {
            Twelve,
            TwentyFour
        }

        int[] incrementMultiplier = { 0, 30, 15, 10, 5, 1 ,60};

        [Header("Text & Styles")]
        [SerializeField] string timelineHeaderText;
        [SerializeField] Color timelineHeaderColor;

        [Header("Dates & Times")]
        [SerializeField] UnityDateTime startTime;
        [SerializeField] UnityDateTime endTime;

        [Header("Clock Settings")]
        [SerializeField] ClockType clockType;
        [SerializeField] TimelineIncrements increments;


        [HideInInspector]
        [SerializeField] GameObject timelineHeader;
        [HideInInspector]
        [SerializeField] TextMeshProUGUI timelineLabelHeader;
        [HideInInspector]
        [SerializeField] GameObject timelineSwimlanes;
        [HideInInspector]
        [SerializeField] GameObject timelineHeaderItemPrefab;
        [HideInInspector]
        [SerializeField] RectTransform swimlaneContent;
        [HideInInspector]
        [SerializeField] Transform swimLaneLabelContent;


        Transform timelineHeaderTransform;

        public TimelineIncrements DailyIncements { get; private set; }

        public int TimeCount { get; private set; }

        public UnityDateTime ScenarioStartTime { get; private set; }
        public UnityDateTime ScenarioEndTime { get; private set; }

        public TimelineSwimlaneController SwimLaneController { get { return timelineSwimlaneController; } }

        TimelineSwimlaneController timelineSwimlaneController;

        // Start is called before the first frame update
        void Start()
        {
            string endStr = "2024-08-06 00:01:06";
            DateTime time = DateTime.ParseExact(endStr, "yyyy-MM-dd HH:mm:ss", null);
            endTime = time;
            
            
            timelineHeaderTransform = timelineHeader.transform;

            ScenarioStartTime = startTime;
            ScenarioEndTime = endTime;

            DailyIncements = increments;

            timelineLabelHeader.text = timelineHeaderText;

            SetupTimelineHeader();

            timelineSwimlaneController = timelineSwimlanes.GetComponent<TimelineSwimlaneController>();
        }

        public void AddNewSwimlane(string swimlaneName)
        {
            timelineSwimlaneController.AddSwimlane(swimlaneName, swimLaneLabelContent, TimeCount, DailyIncements);
        }

        public void AddEventToTimeline(string title, UnityDateTime eventDate, double timeToComplete)
        {
            timelineSwimlaneController.AddTimelineItem(title, ScenarioStartTime, eventDate, timeToComplete);
        }

        public void AddEventToTimeline(int index, string title, UnityDateTime eventDate, double timeToComplete)
        {
            timelineSwimlaneController.AddTimelineItem(index, title, ScenarioStartTime, eventDate, timeToComplete);
        }

        public void ResetTimeline()
        {
            timelineSwimlaneController.DeleteAllSwimlanes();

            for (int i = 0; i < timelineHeaderTransform.childCount; i++)
            {
                Destroy(timelineHeaderTransform.GetChild(i));
            }

            SetupTimelineHeader();
        }

        private void SetupTimelineHeader()
        {

            TimeSpan  totalMin = endTime.m_DateTime.Subtract(startTime.m_DateTime);
            Debug.Log(totalMin.TotalSeconds);
            // for (int m = ScenarioStartTime.m_DateTime.Month; m <= ScenarioEndTime.m_DateTime.Month; m++)
            // {
            //     for (int d = ScenarioStartTime.m_DateTime.Day; d <= ScenarioEndTime.m_DateTime.Day; d++)
                {
                    for (int h = 0; h < totalMin.TotalSeconds; h++)
                    {
                        int cH = h;
                        // if (clockType == ClockType.Twelve)
                        // {
                        //     if (h > 12)
                        //     {
                        //         cH = h - 12;
                        //     }
                        //     else if (h == 0)
                        //     {
                        //         cH = 12;
                        //     }
                        // }

                        // for (int i = 0; i < getIncrements(); i++)
                        // {
                        //     var headerItem = Instantiate(timelineHeaderItemPrefab, timelineHeaderTransform, false);
                        //     var date = headerItem.GetComponent<TimelineHeaderItem>();
                        //
                        //     date.SetTextColor(timelineHeaderColor);
                        //     date.SetDateTime(0, 0, 0, i * incrementMultiplier[(int)increments]);
                        //
                        //     TimeCount++;
                        // }
                        var headerItem = Instantiate(timelineHeaderItemPrefab, timelineHeaderTransform, false);
                        var date = headerItem.GetComponent<TimelineHeaderItem>();
                        
                        date.SetTextColor(timelineHeaderColor);
                        int curSecond = h % 60;
                        
                        // 计算商（整数除法）
                        int quotient = h / 60;
                        // 计算余数
                        int remainder = h % 60;

                        date.SetDateTime(0, 0, curSecond,quotient ,remainder);
                        TimeCount++;
                    }
                }
          //  }
        }

        private int getIncrements()
        {
            switch (increments)
            {
                case TimelineIncrements.Hours:
                    return 1;
                case TimelineIncrements.HalfHour:
                    return 2;
                case TimelineIncrements.QuaterHours:
                    return 4;
                case TimelineIncrements.TenthHours:
                    return 6;
                case TimelineIncrements.FifthHours:
                    return 11;
                case TimelineIncrements.Minutes:
                    return 55;
                case TimelineIncrements.Seconds:
                    return 59;
                default:
                    return 1;
            }
        }
    }
}