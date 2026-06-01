using GoogleServices.GoogleServices;
using Gradient.Utils;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarService
    {
        private static GoogleCalendarService GoogleCalendarService = new();
        private static GoogleCalendarsService GoogleCalendarsService = new();

        private static string SharedCalendarId => TestSessionFixture.CalendarId;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GoogleCalendarService = new GoogleCalendarService();
            GoogleCalendarService.Initialize();
            GoogleCalendarsService = new GoogleCalendarsService();
            GoogleCalendarsService.Initialize();
        }

        /// <summary>
        /// Rename mutates the calendar's canonical name — if a partial failure leaves the wrong
        /// name in place, lookups by name fail. So this test always uses its own throwaway calendar.
        /// </summary>
        [TestMethod]
        public async Task TestRenameCalendar()
        {
            var name1 = TestHelpers.RandomCalendarName();
            var calendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(name1)).Id;
            try
            {
                var name2 = TestHelpers.RandomCalendarName();
                GoogleCalendarService.RenameCalendar(calendarId, name2);
                Assert.IsNull(GoogleCalendarsService.GetCalendarBySummary(name1));
                Assert.IsNotNull(GoogleCalendarsService.GetCalendarBySummary(name2));
            }
            finally
            {
                try { await GoogleCalendarsService.DeleteCalendarAsync(calendarId); }
                catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound) { }
            }
        }

        /// <summary>
        /// Uses the shared session calendar. Doesn't assert on starting state (could be a description
        /// from a previous run). Best-effort restore in finally.
        /// </summary>
        [TestMethod]
        public void TestSetDescription()
        {
            var description = StringHelpers.RandomName(prefix: "_deleteme_");
            try
            {
                GoogleCalendarService.SetDescription(SharedCalendarId, description);
                Assert.AreEqual(description, GoogleCalendarService.Calendar(SharedCalendarId).Description);
            }
            finally
            {
                try { GoogleCalendarService.SetDescription(SharedCalendarId, ""); } catch { }
            }
        }
    }
}
