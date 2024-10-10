using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace GoogleServices.GoogleServices
{
    public class GoogleCalendarReadonlyService : GoogleCalendarEventsService
    {
        public static List<string> RequiredScopes = new List<string>()
            { CalendarService.Scope.Calendar };

        public GoogleCalendarReadonlyService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }

        public Calendar Calendar(string calendarId)
        {
            return GoogleCalendarService.Calendars.Get(calendarId).Execute();
        }
    }
}