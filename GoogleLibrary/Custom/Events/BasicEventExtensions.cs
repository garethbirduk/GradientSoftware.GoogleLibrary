using Google.Apis.Calendar.v3.Data;

namespace GoogleLibrary.Custom.Events
{
    public static class BasicEventExtensions
    {
        /// <summary>
        /// Sentinel line written between the free-text Description and the structured
        /// CustomFields/AdditionalData tail in the calendar event description.
        /// Lets the inverse parser (EventBuilder.Create) recover Description verbatim even if
        /// it contains "Key: value" patterns that would otherwise get reclassified.
        /// </summary>
        public const string DescriptionSectionDelimiter = "---";

        public static List<string> GetCustomFieldsAsDescription(BasicEvent baseEvent)
        {
            var list = new List<string>();
            foreach (var entry in baseEvent.CustomFields.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                list.Add($"{entry.Key.Trim()}: {entry.Value.Trim()}");
            return list;
        }

        public static string ToDescriptionString(this BasicEvent baseEvent)
        {
            var description = baseEvent.Description ?? "";
            var customFieldLines = GetCustomFieldsAsDescription(baseEvent);
            var additionalLines = baseEvent.AdditionalData
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            // If there's no structured tail, emit Description alone.
            if (customFieldLines.Count == 0 && additionalLines.Count == 0)
                return description;

            // Otherwise insert the sentinel between Description and the structured tail
            // so the inverse parser can recover the boundary precisely.
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(description))
                parts.Add(description);
            parts.Add(DescriptionSectionDelimiter);
            parts.AddRange(customFieldLines);
            parts.AddRange(additionalLines);
            return string.Join("\r\n", parts);
        }

        public static string ToLocationString(this BasicEvent baseEvent)
        {
            var locations = baseEvent.Locations.Select(x => x.Address).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!locations.Any())
                return "";

            var prefix = locations.Count > 1 ? "dir" : "search";
            var baseString = $"https://www.google.com/maps/{prefix}/";
            // Per-segment percent-encoding so that &, ?, #, /, non-ASCII etc. survive the URL.
            // Spaces become "+" for readability — Google Maps accepts both "+" and "%20".
            var encoded = locations.Select(l => Uri.EscapeDataString(l).Replace("%20", "+"));
            return $"{baseString}{string.Join("/", encoded)}";
        }

        public static List<EventReminder> ToReminderOverrides(this BasicEvent baseEvent)
        {
            var overrides = new List<EventReminder>();

            foreach (var reminder in baseEvent.Reminders)
                overrides.Add(new EventReminder()
                {
                    Method = "popup",
                    Minutes = reminder,
                });

            return overrides;
        }
    }
}