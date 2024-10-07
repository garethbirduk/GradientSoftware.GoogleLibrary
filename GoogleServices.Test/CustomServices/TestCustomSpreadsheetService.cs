using GoogleServices.CustomServices;
using GoogleServices.GoogleServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestCustomSpreadsheetService
    {
        protected string CalendarId { get; set; }
        protected string SpreadsheetId { get; set; } = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";

        [TestMethod]
        public async Task TestBlackpool2024a()
        {
            var googleSpreadsheetReadonlyService = new GoogleSpreadsheetReadonlyService();
            var googleCalendarService = new GoogleCalendarService();
            var googleCalendarsService = new GoogleCalendarsService();
            var customSpreadsheetService = new CustomSpreadsheetService(googleSpreadsheetReadonlyService, googleCalendarService);

            CalendarId = (await googleCalendarsService.CreateOrGetCalendarAsync("Blackpool2024a", true)).Id;
            await customSpreadsheetService.WorksheetToCalendarAsync("1xXeM_uG2_z1dH9dAfSNZntxNfadmaWo6K9QrFHRURZg", "Blackpool2024a", CalendarId, headerRowsCount: 2);
        }

        [TestMethod]
        public async Task TestBolivia2024a()
        {
            var googleSpreadsheetReadonlyService = new GoogleSpreadsheetReadonlyService();
            var googleCalendarService = new GoogleCalendarService();
            var googleCalendarsService = new GoogleCalendarsService();
            var customSpreadsheetService = new CustomSpreadsheetService(googleSpreadsheetReadonlyService, googleCalendarService);

            CalendarId = (await googleCalendarsService.CreateOrGetCalendarAsync("Bolivia2024a", true)).Id;
            await customSpreadsheetService.WorksheetToCalendarAsync("1xXeM_uG2_z1dH9dAfSNZntxNfadmaWo6K9QrFHRURZg", "Bolivia2024a", CalendarId, headerRowsCount: 2);
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

        //[TestInitialize]
        //public async Task TestInitialize()
        //{
        //    CustomSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
        //}
    }
}