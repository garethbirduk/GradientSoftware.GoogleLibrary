using GoogleServices.CustomServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestCustomSpreadsheetService
    {
        protected string TestSpreadsheetId { get; set; } = "1xXeM_uG2_z1dH9dAfSNZntxNfadmaWo6K9QrFHRURZg";

        public CustomSpreadsheetService CustomSpreadsheetService { get; set; } = new();

        [TestMethod]
        public async Task Formula1()
        {
            var name = "Formula 1";
            await CustomSpreadsheetService.WorksheetToCalendarAsync(TestSpreadsheetId, name, headerRowsCount: 1);
        }

        [TestMethod]
        public async Task TestCrewe()
        {
            var name = "Crewe Alex";
            await CustomSpreadsheetService.WorksheetToCalendarAsync(TestSpreadsheetId, name, headerRowsCount: 1);
        }

        [TestMethod]
        public async Task TestHoliday()
        {
            var name = "Holiday";
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
    }
}