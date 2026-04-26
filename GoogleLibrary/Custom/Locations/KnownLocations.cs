namespace GoogleLibrary.Custom.Locations
{
    /// <summary>
    /// Registry of named locations whose addresses should be resolved automatically when a Location is constructed
    /// with that name as its ShortName. Names are matched case-insensitively. The registered name is treated as
    /// canonical and overwrites the lookup name's casing.
    ///
    /// Loaded automatically from environment variables matching the prefix "KNOWN_LOCATION_" — for example
    /// KNOWN_LOCATION_HOME="10 Some Street, Town, Postcode, Country" registers "HOME". Programmatic registration
    /// via Register() takes precedence and lets consumers wire any configuration source they choose.
    /// </summary>
    public static class KnownLocations
    {
        private const string EnvPrefix = "KNOWN_LOCATION_";

        private static readonly Dictionary<string, (string Name, string Address)> _entries
            = new(StringComparer.OrdinalIgnoreCase);

        static KnownLocations()
        {
            foreach (System.Collections.DictionaryEntry envVar in Environment.GetEnvironmentVariables())
            {
                var key = envVar.Key?.ToString();
                if (key == null || !key.StartsWith(EnvPrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                var name = key.Substring(EnvPrefix.Length);
                var value = envVar.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
                    _entries[name] = (name, value);
            }
        }

        /// <summary>
        /// Forget all registered entries. Intended for tests.
        /// </summary>
        public static void Clear() => _entries.Clear();

        /// <summary>
        /// Register or overwrite a named location's address.
        /// </summary>
        public static void Register(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
                return;
            _entries[name] = (name, address);
        }

        /// <summary>
        /// Resolve a name (case-insensitive) to its canonical name and address, if registered.
        /// </summary>
        public static bool TryGet(string lookupName, out string canonicalName, out string address)
        {
            if (!string.IsNullOrWhiteSpace(lookupName)
                && _entries.TryGetValue(lookupName, out var entry))
            {
                canonicalName = entry.Name;
                address = entry.Address;
                return true;
            }
            canonicalName = "";
            address = "";
            return false;
        }
    }
}
