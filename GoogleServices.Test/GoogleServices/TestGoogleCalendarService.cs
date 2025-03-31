using GoogleServices.GoogleServices;
using Gradient.Utils;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarService
    {
        private static readonly string _calendarName1 = TestHelpers.RandomCalendarName();

        private static GoogleCalendarService GoogleCalendarService = new();
        private static GoogleCalendarsService GoogleCalendarsService = new();

        public static string CalendarId { get; private set; } = "";

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            await GoogleCalendarsService.DeleteCalendarAsync(CalendarId);
        }

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext context)
        {
            GoogleCalendarService = new GoogleCalendarService();
            GoogleCalendarService.Initialize();
            GoogleCalendarsService = new GoogleCalendarsService();
            GoogleCalendarsService.Initialize();
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