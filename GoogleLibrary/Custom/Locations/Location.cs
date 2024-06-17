namespace GoogleLibrary.Custom.Locations
{
    public class Location
    {
        protected void SetShortNameAndAddress(string? shortName, string? address)
        {
            ShortName = shortName == null ? "" : shortName.Replace("/", " ").Trim();
            Address = address == null ? "" : address.Replace("/", " ").Trim();

            if (ShortName == "")
                ShortName = Address;
            if (Address == "")
                Address = ShortName;

            if (ShortName.Equals(Home, StringComparison.InvariantCultureIgnoreCase))
            {
                ShortName = Home;
                Address = "10 Bourne Close, Beeston, NG9 3BZ, UK";
            }
        }

        public readonly string Home = nameof(Home);

        public Location(string? shortName = null, string? address = null)
        {
            SetShortNameAndAddress(shortName, address);
        }

        public string Address { get; set; } = "";

        public string ShortName { get; set; } = "";
    }
}