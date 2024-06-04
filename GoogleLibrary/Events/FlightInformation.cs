namespace GoogleLibrary.Events
{
    public class FlightInformation
    {
        public string Carrier { get; set; }

        public string FlightDetails
        {
            get
            {
                if (string.IsNullOrEmpty(Number))
                    return $"{Carrier}".Trim();
                else
                    return $"{Carrier} ({Number})".Trim();
            }
        }

        public string FlightSummary
        {
            get
            {
                if (string.IsNullOrEmpty(Number))
                    return "";
                else
                    return $"({Number})".Trim();
            }
        }

        public string FlightTracker
        {
            get
            {
                if (string.IsNullOrEmpty(Number))
                    return "";
                else
                    return $"https://www.flightradar24.com/{Number}".Trim();
            }
        }

        public string Number { get; set; }
    }
}