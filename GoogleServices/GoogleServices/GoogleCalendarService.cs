using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace GoogleServices.GoogleServices
{
    public class GoogleCalendarService : GoogleCalendarReadonlyService
    {
        public override IEnumerable<string> Scopes => new List<string>() { CalendarService.Scope.Calendar };

        public Calendar RenameCalendar(string calendarId, string summary)
        {
            var calendar = new Calendar()
            {
                Summary = summary,
            };
            return GoogleCalendarService.Calendars.Patch(calendar, calendarId).Execute();
        }

        public Calendar SetDescription(string calendarId, string description)
        {
            var calendar = new Calendar()
            {
                Description = description,
            };
            return GoogleCalendarService.Calendars.Patch(calendar, calendarId).Execute();
        }
    }
}