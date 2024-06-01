using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.IntegrationTest.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarService : GoogleAuthenticatedUnitTest
    {
        private string CalendarName1 = Utils.RandomName(prefix: "_deleteme_");

        [TestCleanup]
        public async Task TestCleanup()
        {
            await GoogleCalendarsService.DeleteCalendarAsync(CalendarId);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarService, GoogleCalendarsService);
        }

        [TestMethod]
        public async Task TestRenameCalendar()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(CalendarName1)).Id;

            var calendarName2 = Utils.RandomName(prefix: "_deleteme_");
            GoogleCalendarService.RenameCalendar(CalendarId, calendarName2);
            Assert.IsNull(GoogleCalendarsService.GetCalendarBySummary(CalendarName1));
            Assert.IsNotNull(GoogleCalendarsService.GetCalendarBySummary(calendarName2));

            // restoe CalendarName1 for TestCleanup purposes
            CalendarName1 = calendarName2;
        }

        [TestMethod]
        public async Task TestSetDescription()
        {
            CalendarName1 = Utils.RandomName(prefix: "_deleteme_");
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(CalendarName1)).Id;

            Assert.IsNull(GoogleCalendarService.Calendar(CalendarId).Description);
            var description = Utils.RandomName(prefix: "_deleteme_");
            GoogleCalendarService.SetDescription(CalendarId, description);
            Assert.AreEqual(description, GoogleCalendarService.Calendar(CalendarId).Description);
        }
    }
}