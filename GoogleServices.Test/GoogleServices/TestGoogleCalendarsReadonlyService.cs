namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarsReadonlyService : GoogleAuthenticatedUnitTest
    {
        [DataTestMethod]
        [DataRow("garethbird@gmail.com")]
        public void TestGetCalendar(string summary)
        {
            var calendar = GoogleCalendarsReadonlyService.GetCalendar(summary);
            Assert.AreEqual(summary, calendar.Summary);
        }

        [DataTestMethod]
        [DataRow("garethbird@gmail.com")]
        [DataRow("Family")]
        public void TestGetCalendarBySummary(string summary)
        {
            var calendar = GoogleCalendarsReadonlyService.GetCalendarBySummary(summary);
            Assert.AreEqual(summary, calendar.Summary);
        }

        [TestMethod]
        public void TestGetCalendars()
        {
            var calendars = GoogleCalendarsReadonlyService.GetCalendars();
            Assert.IsTrue(calendars.Items.Count > 0);
            Assert.IsTrue(calendars.Items.Count == calendars.Items.Count);
            Assert.AreEqual(1, calendars.Items.Where(x => x.Summary == "garethbird@gmail.com").Count());
        }

        [TestMethod]
        public void TestGetCalendars_WithPredicate()
        {
            var calendars = GoogleCalendarsReadonlyService.GetCalendars(x => x.Summary.StartsWith("Arin"));
            var names = calendars.Items.Select(x => x.Summary).ToList();
            foreach (var name in names)
            {
                Assert.IsTrue(name.StartsWith("Arin"));
                Console.WriteLine(name);
            }
        }
    }
}