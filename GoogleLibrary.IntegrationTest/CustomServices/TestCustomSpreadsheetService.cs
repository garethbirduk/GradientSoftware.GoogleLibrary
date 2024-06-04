using GoogleLibrary.EventsServices;
using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.IntegrationTest.CustomServices
{
    [TestClass]
    public class TestCustomSpreadsheetService : GoogleAuthenticatedUnitTest
    {
        [TestCleanup]
        public async Task TestCleanup()
        {
            await GoogleCalendarsService.DeleteCalendarAsync(CalendarId);
        }

        [TestMethod]
        public async Task TestCreateItineraryEvents()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(Utils.RandomName())).Id;
            await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "ExampleItinerary", CalendarId, headerRowsCount: 2);
        }

        [TestMethod]
        public async Task TestCreateTest2()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(Utils.RandomName())).Id;
            await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "ExampleSimple", CalendarId);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleSpreadsheetReadonlyService, GoogleCalendarService, GoogleCalendarsService);
            CustomSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
        }
    }
}