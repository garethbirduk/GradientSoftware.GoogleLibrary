using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.GoogleExtensions;
using GoogleServices.GoogleAuthentication;
using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarGoogleCalendarEventsService : GoogleAuthenticatedUnitTest
    {
        private static readonly string CalendarSummary = "TEST_D5425FA-D046-49E4-8AFB-D0768F28F774";

        private static void AssertEvents(Event myEvent, Event myEvent2, bool enforceSameIds = false)
        {
            if (enforceSameIds)
                Assert.AreEqual(myEvent.Id, myEvent2.Id);
            else
                Assert.AreNotEqual(myEvent.Id, myEvent2.Id);
            Assert.AreEqual(myEvent.Start.Date, myEvent2.Start.Date);
            Assert.AreEqual(myEvent.Start.DateTimeDateTimeOffset.ToString(), myEvent2.Start.DateTimeDateTimeOffset.ToString());
            Assert.AreEqual(myEvent.End.Date, myEvent2.End.Date);
            Assert.AreEqual(myEvent.End.DateTimeDateTimeOffset.ToString(), myEvent2.End.DateTimeDateTimeOffset.ToString());
            Assert.AreEqual(myEvent2.Summary, myEvent2.Summary);
            Assert.AreEqual(myEvent.Description, myEvent2.Description);
        }

        private static Event CreateAllDayEvent(string summary = "TestAllDay")
        {
            var myEvent = new Event
            {
                Summary = summary,
                Description = $"{summary} Description"
            };
            myEvent.SetAllDay(DateTime.UtcNow);
            return myEvent;
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await GoogleCalendarEventsService.DeleteEventsAsync(CalendarId, x => true);
        }

        [TestMethod]
        public void TestCreateAllDay()
        {
            var myEvent = CreateAllDayEvent();
            var myEvent2 = GoogleCalendarEventsService.CreateEvent(CalendarId, myEvent);
            AssertEvents(myEvent, myEvent2);
        }

        [TestMethod]
        public void TestCreateEvents()
        {
            var myEvents = new List<Event>() { CreateAllDayEvent("1"), CreateAllDayEvent("2") };
            GoogleCalendarEventsService.CreateEvents(CalendarId, myEvents);
            var events = GoogleCalendarEventsService.GetEvents(CalendarId);
            AssertEvents(myEvents[0], events.Items.Where(x => x.Summary == "1").Single());
            AssertEvents(myEvents[1], events.Items.Where(x => x.Summary == "2").Single());
        }

        [TestMethod]
        public void TestGetEvent()
        {
            var myEvent = CreateAllDayEvent();
            var myGoogleEvent1 = GoogleCalendarEventsService.CreateEvent(CalendarId, myEvent);
            var myGoogleEvent2 = GoogleCalendarEventsService.GetEvent(CalendarId, myGoogleEvent1.Id);
            AssertEvents(myGoogleEvent1, myGoogleEvent2, true);
        }

        [TestMethod]
        public void TestGetEvents()
        {
            TestCreateAllDay();
            var events = GoogleCalendarEventsService.GetEvents(CalendarId);
            Assert.AreEqual(1, events.Items.Count);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarsService, GoogleCalendarEventsService);
            CalendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(CalendarSummary)).Id;
            await GoogleCalendarEventsService.DeleteEventsAsync(CalendarId, x => true);
        }

        [TestMethod]
        public void TestModifyNoId()
        {
            var myEvent = CreateAllDayEvent();
            Assert.ThrowsException<ArgumentException>(() => GoogleCalendarEventsService.ReplaceEvent(CalendarId, myEvent));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestMultiDay(int days)
        {
            var myEvent = new Event();
            var start = DateTime.UtcNow;
            var end = DateTime.UtcNow.AddDays(days);
            myEvent.Summary = $"TestMultiDay s:{start} e:{end}";
            myEvent.Description = "TestMultiDay Description";
            myEvent.SetMultiDay(start, end);

            var myEvent2 = GoogleCalendarEventsService.CreateEvent(CalendarId, myEvent);
            AssertEvents(myEvent, myEvent2);
        }

        [TestMethod]
        public void TestPatchAllDayEvent()
        {
            var originalEvent = GoogleCalendarEventsService.CreateEvent(CalendarId, CreateAllDayEvent());
            Assert.AreEqual("TestAllDay", originalEvent.Summary);
            Assert.AreEqual("TestAllDay Description", originalEvent.Description);
            Assert.IsFalse(string.IsNullOrWhiteSpace(originalEvent.Id));

            var modifiedEvent = new Event()
            {
                Id = originalEvent.Id,
                Description = "New description"
            };
            GoogleCalendarEventsService.PatchEvent(CalendarId, modifiedEvent);

            var myEvent = GoogleCalendarEventsService.GetEvent(CalendarId, originalEvent.Id);
            Assert.AreEqual("TestAllDay", myEvent.Summary);
            Assert.AreEqual("New description", myEvent.Description);
        }

        [TestMethod]
        public void TestPatchNoId()
        {
            var myEvent = CreateAllDayEvent();
            Assert.ThrowsException<ArgumentException>(() => GoogleCalendarEventsService.PatchEvent(CalendarId, myEvent));
        }

        [TestMethod]
        public void TestReplaceAllDayEvent()
        {
            var originalEvent = GoogleCalendarEventsService.CreateEvent(CalendarId, CreateAllDayEvent());
            Assert.AreEqual("TestAllDay", originalEvent.Summary);
            Assert.AreEqual("TestAllDay Description", originalEvent.Description);
            Assert.IsFalse(string.IsNullOrWhiteSpace(originalEvent.Id));

            var modifiedEvent = GoogleCalendarEventsService.GetEvent(CalendarId, originalEvent.Id);
            modifiedEvent.Description = "New description";
            GoogleCalendarEventsService.ReplaceEvent(CalendarId, modifiedEvent);

            var myEvent = GoogleCalendarEventsService.GetEvent(CalendarId, originalEvent.Id);
            Assert.AreEqual("TestAllDay", myEvent.Summary);
            Assert.AreEqual("New description", myEvent.Description);
        }

        [DataTestMethod]
        [DataRow(2)]
        [DataRow(22)]
        public void TestTimed(int hours)
        {
            var myEvent = new Event
            {
                Summary = "TestTimed",
                Description = "TestTimed Description"
            };
            myEvent.SetTimed(DateTime.UtcNow, DateTime.UtcNow.AddHours(hours));

            var myEvent2 = GoogleCalendarEventsService.CreateEvent(CalendarId, myEvent);
            AssertEvents(myEvent, myEvent2);
        }
    }
}