using GoogleServices.GoogleServices;

namespace GoogleServices.Test
{
    /// <summary>
    /// Single shared calendar created once per test runner session. Stable-named so subsequent runs
    /// reuse it via <see cref="GoogleCalendarsService.CreateOrGetCalendarAsync"/> — zero create-quota
    /// cost after the first run, ever.
    ///
    /// Contracts for tests that use <see cref="CalendarId"/>:
    ///   - Don't assert pre-conditions you didn't establish (e.g. "description is null").
    ///   - Restore anything you mutate (try-finally for property changes).
    ///   - Events are fine if [TestCleanup] wipes them between tests.
    ///
    /// Tests that mutate the calendar's *name* must use their own throwaway calendar — a partial
    /// failure during rename would orphan the shared one.
    /// </summary>
    [TestClass]
    public static class TestSessionFixture
    {
        public const string SessionCalendarName = "_test_session_googlelibrary";

        public static string CalendarId { get; private set; } = "";
        public static GoogleCalendarsService GoogleCalendarsService { get; } = new();

        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext context)
        {
            GoogleCalendarsService.Initialize();
            try
            {
                CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(SessionCalendarName)).Id;
            }
            catch (Exception ex)
            {
                // Tolerate AssemblyInitialize failure (typically: Google's calendar-create quota
                // hit). Tests that need the shared calendar will fail individually with a clearer
                // error than "all tests aborted". The manual cleanup test in particular needs to
                // be runnable even when this fails.
                Console.Error.WriteLine($"[TestSessionFixture] Could not create/get session calendar: {ex.Message}");
            }
        }
    }
}
