using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Events;
using GoogleLibrary.GoogleEventBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.GoogleEventBuilders
{
    [TestClass]
    public class TestGoogleEventBuilder
    {
        [TestMethod]
        public void Build_WithMissingStartDateTime_ThrowsNullReferenceException()
        {
            var googleEvent = new Event { Summary = "Test Event" };
            Assert.ThrowsException<NullReferenceException>(googleEvent.Build);
        }

        [TestMethod]
        public void Build_WithMissingSummary_ThrowsGoogleEventBuilderException()
        {
            var googleEvent = new Event();
            Assert.ThrowsException<GoogleEventBuilderException>(googleEvent.Build);
        }

        [TestMethod]
        public void Build_WithNullEvent_ThrowsNullReferenceException()
        {
            Event googleEvent = null;
            Assert.ThrowsException<NullReferenceException>(() => googleEvent.Build());
        }

        [TestMethod]
        public void Create_WithValidBasicEvent_ReturnsGoogleEvent()
        {
            var myEvent = new BasicEvent
            {
                Title = "Test Event",
                StartDate = DateTime.Now,
                StartTime = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1),
                Description = "Test Description",
                Locations = [new("here")],
                Status = EventStatus.Confirmed,
                ContactableAttendees = new Dictionary<string, string> { { "Contact1", "contact1@test.com" } }
            };

            var googleEvent = GoogleEventBuilder.Create(myEvent);
            Assert.AreEqual(myEvent.Title, googleEvent.Summary);
            Assert.AreEqual(myEvent.ToDescriptionString(), googleEvent.Description);
            Assert.AreEqual(myEvent.ToLocationString(), googleEvent.Location);
        }

        [TestMethod]
        public void Create_WithValidBasicEvent_StartDateOnly()
        {
            var myEvent = new BasicEvent
            {
                Title = "Test Event",
                StartDate = DateTime.Now,
                Description = "Test Description",
                Locations = [new("here")],
                Status = EventStatus.Confirmed,
                ContactableAttendees = new Dictionary<string, string> { { "Contact1", "contact1@test.com" } }
            };

            var googleEvent = GoogleEventBuilder.Create(myEvent);

            Assert.AreEqual(myEvent.Title, googleEvent.Summary);
            Assert.AreEqual(myEvent.ToDescriptionString(), googleEvent.Description);
            Assert.AreEqual(myEvent.ToLocationString(), googleEvent.Location);
        }

        [TestMethod]
        public void Create_WithValidBasicEvent_StartDateTimeOnly()
        {
            var myEvent = new BasicEvent
            {
                Title = "Test Event",
                StartDate = DateTime.Now,
                StartTime = DateTime.Now,
                Description = "Test Description",
                Locations = [new("here")],
                Status = EventStatus.Confirmed,
                ContactableAttendees = new Dictionary<string, string> { { "Contact1", "contact1@test.com" } }
            };

            var googleEvent = GoogleEventBuilder.Create(myEvent);

            Assert.AreEqual(myEvent.Title, googleEvent.Summary);
            Assert.AreEqual(myEvent.ToDescriptionString(), googleEvent.Description);
            Assert.AreEqual(myEvent.ToLocationString(), googleEvent.Location);
        }

        [DataTestMethod]
        [DataRow(EventStatus.None, "MyEvent")]
        [DataRow(EventStatus.Idea, "(?) MyEvent")]
        [DataRow(EventStatus.Planned, "(p) MyEvent")]
        [DataRow(EventStatus.Confirmed, "MyEvent")]
        [DataRow(EventStatus.Reserved, "(r) MyEvent")]
        [DataRow(EventStatus.Paid, "(£) MyEvent")]
        [DataRow(EventStatus.Cancelled, "(x) MyEvent")]
        public void WithAnnotation_SetsCorrectAnnotation(EventStatus eventStatus, string expected)
        {
            var googleEvent = new Event()
            {
                Summary = "MyEvent"
            };

            googleEvent.WithAnnotation(eventStatus);
            Assert.AreEqual(expected, googleEvent.Summary);
        }

        [TestMethod]
        public void WithColour_DefaultHasNoSetColour()
        {
            var googleEvent = new Event()
            {
                Summary = "MyEvent"
            };

            googleEvent.WithColour(EventStatus.None);
            Assert.AreEqual(null, googleEvent.ColorId);
        }

        [DataTestMethod]
        [DataRow(EventStatus.Idea, ColorId.Orange)]
        [DataRow(EventStatus.Planned, ColorId.Yellow)]
        [DataRow(EventStatus.Confirmed, ColorId.Green)]
        [DataRow(EventStatus.Reserved, ColorId.Green)]
        [DataRow(EventStatus.Paid, ColorId.Cyan)]
        [DataRow(EventStatus.Cancelled, ColorId.Red)]
        public void WithColour_SetsCorrectColorId(EventStatus eventStatus, ColorId expected)
        {
            var googleEvent = new Event()
            {
                Summary = "MyEvent"
            };

            googleEvent.WithColour(eventStatus);
            Assert.AreEqual(((int)expected).ToString(), googleEvent.ColorId);
        }

        [TestMethod]
        public void WithDescription_SetsDescription()
        {
            var googleEvent = new Event();
            googleEvent.WithDescription("Test Description");
            Assert.AreEqual("Test Description", googleEvent.Description);
        }

        [TestMethod]
        public void WithLocation_SetsLocation()
        {
            var googleEvent = new Event();
            googleEvent.WithLocation("Test Location");
            Assert.AreEqual("Test Location", googleEvent.Location);
        }

        [TestMethod]
        public void WithReminders_SetsReminders()
        {
            var googleEvent = new Event();
            var reminders = new List<EventReminder>
            {
                new () { Method = "email", Minutes = 10 }
            };

            googleEvent.WithReminders(reminders);
            Assert.AreEqual(reminders, googleEvent.Reminders.Overrides);
            Assert.IsFalse(googleEvent.Reminders.UseDefault);
        }
    }
}