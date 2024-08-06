using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TimelineViewer;
using TMPro;
using UnityEngine;

namespace TimelineViewer.Sample
{
    public class ButtonTest : MonoBehaviour
    {
        [SerializeField] TimelineController timelineController;

        [Header("Swimlane Controls")]
        [SerializeField] TMP_InputField swimLaneName;

        [Header("Events Controls")]
        [SerializeField] TMP_Dropdown swimlanes;
        [SerializeField] TMP_InputField title;
        [SerializeField] TMP_InputField month;
        [SerializeField] TMP_InputField day;
        [SerializeField] TMP_InputField year;
        [SerializeField] TMP_InputField hour;
        [SerializeField] TMP_InputField min;
        [SerializeField] TMP_InputField seconds;
        [SerializeField] TMP_InputField eventLength;

        [Header("Form Elements")]
        [SerializeField] Transform swimLaneContainer;


        public void AddSwimLane()
        {
            timelineController.AddNewSwimlane(swimLaneName.text);

            swimlanes.ClearOptions();
            swimlanes.AddOptions(timelineController.SwimLaneController.CurrentSwimlanes
                     .Select(p => new TMP_Dropdown.OptionData { text = p }).ToList());
        }

        public void AddEventToTimeline()
        {
            year.text = "2024";
            month.text = "08";
            day.text = "06";
            hour.text = "00";
            min.text = "00";
            seconds.text = "06";
            
            //判断时间在范围内 
            
            
            var eventDate = new UnityDateTime();
            //string endStr = "2024-08-06 00:01:06";
            eventDate.m_DateTime =
                new System.DateTime(int.Parse(year.text), int.Parse(month.text), int.Parse(day.text),
                                    int.Parse(hour.text), int.Parse(min.text), 0, 0);


            double length = double.Parse(eventLength.text);

            int swimlaneIndex = swimlanes.value;

            timelineController.AddEventToTimeline(swimlaneIndex, title.text, eventDate, length);
        }




    }
}