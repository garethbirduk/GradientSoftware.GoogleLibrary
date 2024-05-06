using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.GoogleExtensions
{
    public static class GoogleEventsExtensions
    {
        public static List<string> ToStrings(this IList<Event> events)
        {
            var list = new List<string>()
            {
                "Upcoming events:"
            };

            if (events != null && events.Count > 0)
            {
                foreach (var eventItem in events)
                {
                    var startDateTime = eventItem.Start.DateTimeDateTimeOffset.ToString();
                    if (String.IsNullOrEmpty(startDateTime))
                    {
                        startDateTime = eventItem.Start.Date;
                    }
                    list.Add($"{eventItem.Summary} ({startDateTime})");
                }
            }
            else
            {
                list.Add("No upcoming events found.");
            }
            return list;
        }
    }
}