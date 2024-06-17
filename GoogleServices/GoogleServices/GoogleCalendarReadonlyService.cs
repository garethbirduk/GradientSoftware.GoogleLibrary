using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System.Collections.Generic;

namespace GoogleServices
{
    public class GoogleCalendarReadonlyService : GoogleCalendarEventsService
    {
        public override IEnumerable<string> Scopes => new List<string>() { CalendarService.Scope.Calendar };

        public Calendar Calendar(string calendarId)
        {
            return GoogleCalendarService.Calendars.Get(calendarId).Execute();
        }
    }
}