using System.Collections.Generic;
using System;
using GoogleLibrary.Custom;
using System.Linq;

namespace GoogleLibrary
{
    public static class RemindersHelper
    {
        public static void EnsureNoRemindersInSilentPeriods(DateTime eventStartDateTime, IEnumerable<int> reminders, List<(TimeSpan Start, TimeSpan End)> silentPeriods, EnumHowToFixSilentReminders strategy = EnumHowToFixSilentReminders.Remove)
        {
            if (strategy == EnumHowToFixSilentReminders.Ignore)
                return;

            foreach (var reminder in reminders)
            {
                if (ReminderIsInSilentPeriod(eventStartDateTime, reminder, silentPeriods))
                {
                    switch (strategy)
                    {
                        case EnumHowToFixSilentReminders.Remove:
                            {
                                break;
                            }

                        case EnumHowToFixSilentReminders.Fix:
                            // Adjust reminders for events that occur within or after silent reminder periods
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a reminder in minutes before the event start is within any silent periods.
        /// </summary>
        /// <param name="eventStartDateTime">The start datetime of the event.</param>
        /// <param name="reminder">The reminder time in minutes before the event start.</param>
        /// <param name="silentPeriods">A list of silent periods defined as tuples of start and end times.</param>
        /// <returns>True if the reminder is within a silent period; otherwise, false.</returns>
        public static bool ReminderIsInSilentPeriod(DateTime eventStartDateTime, int reminder, List<(TimeSpan Start, TimeSpan End)> silentPeriods)
        {
            DateTime reminderTime = eventStartDateTime.AddMinutes(-reminder);
            return ReminderIsInSilentPeriod(reminderTime, silentPeriods);
        }

        /// <summary>
        /// Determines if a specific DateTime for a reminder is within any silent periods.
        /// </summary>
        /// <param name="reminder">The specific datetime of the reminder.</param>
        /// <param name="silentPeriods">A list of silent periods defined as tuples of start and end times.</param>
        /// <returns>True if the reminder falls within any of the silent periods; otherwise, false.</returns>
        public static bool ReminderIsInSilentPeriod(DateTime reminder, List<(TimeSpan Start, TimeSpan End)> silentPeriods)
        {
            TimeSpan reminderTime = reminder.TimeOfDay;
            foreach (var (start, end) in silentPeriods)
            {
                TimeSpan adjustedEnd = end.TotalHours == 24 ? TimeSpan.FromHours(24) : end; // Adjust for midnight end.

                if (reminderTime >= start && reminderTime < end)
                {
                    return true;
                }
            }
            return false;
        }
    }
}