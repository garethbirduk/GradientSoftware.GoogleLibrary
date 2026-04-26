using GoogleServices.CustomServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestRoundTripSheetsCalendar
    {
        private readonly string SpreadsheetId = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";
        public CustomSpreadsheetService CustomSpreadsheetService { get; set; } = new();

        [TestMethod]
        public async Task TestRoundTrip()
        {
            var calendarName = TestHelpers.RandomCalendarName();
            var worksheetName = "ExampleAllDay";

            var calendarId = (await CustomSpreadsheetService.GoogleCalendarsService.CreateOrGetCalendarAsync(calendarName)).Id;
            var customSpreadsheetService = new CustomSpreadsheetService();
            var customCalendarEventsService = new CustomCalendarService();

            try
            {
                await customSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, worksheetName, calendarId, maxEvents: 2);
                await customSpreadsheetService.CalendarToWorksheetAsync(calendarId, SpreadsheetId, calendarName);
            }
            finally
            {
                await customSpreadsheetService.GoogleSpreadsheetService.DeleteWorksheetsAsync(SpreadsheetId, calendarName);
            }
        }
    }
}