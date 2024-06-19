using GoogleServices.GoogleAuthentication;
using Gradient.Utils;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarService : GoogleAuthenticatedUnitTest
    {
        private readonly string _calendarName1 = TestHelpers.RandomCalendarName();

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>(
                GoogleCalendarService, GoogleCalendarsService);
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(_calendarName1)).Id;
        }

        [TestMethod]
        public void TestRenameCalendar()
        {
            try
            {
                var calendarName2 = TestHelpers.RandomCalendarName();
                GoogleCalendarService.RenameCalendar(CalendarId, calendarName2);
                Assert.IsNull(GoogleCalendarsService.GetCalendarBySummary(_calendarName1));
                Assert.IsNotNull(GoogleCalendarsService.GetCalendarBySummary(calendarName2));
            }
            finally
            {
                // restore name for TestCleanup purposes
                GoogleCalendarService.RenameCalendar(CalendarId, _calendarName1);
            }
        }

        [TestMethod]
        public void TestSetDescription()
        {
            Assert.IsNull(GoogleCalendarService.Calendar(CalendarId).Description);
            var description = StringHelpers.RandomName(prefix: "_deleteme_");
            GoogleCalendarService.SetDescription(CalendarId, description);
            Assert.AreEqual(description, GoogleCalendarService.Calendar(CalendarId).Description);
        }
    }
}