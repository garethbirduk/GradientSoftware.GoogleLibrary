using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary.GoogleServices
{
    public class GoogleCalendarsReadonlyService : GoogleWebAuthorizationBrokeredScopedService
    {
        public CalendarService GoogleService { get; set; }

        public override IEnumerable<string> Scopes => new List<string>() { CalendarService.Scope.CalendarReadonly };

        /// <summary>
        /// Gets a calendar.
        /// </summary>
        /// <param name="predicate">selects the primary calendar by default</param>
        /// <returns>The calendar as CalendarListEntry</returns>
        public CalendarListEntry GetCalendar(Func<CalendarListEntry, bool> predicate = null)
        {
            predicate ??= x => x.Primary == true;
            return GetCalendars().Items.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Gets a calendar.
        /// </summary>
        /// <param name="id">The calendar id</param>
        /// <returns>The calendar as CalendarListEntry</returns>
        public CalendarListEntry GetCalendar(string id)
        {
            return GoogleService.CalendarList.Get(id).Execute();
        }

        /// <summary>
        /// Gets a calendar.
        /// </summary>
        /// <param name="summary">The exact calendar summary.</param>
        /// <returns>The calendar as CalendarListEntry</returns>
        public CalendarListEntry GetCalendarBySummary(string summary)
        {
            return GetCalendar(x => x.Summary == summary);
        }

        /// <summary>
        /// Gets all calendars.
        /// </summary>
        public CalendarList GetCalendars()
        {
            return GoogleService.CalendarList.List().Execute();
        }

        /// <summary>
        /// Gets all calendars.
        /// </summary>
        public CalendarList GetCalendars(Func<CalendarListEntry, bool> predicate)
        {
            var calendarList = GoogleService.CalendarList.List().Execute();
            calendarList.Items = calendarList.Items.Where(predicate).ToList();
            return calendarList;
        }

        public override void SetupExternalServices()
        {
            GoogleService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = "Google Calender API v3",
            });
        }
    }
}