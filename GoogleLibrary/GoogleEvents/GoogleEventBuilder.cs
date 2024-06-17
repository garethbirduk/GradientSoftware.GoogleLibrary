﻿using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Events;
using GoogleLibrary.GoogleExtensions;
using PostSharp.Patterns.Contracts;

namespace GoogleLibrary.GoogleEvents
{
    /// <summary>
    /// Build a Google Event
    /// </summary>
    public static class GoogleEventBuilder
    {
        //internal static void WithAttendees(BasicEvent baseEvent, Event googleEvent)
        //{
        //    if (baseEvent.Attendees == null)
        //        googleEvent.Attendees = new List<EventAttendee>();

        //    googleEvent.GuestsCanSeeOtherGuests = true;
        //    foreach (var attendee in baseEvent.Attendees)
        //    {
        //        googleEvent.Attendees.Add(new EventAttendee()
        //        {
        //            DisplayName = attendee.ToString(),
        //            Email = $"{Guid.NewGuid()}@{Guid.NewGuid()}.com", //attribute.Email
        //        });
        //    }
        //    foreach (var attendee in baseEvent.ContactableAttendees)
        //    {
        //        googleEvent.Attendees.Add(new EventAttendee()
        //        {
        //            DisplayName = attendee.Key,
        //            Email = attendee.Value
        //        });
        //    }
        //}

        internal static Event Build(this Event googleEvent)
        {
            if (googleEvent == null) throw new NullReferenceException(nameof(googleEvent));
            if (string.IsNullOrWhiteSpace(googleEvent.Summary)) throw new GoogleEventBuilderException("Summary missing.");
            if (googleEvent.Start.DateTimeDateTimeOffset == null) throw new NullReferenceException(nameof(googleEvent.Start.DateTimeDateTimeOffset));
            return googleEvent;
        }

        internal static Event WithColour(this Event googleEvent, EventStatus eventStatus)
        {
            switch (eventStatus)
            {
                case EventStatus.None:
                    break;

                case EventStatus.Idea:
                    {
                        googleEvent.ColorId = ColorId.Orange.ToString();
                        break;
                    }
                case EventStatus.Planned:
                    {
                        googleEvent.ColorId = ColorId.Yellow.ToString();
                        break;
                    }
                case EventStatus.Confirmed:
                    {
                        googleEvent.ColorId = ColorId.Green.ToString();
                        break;
                    }
                case EventStatus.Reserved:
                    {
                        googleEvent.ColorId = ColorId.Green.ToString();
                        break;
                    }
                case EventStatus.Paid:
                    {
                        googleEvent.ColorId = ColorId.Cyan.ToString();
                        break;
                    }
                case EventStatus.Cancelled:
                    {
                        googleEvent.ColorId = ColorId.Red.ToString();
                        break;
                    }
                default:
                    break;
            }
            return googleEvent;
        }

        internal static Event WithDescription(this Event googleEvent, string description)
        {
            googleEvent.Description = description;
            return googleEvent;
        }

        internal static Event WithLocation(this Event googleEvent, string location)
        {
            googleEvent.Location = location;
            return googleEvent;
        }

        internal static Event WithReminders(this Event googleEvent, List<EventReminder> overrides)
        {
            googleEvent.Reminders = new Event.RemindersData
            {
                Overrides = overrides,
                UseDefault = false,
            };
            return googleEvent;
        }

        /// <summary>
        /// Create a Google Event based on a Basic event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <returns>The Google Event</returns>
        public static Event Create([Required] BasicEvent myEvent)
        {
            return new Event()
            {
                Summary = myEvent.Title
            }
            //WithAttendees(myEvent, googleEvent);
            .WithDates(myEvent.StartDate, myEvent.StartTime, myEvent.EndDate, myEvent.EndTime)
            .WithDescription(myEvent.ToDescriptionString())
            .WithLocation(myEvent.ToLocationString())
            .WithReminders(myEvent.ToReminderOverrides())
            .WithColour(myEvent.Status)
            .Build();
        }
    }
}