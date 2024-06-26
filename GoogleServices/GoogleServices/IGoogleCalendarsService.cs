﻿using Google.Apis.Calendar.v3.Data;

namespace GoogleServices
{
    public interface IGoogleCalendarsService
    {
        IEnumerable<string> Scopes { get; }

        CalendarListEntry CreateCalendar(string summary);

        Task<CalendarListEntry> CreateCalendarAsync(string summary, bool allowDuplicate = false);

        Task<CalendarListEntry> CreateOrGetCalendarAsync(string summary, bool clear = false);

        Task DeleteCalendarAsync(string calendarId);
    }
}