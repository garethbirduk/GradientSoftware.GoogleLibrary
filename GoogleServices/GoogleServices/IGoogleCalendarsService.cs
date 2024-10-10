using Google.Apis.Calendar.v3.Data;

namespace GoogleServices.GoogleServices
{
    public interface IGoogleCalendarsService
    {
        CalendarListEntry CreateCalendar(string summary);

        Task<CalendarListEntry> CreateCalendarAsync(string summary, bool allowDuplicate = false);

        Task<CalendarListEntry> CreateOrGetCalendarAsync(string summary, bool clear = false);

        Task DeleteCalendarAsync(string calendarId);
    }
}