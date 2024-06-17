using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Events;
using PostSharp.Patterns.Contracts;

namespace GoogleLibrary.GoogleEvents
{
    /// <summary>
    /// Build one or more Google Events
    /// </summary>
    public static class GoogleEventsBuilder
    {
        /// <summary>
        /// Create one or more Google Events based on one or more Basic event
        /// </summary>
        /// <param name="events">The basic event(s)</param>
        /// <returns>The Google Events</returns>
        public static IEnumerable<Event> Create([Required] params BasicEvent[] events)
        {
            return events.Select(x => GoogleEventBuilder.Create(x));
        }
    }
}