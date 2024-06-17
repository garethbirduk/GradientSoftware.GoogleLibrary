using GoogleLibrary.Locations;

namespace GoogleLibrary.Events
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

        public override List<string> AddCustomSummary()
        {
            var list = new List<string>
            {
                FlightInformation.FlightDetails,
                RouteSummary
            };
            return list;
        }

        public override void Build(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        {
            base.Build(fields, data);
            FlightInformation.Carrier = fields.GetStringOrDefault(EnumEventFieldType.FlightCarrier, data);
            FlightInformation.Number = fields.GetStringOrDefault(EnumEventFieldType.FlightNumber, data);
            CustomFields.Add("Flight", FlightInformation.FlightDetails);
            CustomFields.Add("Flight tracker", FlightInformation.FlightTracker);
            Title = $"{FlightInformation.FlightDetails} {RouteSummary}";
        }
    }
}