using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLibrary.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarReadonlyService : GoogleAuthenticatedUnitTest
    {
        [DataTestMethod]
        [DataRow("garethbird@gmail.com", "garethbird@gmail.com")]
        [DataRow("nrgm37qj4a8tsq9rt8th7fcicg@group.calendar.google.com", "Crewe Alexandra")]
        public void TestCalendarSummary(string calendarId, string expectedSummary)
        {
            Assert.AreEqual(expectedSummary, GoogleCalendarReadonlyService.Calendar(calendarId).Summary);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarReadonlyService);
        }
    }
}