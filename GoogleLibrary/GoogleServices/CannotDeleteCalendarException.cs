using System;
using Gradient.Utils;

namespace GoogleLibrary.GoogleServices
{
    public class CannotDeleteCalendarException : Exception
    {
        public CannotDeleteCalendarException()
        { }

        public CannotDeleteCalendarException(string message) : base(message)
        {
        }

        public CannotDeleteCalendarException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CannotDeleteCalendarException(string calendarId, CannotDeleteCalendarReasons cannotDeleteCalendarReason) : base($"Cannot delete calendar with id: {calendarId} - {cannotDeleteCalendarReason.DisplayDescriptionOrDefault()}")
        {
        }

        // You can add additional constructors or custom properties as needed.
    }
}