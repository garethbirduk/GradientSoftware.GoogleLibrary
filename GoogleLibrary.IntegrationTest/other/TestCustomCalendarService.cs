//using GoogleLibrary.CustomServices;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Threading.Tasks;
//using GoogleLibrary.Services;
//using GoogleLibrary.GoogleAuthentication;

//namespace GoogleLibrary.Test
//{
//    [TestClass]
//    public class TestCustomCalendarService : GoogleAuthenticatedUnitTest
//    {
//        [TestMethod]
//        public async Task TestCalendarToWorksheetAsync()
//        {
//            var worksheetName = Utils.RandomName();
//            CalendarId = GoogleCalendarsReadonlyService.GetCalendarBySummary(worksheetName).Id;

//            try
//            {
//                await CustomCalendarService.CalendarToWorksheetAsync(CalendarId, SpreadsheetId, worksheetName);
//            }
//            finally
//            {
//                await GoogleSpreadsheetService.DeleteWorksheetsAsync(SpreadsheetId, worksheetName);
//            }
//        }

//        [TestInitialize]
//        public async Task TestInitialize()
//        {
//            await GoogleOAuthAuthenticatorHelper.CreateAsync(
//                GoogleCalendarsReadonlyService, GoogleCalendarReadonlyService, GoogleSpreadsheetService, GoogleCalendarService);
//            CustomSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
//            CustomCalendarService = new CustomCalendarService(GoogleCalendarReadonlyService, GoogleSpreadsheetService);
//        }
//    }
//}