using System.Collections.Generic;

namespace GoogleLibrary.Events
{
    public class AccommodationEvent : BasicEvent
    {
        protected override List<int> DefaultRemindersInMinutes { get; set; } = new List<int>() { 12 * 60 };

        internal override List<string> AddCustomSummary()
        {
            var list = new List<string>();
            if (Location == null || Location.ShortName == "")
                list.Add("TBC");
            return list;
        }

        public Location Location { get; set; }

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