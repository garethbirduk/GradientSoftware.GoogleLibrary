using GoogleLibrary.Custom.Attributes;
using GoogleLibrary.Custom.Locations;
using Gradient.Utils;
using Gradient.Utils.Attributes;
using PostSharp.Patterns.Contracts;
using System.Linq.Dynamic.Core;

namespace GoogleLibrary.Custom.Events
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

            AirportHelper.SetAddresses(from, to);
        }

        /// <summary>
        /// The default reminders in minutes for instances of this type.
        /// </summary>
        internal virtual List<int> DefaultRemindersInMinutes { get; set; } = [1 * 60];

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
        [Precondition("fields", "fields.Count > 0")]
        [Precondition("data", "data.Count > 0")]
        public virtual void Build([Required] List<Tuple<string, EnumEventFieldType>> fields, [Required] List<string> data)
        {
            Title = fields.GetStringOrDefault(EnumEventFieldType.Summary, data);
            StartDate = fields.GetDateTimeOrDefault(EnumEventFieldType.StartDate, data, null) ?? DateTime.UtcNow;
            StartTime = fields.GetDateTimeOrDefault(EnumEventFieldType.StartTime, data, null);
            EndDate = fields.GetDateTimeOrDefault(EnumEventFieldType.EndDate, data, StartDate);
            EndTime = fields.GetDateTimeOrDefault(EnumEventFieldType.EndTime, data, StartTime?.AddHours(1));
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
        [Precondition("fields", "fields.Count > 0")]
        [Precondition("data", "data.Count > 0")]
        public void SetCustomFields([Required] List<Tuple<string, EnumEventFieldType>> fields, [Required] List<string> data)
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
    }
}
