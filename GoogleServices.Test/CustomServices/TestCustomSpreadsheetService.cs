using GoogleServices.CustomServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestCustomSpreadsheetService
    {
        protected string TestSpreadsheetId { get; set; } = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";

        public CustomSpreadsheetService CustomSpreadsheetService { get; set; } = new();

        [TestMethod]
        public async Task TestBlackpool2025a()
        {
            var name = "Blackpool2025a";
            await CustomSpreadsheetService.WorksheetToCalendarAsync(TestSpreadsheetId, name, headerRowsCount: 2);
        }

        //[TestMethod]
        //public async Task TestCreateExample2Hour()
        //{
        //    CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(TestHelpers.RandomCalendarName())).Id;
        //    await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "Example2Hour", CalendarId);
        //}

        //[TestMethod]
        //public async Task TestCreateExampleAllDay()
        //{
        //    CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(TestHelpers.RandomCalendarName())).Id;
        //    await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "ExampleAllDay", CalendarId);
        //}

        //[TestMethod]
        //public async Task TestCreateItineraryEvents()
        //{
        //    CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(TestHelpers.RandomCalendarName())).Id;
        //    await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, "ExampleItinerary", CalendarId, headerRowsCount: 1);
        //}

        //[TestMethod]
        //public async Task TestCrewe()
        //{
        //    CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync("Crewe Alex", true)).Id;
        //    await CustomSpreadsheetService.WorksheetToCalendarAsync("1xXeM_uG2_z1dH9dAfSNZntxNfadmaWo6K9QrFHRURZg", "Crewe Alex", CalendarId, headerRowsCount: 1);
        //}
    }
}