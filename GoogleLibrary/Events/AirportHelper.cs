using Ibistic.Public.OpenAirportData;
using Ibistic.Public.OpenAirportData.MemoryDatabase;
using Ibistic.Public.OpenAirportData.OpenFlightsData;

namespace GoogleLibrary.Events
{
    public static class AirportHelper
    {
        public static Airport GetAirportOrDefault(string airportId)
        {
            if (string.IsNullOrWhiteSpace(airportId))
                return null;
            var airportProvider = new OpenFlightsDataAirportProvider("airports.cache", new OpenFlightsDataCountryProvider("countries.cache"));
            AirportIataCodeDatabase airportCodes = new AirportIataCodeDatabase();
            airportCodes.AddOrUpdateAirports(airportProvider.GetAllAirports(), true, true);

            if (airportCodes.TryGetAirport(airportId, out Airport airport))
                return airport;
            return null;
        }

        /// <summary>
        /// Checks if the location could be an airport 3-letter code
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool IsAirport(this string location)
        {
            return GetAirportOrDefault(location) != null;
        }

        /// <summary>
        /// Sets the airport suffix or shortname
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void SetAddresses(Location from, Location to)
        {
            if (from is AirportLocation && to is not AirportLocation)
            {
                from.Address = $"{((AirportLocation)from).AirportInformation.Name} Arrivals";
            }
            else if (from is not AirportLocation && to is AirportLocation)
            {
                to.Address = $"{((AirportLocation)to).AirportInformation.Name} Departures";
            }
        }
    }
}