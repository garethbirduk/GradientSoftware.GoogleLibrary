//using GoogleLibrary.CustomServices;
//using GoogleLibrary.GoogleAuthentication;

//namespace GoogleLibrary.IntegrationTest.other
//{
//    //[TestClass]
//    public class TestCreateCustomCalendars : TestCustomSpreadsheetService
//    {
//        [TestCleanup]
//        public new async Task TestCleanup()
//        {
//            await Task.CompletedTask;
//        }

//        [DataTestMethod]
//        [DataRow("ArinsHoliday", true, 2)]
//        [DataRow("Crewe Alexandra", true, 1)]
//        [DataRow("Easter2024", true, 1)]
//        [DataRow("Formula 1", true, 1)]
//        [DataRow("Fuertoventura2023", true, 2)]
//        public async Task TestCreateCustomCalendar(string name, bool clear, int headerRowsCount)
//        {
//            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync($"{name}_3", clear)).Id;
//            await CustomSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, name, CalendarId, headerRowsCount: headerRowsCount);
//        }

//        [TestInitialize]
//        public new async Task TestInitialize()
//        {
//            await GoogleOAuthAuthenticatorHelper.CreateAsync(
//                GoogleSpreadsheetReadonlyService, GoogleCalendarService, GoogleCalendarsService);
//            SpreadsheetId = "1xXeM_uG2_z1dH9dAfSNZntxNfadmaWo6K9QrFHRURZg";
//            CustomSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
//        }
//    }
//}