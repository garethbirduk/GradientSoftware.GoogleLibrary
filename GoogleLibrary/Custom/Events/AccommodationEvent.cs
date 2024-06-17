using GoogleLibrary.Custom.Locations;

namespace GoogleLibrary.Custom.Events
{
    public class AccommodationEvent : BasicEvent
    {
        internal override List<int> DefaultRemindersInMinutes { get; set; } = [12 * 60];

        public Location Location { get; set; } = new();

        public override List<string> AddCustomSummary()
        {
            var list = new List<string>();
            if (Location == null || Location.ShortName == "")
                list.Add("TBC");
            return list;
        }

        public string LocationSummary(string? summary)
        {
            if (!string.IsNullOrWhiteSpace(summary))
                return summary;
            if (Location == null || Location.ShortName.Trim() == "")
                return "TBC";
            else
                return Location.ShortName.Trim();
        }
    }
}