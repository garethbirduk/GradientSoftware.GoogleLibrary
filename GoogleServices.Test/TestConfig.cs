namespace GoogleServices.Test
{
    /// <summary>
    /// Test configuration. The spreadsheet ID comes from an environment variable when set;
    /// otherwise falls back to the hardcoded personal-account default below.
    ///
    /// To run against a CI- or project-owned sheet later, set the env var instead of touching code:
    ///   GOOGLE_TEST_SPREADSHEET_ID
    ///
    /// Service-account auth (for scope-correctness tests / CI):
    ///   GOOGLE_APPLICATION_CREDENTIALS  — standard ADC env var, path to service-account JSON
    /// </summary>
    internal static class TestConfig
    {
        // TODO: drop personal-account fallback once a CI- or project-owned sheet exists.
        private const string DefaultSpreadsheetId = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";

        /// <summary>The single test fixture sheet — stable tabs (e.g. "TestGetData") plus a sandbox area for mutating tests.</summary>
        public static string SpreadsheetId =>
            Environment.GetEnvironmentVariable("GOOGLE_TEST_SPREADSHEET_ID") ?? DefaultSpreadsheetId;

        /// <summary>Path to a service-account JSON key, if set. Used by scope-correctness tests.</summary>
        public static string? ServiceAccountKeyPath =>
            Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
    }
}
