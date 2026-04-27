namespace GoogleLibrary.Custom.Events
{
    public class AccommodationEvent : BasicEvent
    {
        internal override List<int> DefaultRemindersInMinutes { get; set; } = [12 * 60];

        public override void Build(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        {
            base.Build(fields, data);
            // If the sheet's Summary column was empty, fall back to the first location's
            // short name, or "TBC" if no location was specified either.
            if (string.IsNullOrWhiteSpace(Title))
            {
                var firstShortName = Locations.FirstOrDefault()?.ShortName?.Trim();
                Title = string.IsNullOrWhiteSpace(firstShortName) ? "TBC" : firstShortName;
            }
        }
    }
}
