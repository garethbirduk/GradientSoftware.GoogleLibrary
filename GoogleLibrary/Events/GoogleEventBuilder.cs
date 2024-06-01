using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.GoogleExtensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary.Events
{
    /// <summary>
    /// Build a Google Event
    /// </summary>
    public static class GoogleEventBuilder
    {
        private static List<string> GetCustomFieldsAsDescription(BasicEvent baseEvent)
        {
            var list = new List<string>();
            foreach (var entry in baseEvent.CustomFields.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                list.Add($"{entry.Key.Trim()}: {entry.Value.Trim()}");
            return list;
        }

        //private static void SetAttendees(BasicEvent baseEvent, Event googleEvent)
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

        private static void SetColour(BasicEvent baseEvent, Event googleEvent)
        {
            switch (baseEvent.Status)
            {
                case EventStatus.None:
                    break;

                case EventStatus.Idea:
                    {
                        googleEvent.ColorId = ColorId.Orange;
                        break;
                    }
                case EventStatus.Planned:
                    {
                        googleEvent.ColorId = ColorId.Yellow;
                        break;
                    }
                case EventStatus.Confirmed:
                    {
                        googleEvent.ColorId = ColorId.Green;
                        break;
                    }
                case EventStatus.Reserved:
                    {
                        googleEvent.ColorId = ColorId.Green;
                        break;
                    }
                case EventStatus.Paid:
                    {
                        googleEvent.ColorId = ColorId.Cyan;
                        break;
                    }
                case EventStatus.Cancelled:
                    {
                        googleEvent.ColorId = ColorId.Red;
                        break;
                    }
                default:
                    break;
            }
        }

        private static void SetDates(BasicEvent baseEvent, Event googleEvent)
        {
            googleEvent.SetDates(baseEvent.StartDate, baseEvent.StartTime, baseEvent.EndDate, baseEvent.EndTime);
        }

        private static void SetDescription(BasicEvent baseEvent, Event googleEvent)
        {
            var list = new List<string>()
            {
                baseEvent.Description
            };

            list.AddRange(GetCustomFieldsAsDescription(baseEvent));
            list.AddRange(baseEvent.AdditionalData);
            googleEvent.Description = string.Join("\r\n", list.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private static void SetLocation(BasicEvent baseEvent, Event googleEvent)
        {
            var locations = baseEvent.Locations.Select(x => x.Address).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!locations.Any())
                return;
            var baseString = "https://www.google.com/maps/dir/";
            googleEvent.Location = $"{baseString}{string.Join("/", locations)}".Replace(" ", "+");
        }

        private static void SetReminders(BasicEvent baseEvent, Event googleEvent)
        {
            googleEvent.Reminders = new Event.RemindersData
            {
                Overrides = new List<EventReminder>(),
                UseDefault = false,
            };

            foreach (var reminder in baseEvent.Reminders)
                googleEvent.Reminders.Overrides.Add(new EventReminder()
                {
                    Method = "popup",
                    Minutes = reminder,
                });
        }

        /// <summary>
        /// Create a Google Event based on a Basic event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <returns>The Google Event</returns>
        public static Event Create([Required] BasicEvent myEvent)
        {
            var googleEvent = new Event()
            {
                Summary = myEvent.Title
            };
            //SetAttendees(myEvent, googleEvent);
            SetDates(myEvent, googleEvent);
            SetDescription(myEvent, googleEvent);
            SetLocation(myEvent, googleEvent);
            SetReminders(myEvent, googleEvent);
            SetColour(myEvent, googleEvent);
            return googleEvent;
        }
    }
}