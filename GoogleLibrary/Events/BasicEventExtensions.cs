using Google.Apis.Calendar.v3.Data;

namespace GoogleLibrary.Events
{
    public static class BasicEventExtensions
    {
        public static List<string> GetCustomFieldsAsDescription(BasicEvent baseEvent)
        {
            var list = new List<string>();
            foreach (var entry in baseEvent.CustomFields.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                list.Add($"{entry.Key.Trim()}: {entry.Value.Trim()}");
            return list;
        }

        public static string ToDescriptionString(this BasicEvent baseEvent)
        {
            var list = new List<string>()
            {
                baseEvent.Description
            };

            list.AddRange(GetCustomFieldsAsDescription(baseEvent));
            list.AddRange(baseEvent.AdditionalData);
            return string.Join("\r\n", list.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public static string ToLocationString(this BasicEvent baseEvent)
        {
            var locations = baseEvent.Locations.Select(x => x.Address).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!locations.Any())
                return "";
            var baseString = "https://www.google.com/maps/dir/";
            return $"{baseString}{string.Join("/", locations)}".Replace(" ", "+");
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