using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace GoogleServices
{
    /// <summary>
    /// Service to get calendar events for a given calendar.
    /// </summary>
    public class GoogleCalendarEventsReadonlyService : GoogleWebAuthorizationBrokeredScopedService
    {
        /// <summary>
        /// The google calendar service for accessing the calendar to which the events belong.
        /// </summary>
        public CalendarService GoogleCalendarService { get; set; }

        public override IEnumerable<string> Scopes => new List<string>() { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents }.Distinct();

        /// <summary>
        /// Get a single event by id
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <returns>The event.</returns>
        public Event GetEvent(string calendarId, string eventId)
        {
            return GoogleCalendarService.Events.Get(calendarId, eventId).Execute();
        }

        /// <summary>
        /// Get the events in a calendar. By default restrict events to those from one month ago or newer.
        /// </summary>
        /// <param name="maxResults">Limit the number of events.</param>
        /// <param name="minDate">Restrict events to those from specified Date or newer.</param>
        /// <returns></returns>
        public Google.Apis.Calendar.v3.Data.Events GetEvents(string calendarId, int maxResults = 100, DateTime? minDate = null)
        {
            var now = DateTime.UtcNow.AddMonths(-1);
            if (minDate == null)
                minDate = now.Date;
            EventsResource.ListRequest request = GoogleCalendarService.Events.List(calendarId);
            request.TimeMinDateTimeOffset = minDate.Value;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = maxResults;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            return request.Execute();
        }

        public override void SetupExternalServices()
        {
            GoogleCalendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = "Google Calender API v3",
            });
        }
    }
}