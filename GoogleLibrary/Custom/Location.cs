namespace GoogleLibrary.Custom
{
    public class Location
    {
        public Location(string shortName, string address)
        {
            ShortName = shortName == null ? "" : shortName.Replace("/", " ");
            Address = address == null ? "" : address.Replace("/", " ");

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