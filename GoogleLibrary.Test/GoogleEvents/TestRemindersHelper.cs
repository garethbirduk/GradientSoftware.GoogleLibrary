using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Events;
using GoogleLibrary.Custom.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.GoogleEvents.Tests
{
    [TestClass]
    public class GoogleEventBuilderTests
    {
        [TestMethod]
        public void Build_WithMissingStartDateTime_ThrowsNullReferenceException()
        {
            var googleEvent = new Event { Summary = "Test Event" };
            Assert.ThrowsException<NullReferenceException>(() => googleEvent.Build());
        }

        [TestMethod]
        public void Build_WithMissingSummary_ThrowsGoogleEventBuilderException()
        {
            var googleEvent = new Event();
            Assert.ThrowsException<GoogleEventBuilderException>(() => googleEvent.Build());
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
                Locations = new List<Location>() { new Location("here") },
                Status = EventStatus.Confirmed,
                ContactableAttendees = new Dictionary<string, string> { { "Contact1", "contact1@test.com" } }
            };

            var googleEvent = GoogleEventBuilder.Create(myEvent);

            Assert.AreEqual(myEvent.Title, googleEvent.Summary);
            Assert.AreEqual(myEvent.ToDescriptionString(), googleEvent.Description);
            Assert.AreEqual(myEvent.ToLocationString(), googleEvent.Location);
            Assert.AreEqual(ColorId.Green.ToString(), googleEvent.ColorId);
        }

        [TestMethod]
        public void WithColour_SetsCorrectColorId()
        {
            var googleEvent = new Event();

            googleEvent.WithColour(EventStatus.Idea);
            Assert.AreEqual(ColorId.Orange.ToString(), googleEvent.ColorId);

            googleEvent.WithColour(EventStatus.Planned);
            Assert.AreEqual(ColorId.Yellow.ToString(), googleEvent.ColorId);

            googleEvent.WithColour(EventStatus.Confirmed);
            Assert.AreEqual(ColorId.Green.ToString(), googleEvent.ColorId);

            googleEvent.WithColour(EventStatus.Reserved);
            Assert.AreEqual(ColorId.Green.ToString(), googleEvent.ColorId);

            googleEvent.WithColour(EventStatus.Paid);
            Assert.AreEqual(ColorId.Cyan.ToString(), googleEvent.ColorId);

            googleEvent.WithColour(EventStatus.Cancelled);
            Assert.AreEqual(ColorId.Red.ToString(), googleEvent.ColorId);
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
                new EventReminder { Method = "email", Minutes = 10 }
            };

            googleEvent.WithReminders(reminders);
            Assert.AreEqual(reminders, googleEvent.Reminders.Overrides);
            Assert.IsFalse(googleEvent.Reminders.UseDefault);
        }
    }
}