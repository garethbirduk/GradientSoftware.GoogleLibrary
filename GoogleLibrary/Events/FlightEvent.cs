namespace GoogleLibrary.Events
{
    public class FlightEvent : TravelEvent
    {
        protected override List<int> DefaultRemindersInMinutes { get; set; } = [2 * 60, 4 * 60,];

        public FlightInformation FlightInformation { get; private set; } = new FlightInformation();

        public override List<string> AddCustomSummary()
        {
            var list = new List<string>
            {
                FlightInformation.FlightSummary,
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
        }
    }
}