using PostSharp.Patterns.Contracts;
using System;

namespace GoogleLibrary.Events
{
    public static class AirportHelper
    {
        /// <summary>
        /// Checks if the location could be an airport 3-letter code
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool IsAirport(this Location location)
        {
            return location.ShortName.Equals(location.ShortName, StringComparison.CurrentCultureIgnoreCase) && location.ShortName.Length == 3;
        }

        /// <summary>
        /// Checks if the location is an airport to airport route.
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <returns></returns>
        public static bool IsAirportToAirport(this Location location1, [Required] Location location2)
        {
            if (location1.ShortName == location2.ShortName)
                return false;
            return location1.IsAirport() && location2.IsAirport();
        }
    }
}