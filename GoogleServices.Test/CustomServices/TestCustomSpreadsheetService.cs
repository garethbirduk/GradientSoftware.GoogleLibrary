using GoogleServices.CustomServices;
using GoogleServices.GoogleAuthentication;
using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestCustomSpreadsheetService : GoogleAuthenticatedUnitTest
    {
        [TestMethod]
        public async Task TestCreateExample2Hour()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(TestHelpers.RandomCalendarName())).Id;
            await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "Example2Hour", CalendarId);
        }

        [TestMethod]
        public async Task TestCreateExampleAllDay()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(TestHelpers.RandomCalendarName())).Id;
            await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "ExampleAllDay", CalendarId);
        }

        [TestMethod]
        public async Task TestCreateItineraryEvents()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(TestHelpers.RandomCalendarName())).Id;
            await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "ExampleItinerary", CalendarId, headerRowsCount: 1);
        }

        [TestMethod]
        public async Task TestCrewe()
        {
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync("Crewe Alex", true)).Id;
            await CustomSpreadsheetService.WorksheetToCalendarAsync("1xXeM_uG2_z1dH9dAfSNZntxNfadmaWo6K9QrFHRURZg", "Crewe Alex", CalendarId, headerRowsCount: 1);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>(
                GoogleSpreadsheetReadonlyService, GoogleCalendarService, GoogleCalendarsService);
            CustomSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
        }
    }
}