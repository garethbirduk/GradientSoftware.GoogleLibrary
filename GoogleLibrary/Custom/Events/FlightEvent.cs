using GoogleLibrary.Custom.Locations;

namespace GoogleLibrary.Custom.Events
{
    public class FlightEvent : TravelEvent
    {
        internal override List<int> DefaultRemindersInMinutes { get; set; } = [2 * 60, 4 * 60,];

        public FlightInformation FlightInformation { get; private set; } = new FlightInformation();

        public override string RouteSummary
        {
            get
            {
                var locations = Locations.OfType<AirportLocation>().Where(x => !string.IsNullOrWhiteSpace(x.AirportId));
                if (locations.Count() == 1)
                    return $"{locations.First().AirportId}";
                if (locations.Count() > 1)
                    return $"{locations.First().AirportId} - {locations.Last().AirportId}";
                return base.RouteSummary;
            }
        }

        /// <summary>
        /// Custom-fields key under which any user-supplied Summary value is preserved when Build
        /// overwrites Title with the auto-derived "{Carrier} ({Number}) {Route}" string.
        /// Round-trips into the calendar event description as "UserTitle: <value>".
        /// </summary>
        public const string UserTitleKey = "UserTitle";

        public override void Build(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        {
            base.Build(fields, data);

            // Read the user's Summary value directly — Title at this point may already have been
            // back-filled to the route by TravelEvent.Build, so it's not a reliable indicator of
            // what the user actually typed in the Summary column.
            var userSummary = fields.GetStringOrDefault(EnumEventFieldType.Summary, data);

            FlightInformation.Carrier = fields.GetStringOrDefault(EnumEventFieldType.FlightCarrier, data);
            FlightInformation.Number = fields.GetStringOrDefault(EnumEventFieldType.FlightNumber, data);
            CustomFields.Add("Flight", FlightInformation.FlightDetails);
            CustomFields.Add("Flight tracker", FlightInformation.FlightTracker);

            // The auto-derived flight title (carrier + number + route) is almost always more useful
            // than whatever a user typed in the Summary column, but their input shouldn't be lost.
            // Stash it in the description so it round-trips as a custom field.
            if (!string.IsNullOrWhiteSpace(userSummary))
                CustomFields[UserTitleKey] = userSummary;

            Title = $"{FlightInformation.FlightDetails} {RouteSummary}";
        }
    }
}