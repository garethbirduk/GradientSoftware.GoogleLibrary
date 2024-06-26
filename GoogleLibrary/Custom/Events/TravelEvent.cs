﻿namespace GoogleLibrary.Custom.Events
{
    public class TravelEvent : BasicEvent
    {
        internal override List<int> DefaultRemindersInMinutes { get; set; } = [1 * 60, 2 * 60];

        public virtual string RouteSummary
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

        public override List<string> AddCustomSummary()
        {
            var list = new List<string>
            {
                RouteSummary
            };
            return list;
        }
    }
}