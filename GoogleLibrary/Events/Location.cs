namespace GoogleLibrary.Events
{
    public class Location
    {
        public Location(string? shortName, string? address = null)
        {
            ShortName = shortName == null ? "" : shortName.Replace("/", " ").Trim();
            Address = address == null ? "" : address.Replace("/", " ").Trim();

            if (ShortName == "")
                ShortName = Address;
            if (Address == "")
                Address = ShortName;

            if (ShortName.ToLowerInvariant() == "home")
            {
                ShortName = "Home";
                Address = "10 Bourne Close, Beeston";
            }
        }

        public string Address { get; set; }

        public string ShortName { get; set; }
    }
}