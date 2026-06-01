using GoogleServices.GoogleServices;

namespace GoogleServices.Test
{
    /// <summary>
    /// Manual maintenance tasks gated by [TestCategory("Manual")] — skipped by default test runs
    /// (see test.runsettings). Invoke explicitly:
    ///   dotnet test /TestCaseFilter:"TestCategory=Manual"
    /// </summary>
    [TestClass]
    public class ManualMaintenance
    {
        /// <summary>
        /// Deletes every calendar on the running identity whose summary starts with one of the
        /// known test prefixes. Useful when iteration on the test suite leaves orphans behind
        /// (e.g. crashed runs, predicate-delete races) and the SA's calendar count grows.
        /// </summary>
        [TestMethod]
        [TestCategory("Manual")]
        public async Task DeleteAllTestCalendars()
        {
            var service = new GoogleCalendarsService();
            service.Initialize();

            string[] prefixes = ["_test_", "_deleteme_", "_predtest_"];

            var calendars = service.GetCalendars(c =>
                c.Summary != null && prefixes.Any(p => c.Summary.StartsWith(p)));

            var deleted = 0;
            var failed = 0;
            foreach (var calendar in calendars.Items)
            {
                try { await service.DeleteCalendarAsync(calendar.Id); deleted++; }
                catch { failed++; }
            }

            Console.WriteLine($"Deleted {deleted} test calendars, {failed} failed.");
        }
    }
}
