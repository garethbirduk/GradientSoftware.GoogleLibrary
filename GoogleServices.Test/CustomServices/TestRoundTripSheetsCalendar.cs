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
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>(
                GoogleCalendarService, GoogleCalendarsService, GoogleCalendarReadonlyService,
                GoogleSpreadsheetService, GoogleSpreadsheetReadonlyService);
        }

        [TestMethod]
        public async Task TestRoundTrip()
        {
            var calendarName = TestHelpers.RandomCalendarName();
            var worksheetName = "ExampleAllDay";

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
                await GoogleSpreadsheetService.DeleteWorksheetsAsync(SpreadsheetId, calendarName);
            }
        }

        //[TestMethod]
        //public async Task TestSummer2024()
        //{
        //    var calendarName = "Summer2024";
        //    var worksheetName = "Summer2024";

        //    CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(calendarName)).Id;
        //    var customSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
        //    var customCalendarEventsService = new CustomCalendarService(GoogleCalendarReadonlyService, GoogleSpreadsheetService);

        //    try
        //    {
        //        await customSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, worksheetName, CalendarId, headerRowsCount: 2);
        //        //await customCalendarEventsService.CalendarToWorksheetAsync(CalendarId, SpreadsheetId, calendarName);
        //    }
        //    finally
        //    {
        //        //await GoogleSpreadshee tService.DeleteWorksheetsAsync(SpreadsheetId, calendarName);
        //    }
        //}
    }
}