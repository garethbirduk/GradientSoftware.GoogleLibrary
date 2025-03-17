using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarReadonlyService
    {
        private static GoogleCalendarReadonlyService GoogleCalendarReadonlyService = new();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GoogleCalendarReadonlyService = new();
            GoogleCalendarReadonlyService.Initialize();
        }

        [DataTestMethod]
        [DataRow("garethbird@gmail.com", "garethbird@gmail.com")]
        [DataRow("vbhhaeru6oq3mmg8jance8sar8@group.calendar.google.com", "Bramcote CofE Y3 24/25")]
        public void TestCalendarSummary(string calendarId, string expectedSummary)
        {
            Assert.AreEqual(expectedSummary, GoogleCalendarReadonlyService.Calendar(calendarId).Summary);
        }
    }
}