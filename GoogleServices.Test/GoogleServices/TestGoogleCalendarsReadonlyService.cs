using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    /// <summary>
    /// Tests against the shared session calendar — see <see cref="TestSessionFixture"/>.
    /// Read-only; safe to share with other test classes.
    /// </summary>
    [TestClass]
    public class TestGoogleCalendarsReadonlyService
    {
        private static GoogleCalendarsReadonlyService GoogleCalendarsReadonlyService = new();
        private static string CalendarId => TestSessionFixture.CalendarId;
        private static string CalendarSummary => TestSessionFixture.SessionCalendarName;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GoogleCalendarsReadonlyService = new GoogleCalendarsReadonlyService();
            GoogleCalendarsReadonlyService.Initialize();
        }

        [TestMethod]
        public void TestGetCalendar()
        {
            var calendar = GoogleCalendarsReadonlyService.GetCalendar(CalendarId);
            Assert.IsNotNull(calendar);
            Assert.AreEqual(CalendarSummary, calendar.Summary);
        }

        [TestMethod]
        public void TestGetCalendarBySummary()
        {
            var calendar = GoogleCalendarsReadonlyService.GetCalendarBySummary(CalendarSummary);
            Assert.IsNotNull(calendar);
            Assert.AreEqual(CalendarSummary, calendar.Summary);
        }

        [TestMethod]
        public void TestGetCalendars()
        {
            var calendars = GoogleCalendarsReadonlyService.GetCalendars();
            Assert.IsTrue(calendars.Items.Count > 0);
            Assert.AreEqual(1, calendars.Items.Count(x => x.Id == CalendarId));
        }

        [TestMethod]
        public void TestGetCalendars_WithPredicate()
        {
            var calendars = GoogleCalendarsReadonlyService.GetCalendars(x => x.Id == CalendarId);
            Assert.AreEqual(1, calendars.Items.Count);
            Assert.AreEqual(CalendarSummary, calendars.Items.Single().Summary);
        }
    }
}
