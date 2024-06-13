using Ibistic.Public.OpenAirportData;

namespace GoogleLibrary.Events
{
    public class AirportLocation : Location
    {
        public AirportLocation(string airportId)
        {
            AirportId = airportId;
            AirportInformation = AirportHelper.GetAirportOrDefault(airportId);
            ShortName = AirportInformation.Name;
            Address = AirportInformation.Name;
        }

        public string AirportId { get; set; }
        public Airport AirportInformation { get; set; }
    }
}