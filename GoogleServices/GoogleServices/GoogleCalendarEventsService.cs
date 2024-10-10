using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using PostSharp.Patterns.Contracts;

namespace GoogleServices.GoogleServices
{
    public class GoogleCalendarEventsService : GoogleCalendarEventsReadonlyService
    {
        public static List<string> RequiredScopes = new List<string>()
            { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents };

        public GoogleCalendarEventsService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }

        public async Task ClearEvents(string calendarId)
        {
            await DeleteEventsAsync(calendarId, x => true);
        }

        public Event? CreateEvent(string calendarId, [Required] Event myEvent)
        {
            if (string.IsNullOrWhiteSpace(myEvent.Summary))
            {
                return null;
            }
            return GoogleCalendarService.Events.Insert(myEvent, calendarId).Execute();
        }

        public IEnumerable<Event> CreateEvents(string calendarId, IEnumerable<Event> events)
        {
            var list = new List<Event>();
            foreach (var myEvent in events.Where(x => !string.IsNullOrWhiteSpace(x.Summary)))
                list.Add(CreateEvent(calendarId, myEvent));
            return list;
        }

        public async Task DeleteEventAsync(string calendarId, string eventId)
        {
            var request = GoogleCalendarService.Events.Delete(calendarId, eventId);
            await request.ExecuteAsync();
        }

        public async Task DeleteEventsAsync(string calendarId, Func<Event, bool> predicate)
        {
            foreach (var myEvent in GetEvents(calendarId).Items.Where(predicate))
                await DeleteEventAsync(calendarId, myEvent.Id);
        }

        /// <summary>
        /// Modify existing events
        /// </summary>
        /// <param name="myEvents">events to modify</param>
        /// <exception cref="ArgumentException"></exception>
        public void PatchEvent(string calendarId, params Event[] myEvents)
        {
            if (myEvents.Any(x => string.IsNullOrWhiteSpace(x.Id)))
                throw new ArgumentException("Event.Id missing");
            foreach (var myEvent in myEvents)
                GoogleCalendarService.Events.Patch(myEvent, calendarId, myEvent.Id).Execute();
        }

        /// <summary>
        /// Modify existing events
        /// </summary>
        /// <param name="myEvents">events to modify</param>
        /// <exception cref="ArgumentException"></exception>
        public void ReplaceEvent(string calendarId, params Event[] myEvents)
        {
            if (myEvents.Any(x => string.IsNullOrWhiteSpace(x.Id)))
                throw new ArgumentException("Event.Id missing");
            foreach (var myEvent in myEvents)
                GoogleCalendarService.Events.Update(myEvent, calendarId, myEvent.Id).Execute();
        }
    }
}