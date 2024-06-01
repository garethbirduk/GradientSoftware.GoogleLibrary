using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary.Custom
{
    public class TravelEvent : BasicEvent
    {
        protected override List<int> DefaultRemindersInMinutes { get; set; } = new List<int>() { 1 * 60, 2 * 60 };

        internal override List<string> AddCustomSummary()
        {
            var list = new List<string>
            {
                RouteSummary
            };
            return list;
        }

        public string RouteSummary
        {
            get
            {
                var locations = Locations.Where(x => !string.IsNullOrWhiteSpace(x.ShortName));
                if (locations.Count() == 1)
                    return $"{locations.First().ShortName}";
                if (locations.Count() > 1)
                    return $"{locations.First().ShortName} - {locations.Last().ShortName}";
                return "";
            }
        }
    }
}