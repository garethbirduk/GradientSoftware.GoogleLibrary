using System.Collections.Generic;

namespace GoogleLibrary.Custom
{
    public class AccommodationEvent : BasicEvent
    {
        public Location Location { get; set; }

        protected override List<int> DefaultRemindersInMinutes { get; set; } = new List<int>() { 12 * 60 };

        public string LocationSummary(string summary)
        {
            if (!string.IsNullOrWhiteSpace(summary))
                return summary;
            if (Location == null || Location.ShortName == "")
                return "TBC";
            else
                return Location.ShortName;
        }

        protected override List<string> AddCustomSummary()
        {
            var list = new List<string>();
            if (Location == null || Location.ShortName == "")
                list.Add("TBC");
            return list;
        }
    }
}