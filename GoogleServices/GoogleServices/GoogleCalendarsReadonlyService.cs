﻿using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace GoogleServices.GoogleServices
{
    public class GoogleCalendarsReadonlyService : GoogleAuthorizationService
    {
        public static List<string> RequiredScopes = new List<string>()
            { CalendarService.Scope.CalendarReadonly };

        public GoogleCalendarsReadonlyService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }

        public CalendarService GoogleService { get; set; }

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
            return GetCalendars(x => true);
        }

        /// <summary>
        /// Gets all calendars according to predicate filter.
        /// </summary>
        public CalendarList GetCalendars(Func<CalendarListEntry, bool> predicate)
        {
            var calendarList = GoogleService.CalendarList.List().Execute();
            calendarList.Items = calendarList.Items.Where(predicate).ToList();
            return calendarList;
        }

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
            GoogleService = new CalendarService(initializer);
        }
    }
}