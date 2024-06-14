using Gradient.Utils;

namespace GoogleLibrary.Events
{
    /// <summary>
    /// A basic event is an event that contains most of the standard properties to be considered in a calendar setup.
    /// </summary>
    public class BasicEvent
    {
        /// <summary>
        /// One to one mapping of a status indicator and its enum value.
        /// </summary>
        protected Dictionary<EventStatus, string> EventStatusMap = new()
        {
            { EventStatus.None, "" },
            { EventStatus.Idea, "i" },
            { EventStatus.Planned, "p" },
            { EventStatus.Confirmed, "c" },
            { EventStatus.Paid, "£" },
            { EventStatus.Cancelled, "x" },
            { EventStatus.Reserved, "r" },
        };

        /// <summary>
        /// The default reminders in minutes for instances of this type.
        /// </summary>
        protected virtual List<int> DefaultRemindersInMinutes { get; set; } = [1 * 60];

        ///// <summary>
        ///// Get a list of attendess.
        ///// </summary>
        ///// <param name = "enumEventFieldType" ></ param >
        ///// < param name="fields"></param>
        ///// <param name = "data" ></ param >
        ///// < returns ></ returns >
        //protected static List<EnumAttendeeField> GetAttendees(EnumEventFieldType enumEventFieldType, List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        //{
        //    var list = GetList<string>(enumEventFieldType, fields, data);
        //    var attendees = new List<EnumAttendeeField>();
        //    foreach (var item in list)
        //    {
        //        //attendees.Add();
        //    }
        //    return attendees;
        //}

        /// <summary>
        /// Build the locations from 'from, to, via' etc.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="via"></param>
        protected void SetLocations(Location from, Location to, params Location[] via)
        {
            Locations =
            [
                from,
                .. via,
                to
            ];

            foreach (var location in Locations)
            {
                AirportHelper.SetAddresses(from, to);
            }
        }

        /// <summary>
        /// The event id
        /// </summary>
        public readonly Guid EventId = Guid.NewGuid();

        /// <summary>
        /// List of other unspecified data not held in key-value form.
        /// </summary>
        public List<string> AdditionalData { get; private set; } = [];

        /// <summary>
        /// Event category.
        /// </summary>
        public EventCategory Category { get; set; } = EventCategory.None;

        ///// <summary>
        ///// List of attendees known enum form.
        ///// </summary>
        //public IEnumerable<EnumAttendeeField> Attendees { get; set; } = new List<EnumAttendeeField>();
        /// <summary>
        /// Attendee-contact info key-value pairs.
        /// </summary>
        public Dictionary<string, string> ContactableAttendees { get; set; } = [];

        /// <summary>
        /// Custom fields in key-value form.
        /// </summary>
        public Dictionary<string, string> CustomFields { get; private set; } = [];

        /// <summary>
        /// The description.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// The optional end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The optional end time; note that the date portion of the DateTime is ignored.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// The ordered locations referenced in this event, including start, end and via points.
        /// </summary>
        public List<Location> Locations { get; set; } = [];

        //public EnumHowToFixSilentReminders HowToFixSilentReminders { get; set; } = EnumHowToFixSilentReminders.Remove;
        /// <summary>
        /// The reminder in minutes.
        /// </summary>
        public List<int> Reminders { get; set; } = [];

        /// <summary>
        /// The start date of the event.
        /// </summary>
        public DateTime StartDate { get; set; }

        public DateTime? StartTime { get; set; }

        /// <summary>
        /// The event status as an enum type.
        /// </summary>
        public EventStatus Status { get; set; } = EventStatus.None;

        /// <summary>
        /// The summary of the event composed of constituent summary parts.
        /// </summary>
        public List<string> Summary
        {
            get
            {
                var list = new List<string>();
                list.AddRange(SummaryPrefix);
                list.AddRange(AddCustomSummary());
                list.AddRange(SummarySuffix);
                return list.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            }
        }

        /// <summary>
        /// An optional prefix that is inserted before the generated summary.
        /// </summary>
        public List<string> SummaryPrefix { get; set; } = [];

        /// <summary>
        /// An optional suffix that is appended to the generated summary.
        /// </summary>
        public List<string> SummarySuffix { get; set; } = [];

        /// <summary>
        /// The title of the event. Also known as the Name or simply the Event.
        /// </summary>
        public string Title { get; set; } = "";

        public static List<int> FinalizeReminders(List<int> reminders)
        {
            // Removes duplicates and sorts the reminders
            return reminders.Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Add a custom summary to the summary builder based on title, status etc.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> AddCustomSummary()
        {
            var list = new List<string>()
            {
                Title
            };

            if (Status != EventStatus.None)
                list.Add($"({EventStatusMap[Status]})");
            if (Category != EventCategory.None)
                list.Add($"{Category}:");
            return list;
        }

        /// <summary>
        /// Build a basic event. Can be overridden in derived classes.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        public virtual void Build(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        {
            Title = fields.GetStringOrDefault(EnumEventFieldType.Summary, data);
            StartDate = fields.GetDateTimeOrDefault(EnumEventFieldType.StartDate, data);
            StartTime = fields.GetDateTimeOrDefault(EnumEventFieldType.StartTime, data);
            EndDate = fields.GetDateTimeOrDefault(EnumEventFieldType.EndDate, data);
            EndTime = fields.GetDateTimeOrDefault(EnumEventFieldType.EndTime, data);
            Status = fields.GetEnumOrDefault<EnumEventFieldType, EventStatus>(EnumEventFieldType.Status, data, true);
            Category = fields.GetEnumOrDefault<EnumEventFieldType, EventCategory>(EnumEventFieldType.Category, data, true);
            Description = fields.GetStringOrDefault(EnumEventFieldType.Description, data);
            Reminders = fields.GetList<int>(EnumEventFieldType.Reminders, data);
            var from = fields.CreateLocation(EnumEventFieldType.From, EnumEventFieldType.FromAddress, data);
            var to = fields.CreateLocation(EnumEventFieldType.To, EnumEventFieldType.ToAddress, data);
            var via1 = fields.CreateLocation(EnumEventFieldType.Via, EnumEventFieldType.ViaAddress, data);
            var via2 = fields.CreateLocation(EnumEventFieldType.Via2, EnumEventFieldType.ViaAddress2, data);

            SetLocations(from, to, via1, via2);
            SetCustomFields(fields, data);

            if (Reminders.Count == 0)
                Reminders = DefaultRemindersInMinutes;
            //Reminders = SilenceReminders(HowToFixSilentReminders, StartDate, StartTime, Reminders, SlientRemindersPeriods);

            //            Attendees = GetAttendees(EnumEventFieldType.ContactableAttendees, fields, data);
        }

        /// <summary>
        /// Append unknown fields as custom key-value pairs, and additional data that is not identifiable by the fields argument.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        public void SetCustomFields(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        {
            var list = fields.Where(x => x.Item2.EnumAttributeFirstOrDefault<DescriptionAttribute>() != null);

            CustomFields = [];
            foreach (var item in list)
            {
                var index = fields.IndexOf(item);
                var value = data[index];
                if (string.IsNullOrWhiteSpace(item.Item1))
                    AdditionalData.Add(value);
                else if (!string.IsNullOrWhiteSpace(value))
                    CustomFields.Add(item.Item1, value);
            }
            var additionalDataIndex = fields.Count;
            while (additionalDataIndex < data.Count)
            {
                AdditionalData.Add(data[additionalDataIndex]);
                additionalDataIndex++;
            }
        }

        //public static DateTime AdjustForSilentPeriods(DateTime reminderTime, List<(TimeSpan Start, TimeSpan End)> silentPeriods)
        //{
        //    foreach (var (start, end) in silentPeriods)
        //    {
        //        DateTime periodStart = reminderTime.Date + start; // Calculate period start based on reminder time's date
        //        DateTime periodEnd = reminderTime.Date + end;     // Calculate period end based on reminder time's date

        //        // Adjust for overnight silent periods
        //        if (end < start)
        //        {
        //            periodEnd = periodEnd.AddDays(1);
        //        }

        //        // Check if the reminder is within this silent period
        //        if (reminderTime >= periodStart && reminderTime < periodEnd)
        //        {
        //            // Adjust the reminder to just before the start of the silent period
        //            return periodStart.AddMinutes(-1);
        //        }
        //    }
        //    return reminderTime; // Return unmodified if no adjustments needed
        //}

        //public static DateTime AdjustForSilentPeriods(DateTime reminderTime, DateTime eventStart, List<(TimeSpan Start, TimeSpan End)> silentPeriods)
        //{
        //    TimeSpan reminderTimeOnly = reminderTime.TimeOfDay;  // Extract only the time part of reminderTime

        //    foreach (var (start, end) in silentPeriods)
        //    {
        //        DateTime periodStart = eventStart.Date + start;
        //        DateTime periodEnd = eventStart.Date + end;

        //        // Handle overnight periods where end time is the next day
        //        if (end < start)
        //        {
        //            periodEnd = periodEnd.AddDays(1);
        //        }

        //        // Adjust checking to see if reminderTime falls within the period
        //        if ((reminderTimeOnly >= start && reminderTimeOnly < end) ||
        //            (end < start && (reminderTimeOnly >= start || reminderTimeOnly < end)))
        //        {
        //            // Adjust reminder to one minute before the silent period starts
        //            // If the period spans over midnight and the reminder time is after midnight, adjust to just before midnight
        //            if (reminderTimeOnly >= start && start <= end)
        //            {
        //                reminderTime = periodStart.AddMinutes(-1);
        //            }
        //            else
        //            {
        //                // If reminder time is after midnight, adjust to just before the start of the period
        //                reminderTime = eventStart.Date.AddDays(start > end ? -1 : 0) + start.Add(TimeSpan.FromMinutes(-1));
        //            }
        //            return reminderTime;
        //        }
        //    }

        //    return reminderTime;  // Return original if no adjustments were needed
        //}

        //public static DateTime EnsureMinimumReminderTime(DateTime reminderTime, DateTime eventStart, int minimumReminderMinutes)
        //{
        //    if ((eventStart - reminderTime).TotalMinutes < minimumReminderMinutes)
        //    {
        //        reminderTime = eventStart.AddMinutes(-minimumReminderMinutes);
        //    }
        //    return reminderTime;
        //}
        //public static DateTime GetAdjustedReminderTime(DateTime eventStart, DateTime reminderTime, List<(TimeSpan Start, TimeSpan End)> silentPeriods, int minimumReminderMinutes)
        //{
        //    if (!ReminderTimeIsWithinSilentPeriods(reminderTime, silentPeriods))
        //    {
        //        // Reminder time is not within any silent period, so return it as is
        //        return reminderTime;
        //    }

        //    // Find the closest silent period before the reminder time
        //    var closestPeriodBefore = silentPeriods
        //        .Where(period => period.End < reminderTime.TimeOfDay)
        //        .OrderByDescending(period => period.End)
        //        .FirstOrDefault();

        //    // Find the closest silent period after the reminder time
        //    var closestPeriodAfter = silentPeriods
        //        .Where(period => period.Start > reminderTime.TimeOfDay)
        //        .OrderBy(period => period.Start)
        //        .FirstOrDefault();

        //    // Calculate the adjusted reminder time
        //    DateTime adjustedReminderTime;
        //    if (closestPeriodBefore != default || closestPeriodAfter != default)
        //    {
        //        TimeSpan adjustedTime;
        //        if (closestPeriodBefore != default && closestPeriodAfter != default)
        //        {
        //            // Both closest periods before and after the reminder time are found
        //            adjustedTime = closestPeriodBefore.End > closestPeriodAfter.Start
        //                ? closestPeriodAfter.Start.Add(TimeSpan.FromMinutes(minimumReminderMinutes))
        //                : closestPeriodBefore.End;
        //        }
        //        else if (closestPeriodBefore != default)
        //        {
        //            // Only closest period before the reminder time is found
        //            adjustedTime = closestPeriodBefore.End.Add(TimeSpan.FromMinutes(minimumReminderMinutes));
        //        }
        //        else
        //        {
        //            // Only closest period after the reminder time is found
        //            adjustedTime = closestPeriodAfter.Start.Add(TimeSpan.FromMinutes(-minimumReminderMinutes));
        //        }
        //        adjustedReminderTime = reminderTime.Date.Add(adjustedTime);
        //    }
        //    else
        //    {
        //        // No silent periods found, so just adjust by the minimum reminder minutes
        //        adjustedReminderTime = reminderTime.AddMinutes(-minimumReminderMinutes);
        //    }

        //    return adjustedReminderTime;
        //}

        ///// <summary>
        ///// Adjusts event reminders such that they are set to avoid prescribed silent periods.
        ///// </summary>
        ///// <param name="eventStart">The start time of the event.</param>
        ///// <param name="reminders">A list of reminders expressed as minutes before the event.</param>
        ///// <param name="silentPeriods">List of silent periods during which reminders should not occur.</param>
        ///// <param name="minimumReminderMinutes">Minimum number of minutes a reminder must be set before the event.</param>
        ///// <returns>A list of adjusted reminder times in minutes before the event.</returns>
        //public static List<int> GetAdjustedReminderTimes(DateTime eventStart, IEnumerable<int> reminders, List<(TimeSpan Start, TimeSpan End)> silentPeriods, int minimumReminderMinutes)
        //{
        //    List<int> adjustedReminders = new List<int>();

        //    foreach (int reminder in reminders)
        //    {
        //        DateTime reminderTime = eventStart.AddMinutes(-reminder);
        //        DateTime originalTime = reminderTime;  // Store original time for comparison

        //        // Utilize the separate AdjustForSilentPeriods method to handle period adjustments
        //        reminderTime = AdjustForSilentPeriods(reminderTime, eventStart, silentPeriods);

        //        // Check if there was an adjustment
        //        bool isAdjusted = reminderTime != originalTime;

        //        if (isAdjusted)
        //        {
        //            // Apply minimum reminder time only if adjusted
        //            reminderTime = EnsureMinimumReminderTime(reminderTime, eventStart, minimumReminderMinutes);
        //        }

        //        int adjustedMinutes = (int)(eventStart - reminderTime).TotalMinutes;
        //        adjustedReminders.Add(adjustedMinutes);

        //        // Log final outcome
        //        Console.WriteLine($"Original Reminder: {reminder}, Final Adjusted Reminder: {adjustedMinutes}");
        //    }

        //    return adjustedReminders.Distinct().OrderBy(x => x).ToList();
        //}
        //public static bool ReminderTimeIsWithinSilentPeriods(DateTime reminderTime, List<(TimeSpan Start, TimeSpan End)> silentPeriods)
        //{
        //    return silentPeriods.Any(period =>
        //    {
        //        TimeSpan periodStart = period.Start;
        //        TimeSpan periodEnd = period.End;
        //        TimeSpan reminderTimeOfDay = reminderTime.TimeOfDay;

        //        // Handle the case where the silent period starts in one day and ends in the next day
        //        if (periodStart > periodEnd)
        //        {
        //            return reminderTimeOfDay >= periodStart || reminderTimeOfDay <= periodEnd;
        //        }
        //        else
        //        {
        //            return reminderTimeOfDay >= periodStart && reminderTimeOfDay <= periodEnd;
        //        }
        //    });
        //}

        //public static List<int> RemoveRemindersWithinSilentPeriods(DateTime eventStart, IEnumerable<int> reminders, List<(TimeSpan Start, TimeSpan End)> silentPeriods)
        //{
        //    // Initialize a list to store adjusted reminders
        //    List<int> adjustedReminders = new List<int>();

        //    // Iterate through each reminder
        //    foreach (int reminder in reminders)
        //    {
        //        // Calculate the reminder date and time
        //        DateTime reminderDateTime = eventStart.AddMinutes(-reminder);

        //        // Check if the reminder falls within any silent period
        //        bool withinSilentPeriod = false;
        //        foreach (var period in silentPeriods)
        //        {
        //            DateTime periodStart = eventStart.Date + period.Start;
        //            DateTime periodEnd = eventStart.Date + period.End;

        //            if (reminderDateTime > periodStart && reminderDateTime < periodEnd)
        //            {
        //                // Reminder falls within a silent period, skip adding it to adjusted reminders
        //                withinSilentPeriod = true;
        //                break;
        //            }
        //        }

        //        // Add the reminder to adjusted reminders if it's not within any silent period
        //        if (!withinSilentPeriod)
        //        {
        //            adjustedReminders.Add(reminder);
        //        }
        //    }

        //    return adjustedReminders;
        //}

        //public static List<int> SilenceReminders(EnumHowToFixSilentReminders howToFix, DateTime startDate, DateTime? startTime, IEnumerable<int> reminders, List<(TimeSpan Start, TimeSpan End)> silentPeriods, int minimumReminderMinutes = 60)
        //{
        //    // Calculate the event start date and time
        //    DateTime eventStart = startTime.HasValue ? startDate.Date.Add(startTime.Value.TimeOfDay) : startDate.Date;

        //    // Ensure distinct reminders
        //    reminders = reminders.Distinct();

        //    // Implement the logic to adjust reminders based on the selected `howToFix` strategy
        //    switch (howToFix)
        //    {
        //        case EnumHowToFixSilentReminders.Ignore:
        //            // Do nothing with reminders, they will appear regardless of any silent reminder periods
        //            return reminders.Distinct().ToList();

        //        case EnumHowToFixSilentReminders.Remove:
        //            // Remove reminders set within any silent reminder periods
        //            return RemoveRemindersWithinSilentPeriods(eventStart, reminders, silentPeriods).Distinct().ToList();

        //        case EnumHowToFixSilentReminders.Fix:
        //            // Adjust reminders for events that occur within or after silent reminder periods
        //            return reminders.ToList();

        //        default:
        //            throw new ArgumentException("Invalid enum value for howToFixSilentReminders.");
        //    }
        //}

        //public static bool TimeIsWithinPeriod(DateTime reminderDateTime, DateTime periodStart, DateTime periodEnd)
        //{
        //    return reminderDateTime >= periodStart && reminderDateTime <= periodEnd;
        //}

        ///// <summary>
        ///// Periods in the day in which reminders are not allow. Reminders set within this period will be modified or removed.
        ///// </summary>
        //public List<(TimeSpan, TimeSpan)> SlientRemindersPeriods
        //{
        //    get;
        //    set;
        //} =
        //[
        //    new(new TimeSpan(0, 0, 0), new TimeSpan(7, 0, 0)),
        //    new(new TimeSpan(22, 0, 0), new TimeSpan(23, 59, 59)),
        //];
        /// <summary>
        /// The optional end time; note that the date portion of the DateTime is ignored.
        /// </summary>

        ///// <summary>
        ///// Get a list of contactable attendees.
        ///// </summary>
        ///// <param name="fields"></param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //protected static Dictionary<string, string> GetContactableAttendees(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        //{
        //    var dictionary = new Dictionary<string, string>();
        //    foreach (var item in GetList<string>(EnumEventFieldType.ContactableAttendees, fields, data))
        //    {
        //        var list = item.Split(":");
        //        dictionary.Add(list.First().Trim(), list.Last().Trim());
        //    }
        //    return dictionary;
        //}
        /// <summary>
        /// Retrieve data as a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumEventFieldType"></param>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        /// <returns></returns>
    }
}