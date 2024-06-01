using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary.GoogleExtensions
{
    public static class GoogleEventExtensions
    {
        private const string GoogleDateFormat = "yyyy-MM-dd";

        public static void CreateSummary(this Event myEvent, string summary, EventCategory type, List<Location> locations, EventStatus status)
        {
            myEvent.Summary = summary;
            var t = "";
            if (new List<EventCategory>() { EventCategory.Flight, EventCategory.Train, EventCategory.Taxi, EventCategory.Drive }.Contains(type))
            {
                var actualLocations = locations.Where(x => x != null && x.Address != "");
                var shortRoute = "";
                if (actualLocations.Count() > 1)
                    shortRoute = $"{actualLocations.First().ShortName} - {actualLocations.Last().ShortName}";

                var list = new List<string>()
                {
                    t,
                    shortRoute
                };
                var s = string.Join(": ", list.Where(x => !string.IsNullOrWhiteSpace(x)));

                if (!string.IsNullOrWhiteSpace(s))
                    myEvent.Summary = s;
                else
                    myEvent.Summary = summary;
            }
            else if (new List<EventCategory>() { EventCategory.Accommodation }.Contains(type))
            {
                var location = locations.FirstOrDefault();
                if (location == null || location.ShortName == "")
                    myEvent.Summary = $"{type}: TBC";
                else
                    myEvent.Summary = $"{type}: {location.ShortName}";
            }

            var dictionary = new Dictionary<string, string>()
            {
                { "idea", "i" },
                { "planned", "p" },
                { "confirmed", "c" },
                { "paid", "£" }
            };
            if (status != EventStatus.None)
            {
                myEvent.Summary = $"({dictionary[status.ToString().ToLowerInvariant()]}) {myEvent.Summary}";
            }
        }

        /// <summary>
        /// Turns the event into a single all day event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static void SetAllDay(this Event myEvent, DateTime dateTime)
        {
            myEvent.Start = new EventDateTime();
            myEvent.End = new EventDateTime();

            myEvent.Start.Date = dateTime.ToString(GoogleDateFormat);
            myEvent.End.Date = dateTime.ToString(GoogleDateFormat);
        }

        public static void SetDates(this Event myEvent, DateTime startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime)
        {
            if (endDate == null)
                endDate = startDate;

            if (startTime == null)
            {
                myEvent.SetMultiDay(startDate, endDate.Value);
            }
            else
            {
                if (endTime == null)
                {
                    endTime = startTime.Value.AddHours(1);
                }
                if (endTime.Value.TimeOfDay < startTime.Value.TimeOfDay && endDate.Value.Date == startDate)
                {
                    endDate = endDate.Value.AddDays(1);
                }
                myEvent.SetTimed(startDate, startTime.Value, endDate.Value, endTime.Value);
            }
        }

        /// <summary>
        /// Turns the event into a multi all day event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static void SetMultiDay(this Event myEvent, DateTime startDate, DateTime endDate)
        {
            myEvent.Start = new EventDateTime();
            myEvent.End = new EventDateTime();

            myEvent.Start.Date = startDate.ToString("yyyy-MM-dd");
            myEvent.End.Date = endDate.AddDays(1).ToString("yyyy-MM-dd");

            //return myEvent;
        }

        /// <summary>
        /// Turns the event into a timed event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static void SetTimed(this Event myEvent, DateTime startDateTime, DateTime endDateTime)
        {
            myEvent.Start = new EventDateTime();
            myEvent.End = new EventDateTime();

            myEvent.Start.DateTimeDateTimeOffset = startDateTime;
            myEvent.End.DateTimeDateTimeOffset = endDateTime;
        }

        /// <summary>
        /// Turns the event into a timed event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static void SetTimed(this Event myEvent, DateTime startDate, DateTime startTime, DateTime endDate, DateTime endTime)
        {
            myEvent.SetTimed(startDate.Date + startTime.TimeOfDay, endDate.Date + endTime.TimeOfDay);
        }
    }
}