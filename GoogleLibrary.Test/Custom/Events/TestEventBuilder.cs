using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Events;
using GoogleLibrary.GoogleEventBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestEventBuilder
    {
        [TestMethod]
        public void TestCreate()
        {
            var fields = new List<Tuple<string, EnumEventFieldType>>()
            {
                new("Summary", EnumEventFieldType.Summary),
            };
            var data = new List<string>()
            {
                "MySummary"
            };
            Assert.IsNotNull(EventBuilder.Create(fields, data));
        }

        [TestMethod]
        public void TestCreateBasicEventFromGoogleEvent()
        {
            // Arrange
            var googleEvent = new Event { Summary = "Test Event" };

            // Act
            var result = EventBuilder.Create(googleEvent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Event", result.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateBasicEventFromNullGoogleEvent()
        {
            // Arrange
            Event? googleEvent = null;

            // Act
            EventBuilder.Create(googleEvent!);
        }

        [DataTestMethod]
        [DataRow("(i) Hotel Hilton", EventStatus.Idea, "Hotel Hilton")]
        [DataRow("(p) Hotel Hilton", EventStatus.Planned, "Hotel Hilton")]
        [DataRow("(r) Hotel Hilton", EventStatus.Reserved, "Hotel Hilton")]
        [DataRow("(£) Hotel Hilton", EventStatus.Paid, "Hotel Hilton")]
        [DataRow("(x) Hotel Hilton", EventStatus.Cancelled, "Hotel Hilton")]
        [DataRow("Hotel Hilton", EventStatus.None, "Hotel Hilton")]
        public void Create_ParsesStatusFromAnnotationPrefix(string summary, EventStatus expectedStatus, string expectedTitle)
        {
            var result = EventBuilder.Create(new Event { Summary = summary });

            Assert.AreEqual(expectedStatus, result.Status);
            Assert.AreEqual(expectedTitle, result.Title);
        }

        [DataTestMethod]
        [DataRow(6, EventStatus.Idea)]      // Orange
        [DataRow(5, EventStatus.Planned)]   // Yellow
        [DataRow(10, EventStatus.Confirmed)] // Green (Reserved is indistinguishable)
        [DataRow(7, EventStatus.Paid)]      // Cyan
        [DataRow(11, EventStatus.Cancelled)] // Red
        public void Create_FallsBackToColorIdWhenNoAnnotation(int colorId, EventStatus expected)
        {
            var result = EventBuilder.Create(new Event { Summary = "Plain", ColorId = colorId.ToString() });

            Assert.AreEqual(expected, result.Status);
            Assert.AreEqual("Plain", result.Title);
        }

        [TestMethod]
        public void Create_ParsesAllDayDates_AndUndoesExclusiveEnd()
        {
            var googleEvent = new Event
            {
                Summary = "Trip",
                Start = new EventDateTime { Date = "2026-05-10" },
                End = new EventDateTime { Date = "2026-05-13" }, // exclusive — actual last day is 12th
            };

            var result = EventBuilder.Create(googleEvent);

            Assert.AreEqual(new DateTime(2026, 5, 10), result.StartDate);
            Assert.AreEqual(new DateTime(2026, 5, 12), result.EndDate);
            Assert.IsNull(result.StartTime);
            Assert.IsNull(result.EndTime);
        }

        [TestMethod]
        public void Create_ParsesTimedDates()
        {
            var start = new DateTime(2026, 5, 10, 14, 30, 0);
            var end = new DateTime(2026, 5, 10, 16, 0, 0);
            var googleEvent = new Event
            {
                Summary = "Meeting",
                Start = new EventDateTime { DateTimeDateTimeOffset = start },
                End = new EventDateTime { DateTimeDateTimeOffset = end },
            };

            var result = EventBuilder.Create(googleEvent);

            Assert.AreEqual(start.Date, result.StartDate);
            Assert.AreEqual(start.TimeOfDay, result.StartTime?.TimeOfDay);
            Assert.AreEqual(end.Date, result.EndDate);
            Assert.AreEqual(end.TimeOfDay, result.EndTime?.TimeOfDay);
        }

        [TestMethod]
        public void Create_ParsesDescriptionIntoDescriptionCustomFieldsAndAdditionalData()
        {
            var googleEvent = new Event
            {
                Summary = "Flight",
                Description = "Booked via Skyscanner\r\nAisle seat preferred\r\nFlight: BA (175)\r\nFlight tracker: https://www.flightradar24.com/BA175\r\nbring passport\r\ncheck in 2h before",
            };

            var result = EventBuilder.Create(googleEvent);

            Assert.AreEqual("Booked via Skyscanner\r\nAisle seat preferred", result.Description);
            Assert.AreEqual("BA (175)", result.CustomFields["Flight"]);
            Assert.AreEqual("https://www.flightradar24.com/BA175", result.CustomFields["Flight tracker"]);
            CollectionAssert.AreEqual(new[] { "bring passport", "check in 2h before" }, result.AdditionalData);
        }

        [TestMethod]
        public void Create_ParsesSingleLocationFromMapsSearchUrl()
        {
            var googleEvent = new Event
            {
                Summary = "Lunch",
                Location = "https://www.google.com/maps/search/123+High+Street+London",
            };

            var result = EventBuilder.Create(googleEvent);

            Assert.AreEqual(1, result.Locations.Count);
            Assert.AreEqual("123 High Street London", result.Locations[0].Address);
        }

        [TestMethod]
        public void Create_ParsesMultiLegLocationsFromMapsDirUrl()
        {
            var googleEvent = new Event
            {
                Summary = "Drive",
                Location = "https://www.google.com/maps/dir/London+Heathrow+Airport+Departures/Verbier+Switzerland",
            };

            var result = EventBuilder.Create(googleEvent);

            Assert.AreEqual(2, result.Locations.Count);
            Assert.AreEqual("London Heathrow Airport Departures", result.Locations[0].Address);
            Assert.AreEqual("Verbier Switzerland", result.Locations[1].Address);
        }

        [TestMethod]
        public void Create_ParsesReminderOverrides()
        {
            var googleEvent = new Event
            {
                Summary = "Flight",
                Reminders = new Event.RemindersData
                {
                    Overrides = new List<EventReminder>
                    {
                        new() { Method = "popup", Minutes = 120 },
                        new() { Method = "popup", Minutes = 240 },
                    },
                    UseDefault = false,
                },
            };

            var result = EventBuilder.Create(googleEvent);

            CollectionAssert.AreEqual(new[] { 120, 240 }, result.Reminders);
        }

        [TestMethod]
        public void RoundTrip_BasicEventToGoogleEventAndBack_PreservesCoreFields()
        {
            var original = new BasicEvent
            {
                Title = "Hotel Hilton",
                Status = EventStatus.Planned,
                StartDate = new DateTime(2026, 5, 10),
                StartTime = new DateTime(2026, 5, 10, 15, 0, 0),
                EndDate = new DateTime(2026, 5, 11),
                EndTime = new DateTime(2026, 5, 11, 11, 0, 0),
                Description = "Confirmation #ABC123",
                Reminders = new List<int> { 60, 720 },
            };
            original.CustomFields["Booking"] = "ABC123";

            var googleEvent = GoogleEventBuilder.Create(new GoogleEventOptions { WithAnnotation = true, WithColour = true }, original);
            var roundTripped = EventBuilder.Create(googleEvent);

            Assert.AreEqual(original.Title, roundTripped.Title);
            Assert.AreEqual(original.Status, roundTripped.Status);
            Assert.AreEqual(original.StartDate, roundTripped.StartDate);
            Assert.AreEqual(original.StartTime?.TimeOfDay, roundTripped.StartTime?.TimeOfDay);
            Assert.AreEqual(original.EndDate, roundTripped.EndDate);
            Assert.AreEqual(original.EndTime?.TimeOfDay, roundTripped.EndTime?.TimeOfDay);
            Assert.AreEqual(original.Description, roundTripped.Description);
            Assert.AreEqual("ABC123", roundTripped.CustomFields["Booking"]);
            CollectionAssert.AreEqual(original.Reminders.OrderBy(x => x).ToList(), roundTripped.Reminders.OrderBy(x => x).ToList());
        }

        [TestMethod]
        public void TestCreateEventWithAccommodationCategory()
        {
            // Arrange
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                Tuple.Create("Category", EnumEventFieldType.Category)
            };
            var data = new List<string> { "Accommodation" };

            // Act
            var result = EventBuilder.Create(fields, data);

            // Assert
            Assert.IsInstanceOfType(result, typeof(AccommodationEvent));
        }

        [TestMethod]
        public void TestCreateEventWithDefaultCategory()
        {
            // Arrange
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                Tuple.Create("Category", EnumEventFieldType.Category)
            };
            var data = new List<string> { "NonExistentCategory" };

            // Act
            var result = EventBuilder.Create(fields, data);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BasicEvent));
        }

        [TestMethod]
        public void TestCreateEventWithFlightCategory()
        {
            // Arrange
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                Tuple.Create("Category", EnumEventFieldType.Category)
            };
            var data = new List<string> { "Flight" };

            // Act
            var result = EventBuilder.Create(fields, data);

            // Assert
            Assert.IsInstanceOfType(result, typeof(FlightEvent));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateEventWithMissingCategoryField()
        {
            // Arrange
            var fields = new List<Tuple<string, EnumEventFieldType>>();
            var data = new List<string>();

            // Act
            EventBuilder.Create(fields, data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateEventWithNullData()
        {
            // Arrange
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                Tuple.Create("Category", EnumEventFieldType.Category)
            };

            // Act
            EventBuilder.Create(fields, null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateEventWithNullFields()
        {
            // Arrange
            var data = new List<string>();

            // Act
            EventBuilder.Create(null!, data);
        }
    }
}