using Google.Apis.PeopleService.v1.Data;

namespace GoogleServices.Test.Helpers
{
    /// <summary>
    /// Test-only helpers for merging multiple Address snapshots with formattedValue correction.
    /// Used by contact-deduplication tests; not production code.
    /// </summary>
    internal static class AddressMergeHelpers
    {
        public static string ConstructFormattedValue(Address address)
        {
            var components = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.StreetAddress)) components.Add(address.StreetAddress);
            if (!string.IsNullOrWhiteSpace(address.City)) components.Add(address.City);
            if (!string.IsNullOrWhiteSpace(address.Region)) components.Add(address.Region);
            if (!string.IsNullOrWhiteSpace(address.PostalCode)) components.Add(address.PostalCode);
            if (!string.IsNullOrWhiteSpace(address.Country)) components.Add(address.Country);
            return string.Join(", ", components);
        }

        public static Address MergeAddresses(params Address[] addresses)
        {
            var merged = new Address();

            foreach (var address in addresses)
            {
                if (!string.IsNullOrWhiteSpace(address.StreetAddress)) merged.StreetAddress = address.StreetAddress;
                if (!string.IsNullOrWhiteSpace(address.City)) merged.City = address.City;
                if (!string.IsNullOrWhiteSpace(address.Region)) merged.Region = address.Region;
                if (!string.IsNullOrWhiteSpace(address.PostalCode)) merged.PostalCode = address.PostalCode;
                if (!string.IsNullOrWhiteSpace(address.Country)) merged.Country = address.Country;
                if (!string.IsNullOrWhiteSpace(address.CountryCode)) merged.CountryCode = address.CountryCode;

                if (!string.IsNullOrWhiteSpace(address.FormattedValue))
                {
                    var components = address.FormattedValue.Split(',').Select(c => c.Trim()).ToArray();

                    if (string.IsNullOrWhiteSpace(merged.StreetAddress) && components.Length > 0)
                        merged.StreetAddress = components[0];
                    if (string.IsNullOrWhiteSpace(merged.City) && components.Length > 1)
                        merged.City = components[1];
                    if (string.IsNullOrWhiteSpace(merged.Region) && components.Length > 2)
                        merged.Region = components[2];
                    if (string.IsNullOrWhiteSpace(merged.Country) && components.Length > 3)
                        merged.Country = components[3];
                }

                // Region containing country (e.g. "Gwynedd, UK"): split off the trailing country.
                if (!string.IsNullOrWhiteSpace(merged.Region) && merged.Region.Contains(','))
                {
                    var regionParts = merged.Region.Split(',').Select(r => r.Trim()).ToArray();
                    merged.Region = regionParts[0];
                    if (string.IsNullOrWhiteSpace(merged.Country) && regionParts.Length > 1)
                        merged.Country = regionParts[1];
                }
            }

            merged.FormattedValue = ConstructFormattedValue(merged);
            return merged;
        }

        public static string NormalizeString(string value)
            => string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToUpper();
    }
}
