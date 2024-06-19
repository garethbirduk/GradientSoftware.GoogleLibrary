using GoogleServices.CustomServices;
using GoogleServices.GoogleAuthentication;
using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestRoundTripSheetsCalendar : GoogleAuthenticatedUnitTest
    {
        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarService, GoogleCalendarsService, GoogleCalendarReadonlyService,
                GoogleSpreadsheetService, GoogleSpreadsheetReadonlyService);
        }

        [TestMethod]
        public async Task TestRoundTrip()
        {
            var calendarName = TestHelpers.RandomCalendarName();
            var worksheetName = "ExampleStartEndTimes";

            CalendarId = GoogleCalendarsService.CreateCalendar(calendarName).Id;
            var customSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
            var customCalendarEventsService = new CustomCalendarService(GoogleCalendarReadonlyService, GoogleSpreadsheetService);

            try
            {
                await customSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, worksheetName, CalendarId, maxEvents: 2);
                await customCalendarEventsService.CalendarToWorksheetAsync(CalendarId, SpreadsheetId, calendarName);
            }
            finally
            {
                await GoogleCalendarsService.DeleteCalendarAsync(CalendarId);
                await GoogleSpreadsheetService.DeleteWorksheetsAsync(SpreadsheetId, calendarName);
            }
        }
    }
}