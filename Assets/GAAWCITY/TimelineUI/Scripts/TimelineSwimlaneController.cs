using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TimelineViewer
{
    struct SwimLaneBracket
    {
        public SwimlaneItemLabel swimlaneLabel;
        public TimelineSwimlane swimlane;

        public SwimLaneBracket(SwimlaneItemLabel label, TimelineSwimlane lane)
        {
            swimlaneLabel = label;
            swimlane = lane;
        }
    }

    public class TimelineSwimlaneController : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] GameObject swimLanePrefab;

        TimelineSwimlane swimLaneTS;

        TimelineIncrements dailyIncrements;

        public static int SwimLaneCount { get { return swimlanes.Count; } }
        public List<string> CurrentSwimlanes { get { return swimlanes.Values.Select(p => p.swimlaneLabel.SwimlaneName).ToList(); } }

        static Dictionary<int, SwimLaneBracket> swimlanes = new Dictionary<int, SwimLaneBracket>();

        private void Start()
        {

        }


        public void AddSwimlane(string laneName, Transform parent, int timeCount, TimelineIncrements increments)
        {
            dailyIncrements = increments;

            var swimLane = Instantiate(swimLanePrefab, transform, false);

            swimLaneTS = swimLane.GetComponent<TimelineSwimlane>();
            SwimlaneItemLabel label = swimLaneTS.SetupSwimlane(laneName, parent, timeCount);
            swimlanes.Add(swimlanes.Count, new SwimLaneBracket(label, swimLaneTS));
        }

        public void DeleteAllSwimlanes()
        {
            foreach (var lane in swimlanes)
            {
                DeleteSwimlane(lane.Value.swimlaneLabel.swimLaneIndex);
            }
        }

        public static void DeleteSwimlane(int swimlaneIndex)
        {
            var combo = swimlanes[swimlaneIndex];

            Destroy(combo.swimlaneLabel.gameObject);
            Destroy(combo.swimlane.gameObject);

            swimlanes.Remove(swimlaneIndex);

            ReCalibrateLaneIndexes();
        }

        private static void ReCalibrateLaneIndexes()
        {
            int i = 0;
            Dictionary<int, SwimLaneBracket> tmp = new Dictionary<int, SwimLaneBracket>(swimlanes);
            swimlanes.Clear();
            foreach (var lane in tmp)
            {
                lane.Value.swimlaneLabel.swimLaneIndex = i;
                swimlanes.Add(i, lane.Value);
                i++;
            }
        }

        public void AddTimelineItem(string title, UnityDateTime startTime, UnityDateTime eventDate, double hoursToComplete)
        {
            var pos = CalculatePositionFromDateTime(startTime, eventDate);
            swimLaneTS.AddTimelineItem(title, pos, hoursToComplete);
        }

        public void AddTimelineItem(int index, string title, UnityDateTime startTime, UnityDateTime eventDate, double hoursToComplete)
        {
            swimLaneTS = swimlanes[index].swimlane;
            var pos = CalculatePositionFromDateTime(startTime, eventDate);
            swimLaneTS.AddTimelineItem(title, pos, hoursToComplete);
        }

        private double CalculatePositionFromDateTime(UnityDateTime timelineStart, UnityDateTime mark)
        {
            double timeLocation = GetTimeDifference(timelineStart, mark);

            return timeLocation;
        }

        private double GetTimeDifference(UnityDateTime timelineStart, UnityDateTime mark)
        {
            DateTime date1 = new DateTime(timelineStart.m_DateTime.Year, timelineStart.m_DateTime.Month, timelineStart.m_DateTime.Day, timelineStart.m_DateTime.Hour, timelineStart.m_DateTime.Minute, 0);
            DateTime date2 = new DateTime(mark.m_DateTime.Year, mark.m_DateTime.Month, mark.m_DateTime.Day, mark.m_DateTime.Hour, mark.m_DateTime.Minute, 0);

            TimeSpan difference = date2 - date1;

            double adjustedHours = adjustHoursbyDailyIncrements(difference);

            return adjustedHours;
        }

        private double adjustHoursbyDailyIncrements(TimeSpan difference)
        {
            int posInt = (int)Math.Truncate(difference.TotalHours);
            float posfl = (float)difference.TotalHours - posInt;

            if (dailyIncrements == TimelineIncrements.HalfHour)
            {
                int newPosInt = posInt * 2;
                float newPosFl = posfl * 2;

                var newTotalHours = (double)newPosInt + newPosFl;

                return newTotalHours;
            }
            else if (dailyIncrements == TimelineIncrements.QuaterHours)
            {
                int newPosInt = posInt * 4;
                float newPosFl = posfl * 4;

                var newTotalHours = (double)newPosInt + newPosFl;

                return newTotalHours;
            }
            else if (dailyIncrements == TimelineIncrements.TenthHours)
            {
                int newPosInt = posInt * 6;
                float newPosFl = posfl * 6;

                var newTotalHours = (double)newPosInt + newPosFl;

                return newTotalHours;
            }
            else if (dailyIncrements == TimelineIncrements.FifthHours)
            {
                int newPosInt = posInt * 11;
                float newPosFl = posfl * 12;

                var newTotalHours = (double)newPosInt + newPosFl;

                return newTotalHours;
            }
            else
            {
                return difference.TotalHours;
            }
        }
    }
}