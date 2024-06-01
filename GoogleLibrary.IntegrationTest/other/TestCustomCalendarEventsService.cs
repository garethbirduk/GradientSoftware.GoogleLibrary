//using GoogleLibrary.EventsServices;
//using GoogleLibrary;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Threading.Tasks;
//using GoogleLibrary.Services;

//namespace GoogleLibrary.Test
//{
//    [TestClass]
//    public class TestCustomCalendarEventsService : GoogleAuthenticatedUnitTest
//    {
//        private CustomCalendarService GoogleCalendarsService;

//        [TestInitialize]
//        public async Task TestInitialize()
//        {
//            await GoogleAuth.SetupAuthAsync();
//            var customCalendarReadonlyService = new GoogleCalendarReadonlyService()
//            {
//                CalendarId = "Crewe Alexandra",
//                ClientSecrets = GoogleAuth.ClientSecrets
//            };
//            GoogleCalendarsService = new CustomCalendarService(customCalendarReadonlyService);
//        }
//    }
//}