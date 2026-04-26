using Google.Apis.PeopleService.v1.Data;
using Gradient.Utils;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GoogleLibrary.GoogleContacts
{
    public static class AddressStandardizationExtensions
    {
        private static List<string> RemoveSpecialString(List<string> list, string search, string replace = " ")
        {
            list = list.Select(x => x.Replace(search, replace))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
            return list;
        }

        private static List<string> StandardizeCommonNames(List<string> list)
        {
            // St could be St John St i.e. Saint John Street
            // Only replace ending St
            var commonEndings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "St", "Street" },
            };

            var commonAnywhere = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Ave", "Avenue" },
                { "Av", "Avenue" },
                { "Blvd", "Boulevard" },
                { "Bvd", "Boulevard" },
                { "Cl", "Close" },
                { "Cr", "Crescent" },
                { "Cres", "Crescent" },
                { "Ct", "Court" },
                { "Crt", "Court" },
                { "Dr", "Drive" },
                { "Expy", "Expressway" },
                { "Gdn", "Garden" },
                { "Gdns", "Gardens" },
                { "Gr", "Green" },
                { "Gv", "Grove" },
                { "Hwy", "Highway" },
                { "La", "Lane" },
                { "Ln", "Lane" },
                { "Mdw", "Meadow" },
                { "Pk", "Park" },
                { "Pkwy", "Parkway" },
                { "Pl", "Place" },
                { "Rd", "Road" },
                { "Sq", "Square" },
                { "Tce", "Terrace" },
                { "Trl", "Trail" },
                { "Vw", "View" },
                { "Wk", "Walk" },
                { "Wy", "Way" }
            };

            for (var index = 0; index < list.Count; index++)
            {
                var addressParts = list[index];
                var split = addressParts.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

                var newAddress = new List<string>();

                for (int i = 0; i < split.Count; i++)
                {
                    var item = split[i];
                    var newItem = item;

                    // Replace common abbreviations anywhere in the string
                    foreach (var pair in commonAnywhere)
                    {
                        if (item.Equals(pair.Key, StringComparison.InvariantCultureIgnoreCase) ||
                            item.Equals($"{pair.Key}.", StringComparison.InvariantCultureIgnoreCase))
                        {
                            newItem = pair.Value;
                        }
                    }

                    // Special handling for the last word only
                    if (i == split.Count - 1) // Ensure it's the last word in the address
                    {
                        foreach (var pair in commonEndings)
                        {
                            if (item.Equals(pair.Key, StringComparison.InvariantCultureIgnoreCase) ||
                                item.Equals($"{pair.Key}.", StringComparison.InvariantCultureIgnoreCase))
                            {
                                newItem = pair.Value;
                            }
                        }
                    }

                    newAddress.Add(newItem);
                }

                list[index] = string.Join(" ", newAddress);
            }

            return list;
        }

        public static string ToCommonLowerCase(this string s)
        {
            var common = new List<string>()
            {
                "the",
                "on",
                "of",
                "upon",
                "by",
                "over",
                "le",
                "la",
                "du",
                "des",
                "del",
            };

            var list = s
                .Split(" ");

            // skip first, then any more that are just numbers
            var skip = 0;

            while (list.Length > skip)
            {
                if (int.TryParse(list[skip], out _))
                {
                    skip++;
                }
                else
                {
                    break;
                }
            }

            for (var index = skip + 1; index < list.Count(); index++)
            {
                foreach (var item in common)
                {
                    if (list[index].Equals(item, StringComparison.InvariantCultureIgnoreCase))
                        list[index] = item;
                }
            }

            return string.Join(" ", list);
        }

        public static string TrimDelimitedAddress(this string s, string delimiter, AddressStandardizationOptions standardizationOptions)
        {
            var list = s
                .Split(delimiter)
                .Select(x => x.TrimDelimitedWhitespace(delimiter))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.TrimTrailingPunctuation))
            {
                list = list
                    .Select(x => x.TrimDelimitedTrailingPunctuation(delimiter))
                    .Select(x => x.TrimDelimitedWhitespace(delimiter))
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.RemoveHyphens))
            {
                list = RemoveSpecialString(list, "-");
            }

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.RemoveUnderscores))
            {
                list = RemoveSpecialString(list, "_");
            }

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.TitleCase))
            {
                list = list
                    .Select(x => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(x.ToLower()))
                    .Select(x => x.ToCommonLowerCase())
                    .ToList();
            }

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.StandardizeCommonNames))
            {
                list = StandardizeCommonNames(list);
            }

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.TrimRepetative))
            {
                list = list
                    .Distinct()
                    .ToList();
            }

            if (standardizationOptions.HasFlag(AddressStandardizationOptions.IncludeSpaceAfterDelimiter))
            {
                delimiter = $"{delimiter} ";
            }

            return string.Join(delimiter, list);
        }

        public static string TrimDelimitedTrailingPunctuation(this string s, string delimiter, List<string>? punctuation = null)
        {
            if (punctuation == null)
            {
                punctuation = new List<string>()
                {
                    ".",
                    ".",
                    "!",
                    "\"",
                    "`",
                    "'",
                    "~",
                    "_",
                    "-",
                    "+",
                    "=",
                    ":",
                    ";",
                    "@",
                    "#",
                    "<",
                    ">",
                    "",
                };
            }

            var list = s.Split(delimiter);

            foreach (var p in punctuation)
            {
                list = list.Select(x => x.TrimEnd(p)).ToArray();
            }

            return string.Join(delimiter, list);
        }

        public static string TrimDelimitedWhitespace(this string s, string delimiter)
        {
            var list = s
                .Split(delimiter)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            return string.Join(delimiter, list);
        }
    }

    public class AddressStandardizer
    {
        private readonly AddressStandardizationOptions _options;

        private static void IdentifyProperties(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.ExtendedAddress)
                && !string.IsNullOrWhiteSpace(address.City)
                && address.City.Split(',').Count() > 1)
            {
                var split = address.City
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());

                address.ExtendedAddress = string.Join(", ", split.Take(split.Count() - 1));
                address.City = split.Last();
            }
            else if (string.IsNullOrWhiteSpace(address.City)
                && !string.IsNullOrWhiteSpace(address.ExtendedAddress)
                && address.ExtendedAddress.Split(',').Count() > 1)
            {
                var split = address.ExtendedAddress
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());

                address.ExtendedAddress = string.Join(", ", split.Take(split.Count() - 1));
                address.City = split.Last();
            }

            if (!string.IsNullOrWhiteSpace(address.ExtendedAddress)
                && string.IsNullOrWhiteSpace(address.City))
            {
                address.City = address.ExtendedAddress;
            }

            if (address.ExtendedAddress == address.City)
            {
                address.ExtendedAddress = "";
            }
        }

        /// <summary>
        /// Trim trailing whitespace, commas, periods, or other punctuation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string TrimTrailingPunctuation(string input)
        {
            return Regex.Replace(input, @"[\search,.!?;:]+$", "");
        }

        private static string TrimWhitespaceAndTrailingPunctuation(string streetAddress)
        {
            streetAddress = string.Join(", ",
                streetAddress
                    .Split(',')
                    .Select(x => x.Trim())
                    .Select(x => TrimTrailingPunctuation(x))
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                );
            return streetAddress;
        }

        private string ApplyTitleCase(string input) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input.ToLower());

        private string CapitalizeAbbreviations(string input)
        {
            var abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "uk", "UK" },
            { "usa", "USA" }
        };

            foreach (var pair in abbreviations)
            {
                input = Regex.Replace(input, $@"\b{Regex.Escape(pair.Key)}\b", pair.Value, RegexOptions.IgnoreCase);
            }

            return input;
        }

        private string NormalizeWhitespace(string input) => Regex.Replace(input, @"\search+", " ");

        private string RemoveWhitespaceAroundHyphens(string input) => Regex.Replace(input, @"\search*-\search*", "-");

        private string ReplaceUnderscoresWithHyphens(string input) => input.Replace("_", "-");

        // Standardization logic for City
        private string StandardizeCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return city;

            if (_options.HasFlag(AddressStandardizationOptions.TrimWhitespace))
            {
                city = TrimWhitespaceAndTrailingPunctuation(city);
            }

            if (_options.HasFlag(AddressStandardizationOptions.NormalizeWhitespace))
            {
                city = NormalizeWhitespace(city);
            }

            if (_options.HasFlag(AddressStandardizationOptions.TitleCase))
            {
                city = ApplyTitleCase(city);
            }

            return city;
        }

        private string StandardizeCommonNames(string input)
        {
            var list = input.Split(',').Select(x => x.Trim()).ToList();

            var commonNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "St", "Street" },
                { "Ave", "Avenue" },
                { "Rd", "Road" },
                { "Blvd", "Boulevard" },
                { "Dr", "Drive" },
                { "Cl", "Close" },
                { "Cr", "Crescent" }
            };

            for (var index = 0; index < list.Count(); index++)
            {
                foreach (var pair in commonNames)
                {
                    if (list[index].EndsWith(pair.Key, StringComparison.InvariantCultureIgnoreCase)
                        || list[index].EndsWith($"{pair.Key}.", StringComparison.InvariantCultureIgnoreCase))
                        list[index] = list[index].ReplaceLast(pair.Key, pair.Value, StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return string.Join(", ", list);
        }

        // Standardization logic for Country
        private string StandardizeCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country)) return country;

            if (_options.HasFlag(AddressStandardizationOptions.TrimWhitespace))
            {
                country = TrimWhitespaceAndTrailingPunctuation(country);
            }

            if (_options.HasFlag(AddressStandardizationOptions.NormalizeWhitespace))
            {
                country = NormalizeWhitespace(country);
            }

            if (_options.HasFlag(AddressStandardizationOptions.CapitalizeAbbreviations))
            {
                country = CapitalizeAbbreviations(country);
            }

            return country;
        }

        // Standardization logic for PostalCode
        private string StandardizePostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode)) return postalCode;

            if (_options.HasFlag(AddressStandardizationOptions.TrimWhitespace))
            {
                postalCode = TrimWhitespaceAndTrailingPunctuation(postalCode);
            }

            return postalCode; // Postal code rules can be expanded for country-specific formats
        }

        // Standardization logic for Region
        private string StandardizeRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region)) return region;

            if (_options.HasFlag(AddressStandardizationOptions.TrimWhitespace))
            {
                region = TrimWhitespaceAndTrailingPunctuation(region);
            }

            if (_options.HasFlag(AddressStandardizationOptions.NormalizeWhitespace))
            {
                region = NormalizeWhitespace(region);
            }

            if (_options.HasFlag(AddressStandardizationOptions.TitleCase))
            {
                region = ApplyTitleCase(region);
            }

            return region;
        }

        // Standardization logic for StreetAddress
        internal string StandardizeStreetAddress(string streetAddress)
        {
            if (string.IsNullOrWhiteSpace(streetAddress))
                return "";

            streetAddress = streetAddress.TrimDelimitedAddress(",", _options);

            return streetAddress;
        }

        public AddressStandardizer(AddressStandardizationOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Main method to standardize an Address object
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Address Standardize(Address address)
        {
            IdentifyProperties(address);

            address.StreetAddress = StandardizeStreetAddress(address.StreetAddress);
            address.City = StandardizeCity(address.City);
            address.Region = StandardizeRegion(address.Region);
            address.PostalCode = StandardizePostalCode(address.PostalCode);
            address.Country = StandardizeCountry(address.Country);

            return address;
        }
    }
}