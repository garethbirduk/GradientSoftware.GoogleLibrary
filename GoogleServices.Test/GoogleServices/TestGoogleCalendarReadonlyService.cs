using GoogleServices.GoogleAuthentication;
using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarReadonlyService : GoogleAuthenticatedUnitTest
    {
        [DataTestMethod]
        [DataRow("garethbird@gmail.com", "garethbird@gmail.com")]
        [DataRow("vbhhaeru6oq3mmg8jance8sar8@group.calendar.google.com", "Bramcote CofE Y1 22/23 (Y2 23/24)")]
        public void TestCalendarSummary(string calendarId, string expectedSummary)
        {
            Assert.AreEqual(expectedSummary, GoogleCalendarReadonlyService.Calendar(calendarId).Summary);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>(
                GoogleCalendarReadonlyService);
        }
    }
}