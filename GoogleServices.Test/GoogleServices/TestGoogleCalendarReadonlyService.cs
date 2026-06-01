using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    /// <summary>
    /// Tests against the shared session calendar — see <see cref="TestSessionFixture"/>.
    /// </summary>
    [TestClass]
    public class TestGoogleCalendarReadonlyService
    {
        private static GoogleCalendarReadonlyService GoogleCalendarReadonlyService = new();
        private static string CalendarId => TestSessionFixture.CalendarId;
        private static string CalendarSummary => TestSessionFixture.SessionCalendarName;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GoogleCalendarReadonlyService = new GoogleCalendarReadonlyService();
            GoogleCalendarReadonlyService.Initialize();
        }

        [TestMethod]
        public void TestCalendarSummary()
        {
            Assert.AreEqual(CalendarSummary, GoogleCalendarReadonlyService.Calendar(CalendarId).Summary);
        }
    }
}
