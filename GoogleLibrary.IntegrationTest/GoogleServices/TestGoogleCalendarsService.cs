//using Google.Apis.Calendar.v3.Data;
//using GoogleLibrary.EventsServices;
//using GoogleLibrary.GoogleAuthentication;
//using GoogleLibrary.Services;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GoogleLibrary.Test.GoogleServices
//{
//    [TestClass]
//    public class TestGoogleCalendarsService : GoogleAuthenticatedUnitTest
//    {
//        [TestMethod]
//        public async Task TestDeleteCalendars_WithPredicate()
//        {
//            var calendars = GoogleCalendarsService.GetCalendars(x => new List<string>()
//            {
//                "0eee9f85",
//                "32b32e2e",
//                "88ff3c87",
//                "df9cf2ad",
//                "bea5ebb5",
//            }.Contains(x.Summary));
//            var calendarIds = calendars.Items.Select(x => x.Id).ToList();
//            foreach (var calendarId in calendarIds)
//                await GoogleCalendarsService.DeleteCalendarAsync(calendarId);
//        }

//        [TestInitialize]
//        public async Task TestInitialize()
//        {
//            await GoogleOAuthAuthenticatorHelper.CreateAsync(
//                GoogleCalendarsService);
//        }
//    }
//}