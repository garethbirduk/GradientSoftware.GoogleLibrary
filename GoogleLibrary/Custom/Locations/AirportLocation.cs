using Ibistic.Public.OpenAirportData;

namespace GoogleLibrary.Custom.Locations
{
    public class AirportLocation : Location
    {
        public AirportLocation(string airportId)
        {
            AirportId = airportId;
            AirportInformation = AirportHelper.GetAirportOrDefault(airportId)
                ?? throw new ArgumentException($"Airport code '{airportId}' not found in OpenFlights data.", nameof(airportId));
            ShortName = AirportInformation.Name;
            Address = AirportInformation.Name;
        }

        public string AirportId { get; set; }
        public Airport AirportInformation { get; set; }
    }
}