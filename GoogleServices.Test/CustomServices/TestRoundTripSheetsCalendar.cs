using GoogleServices.CustomServices;
using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestRoundTripSheetsCalendar : GoogleAuthenticatedUnitTest
    {
        [TestMethod]
        public async Task TestRoundTrip()
        {
            var calendarName = TestHelpers.RandomCalendarName();
            var worksheetName = "ExampleAllDay";

            CalendarId = GoogleCalendarsService.CreateCalendar(calendarName).Id;
            var customSpreadsheetService = new CustomSpreadsheetService();
            var customCalendarEventsService = new CustomCalendarService();

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
    }
}