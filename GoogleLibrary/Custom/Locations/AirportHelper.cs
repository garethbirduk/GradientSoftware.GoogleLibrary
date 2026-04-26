using Ibistic.Public.OpenAirportData;
using Ibistic.Public.OpenAirportData.MemoryDatabase;
using Ibistic.Public.OpenAirportData.OpenFlightsData;

namespace GoogleLibrary.Custom.Locations
{
    public static class AirportHelper
    {
        private static readonly Lazy<AirportIataCodeDatabase> _airportCodes = new(() =>
        {
            var airportProvider = new OpenFlightsDataAirportProvider("airports.cache", new OpenFlightsDataCountryProvider("countries.cache"));
            var db = new AirportIataCodeDatabase();
            db.AddOrUpdateAirports(airportProvider.GetAllAirports(), true, true);
            return db;
        });

        public static Airport? GetAirportOrDefault(string airportId)
        {
            if (string.IsNullOrWhiteSpace(airportId))
                return null;

            if (_airportCodes.Value.TryGetAirport(airportId, out Airport airport))
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
            if (from is AirportLocation location && to is not AirportLocation)
            {
                from.Address = $"{location.AirportInformation.Name} Arrivals";
            }
            else if (from is not AirportLocation && to is AirportLocation)
            {
                to.Address = $"{((AirportLocation)to).AirportInformation.Name} Departures";
            }
        }
    }
}