using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.Tests
{
    [TestClass]
    public class TestEventsBuilder
    {
        [TestMethod]
        public void Create_WithGoogleEvents_CreatesBasicEventsList()
        {
            var googleEvents = new Events
            {
                Items = new List<Event>
                {
                    new Event { Id = "1", Summary = "Meeting" },
                    new Event { Id = "2", Summary = "Review" }
                }
            };

            var result = EventsBuilder.Create(googleEvents);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Meeting", result[0].Title);
            Assert.AreEqual("Review", result[1].Title);
        }

        [TestMethod]
        public void Create_WithHeadersAndData_CreatesEventsList()
        {
            var headers = new List<string> { "EventId", "Summary", "StartDate", "StartTime", "EndDate", "EndTime", "Status", "Category", "Description" };
            var data = new List<IEnumerable<string>>
            {
                new List<string> { "1", "Meeting", "2023-06-17", "09:00", "2023-06-17", "10:00", "Confirmed", "Business", "Description 1" },
                new List<string> { "2", "Review", "2023-06-18", "11:00", "2023-06-18", "12:00", "Planned", "Personal", "Description 2" }
            };

            var result = EventsBuilder.Create(headers, data);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Meeting", result[0].Title);
            Assert.AreEqual("Review", result[1].Title);
        }

        [TestMethod]
        public void Create_WithValidData_CreatesEventsList()
        {
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new Tuple<string, EnumEventFieldType>("MyIdField", EnumEventFieldType.Unknown),
                new Tuple<string, EnumEventFieldType>("Summary", EnumEventFieldType.Summary),
                new Tuple<string, EnumEventFieldType>("StartDate", EnumEventFieldType.StartDate),
                new Tuple<string, EnumEventFieldType>("StartTime", EnumEventFieldType.StartTime),
                new Tuple<string, EnumEventFieldType>("EndDate", EnumEventFieldType.EndDate),
                new Tuple<string, EnumEventFieldType>("EndTime", EnumEventFieldType.EndTime),
                new Tuple<string, EnumEventFieldType>("Status", EnumEventFieldType.Status),
                new Tuple<string, EnumEventFieldType>("Category", EnumEventFieldType.Category),
                new Tuple<string, EnumEventFieldType>("Description", EnumEventFieldType.Description),
                new Tuple<string, EnumEventFieldType>("", EnumEventFieldType.Unknown),
                new Tuple<string, EnumEventFieldType>("MyOtherField", EnumEventFieldType.None),

                // Add more field mappings as required
            };

            var data = new List<IEnumerable<string>>
            {
                new List<string> { "1", "Meeting", "2023-06-17", "09:00", "2023-06-17", "10:00", "Confirmed", "Business", "Description 1", "Missing1", "Other1"},
                new List<string> { "2", "Review", "2023-06-18", "11:00", "2023-06-18", "12:00", "Planned", "Personal", "Description 2", "Missing2", "Other2" }
                // Add more data as required
            };

            var result = EventsBuilder.Create(fields, data);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Meeting", result[0].Title);
            Assert.AreEqual(new DateTime(2023, 06, 17).ToString(), result[0].StartDate.ToString());
            Assert.AreEqual("Review", result[1].Title);
            Assert.AreEqual(new DateTime(2023, 06, 18).ToString(), result[1].StartDate.ToString());
        }

        [TestMethod]
        public void FindDuplicateOrDefault_ReturnsDuplicateEvent()
        {
            var now = DateTime.UtcNow;
            var myEvent = new BasicEvent
            {
                Title = "Meeting",
                StartDate = now.Date,
                StartTime = now.Add(TimeSpan.FromHours(8)), // 08:00
                EndDate = now.Date,
                EndTime = now.Add(TimeSpan.FromHours(9)), // 09:00
            };

            var otherEvents = new List<BasicEvent>
            {
                new BasicEvent
                {
                    Title = "Meeting",
                    StartDate = myEvent.StartDate,
                    StartTime = myEvent.StartTime,
                    EndDate = myEvent.EndDate,
                    EndTime = myEvent.EndTime
                }
            };

            var result = EventsBuilder.FindDuplicateOrDefault(myEvent, otherEvents);

            Assert.IsNotNull(result);
            Assert.AreEqual(otherEvents[0].EventId, result.EventId);
        }

        [TestMethod]
        public void FindDuplicateOrDefault_ReturnsNullWhenNoDuplicate()
        {
            var now = DateTime.UtcNow;
            var myEvent = new BasicEvent
            {
                Title = "Meeting",
                StartDate = now.Date,
                StartTime = now.Add(TimeSpan.FromHours(8)), // 08:00
                EndDate = now.Date,
                EndTime = now.Add(TimeSpan.FromHours(9)), // 09:00
            };

            var notNow1 = DateTime.UtcNow.AddDays(1);
            var otherEvent1 = new BasicEvent
            {
                Title = "Meeting1",
                StartDate = notNow1.Date,
                StartTime = notNow1.Add(TimeSpan.FromHours(8)), // 08:00
                EndDate = notNow1.Date,
                EndTime = notNow1.Add(TimeSpan.FromHours(9)), // 09:00
            };
            var notNow2 = DateTime.UtcNow.AddDays(2);
            var otherEvent2 = new BasicEvent
            {
                Title = "Meeting2",
                StartDate = notNow2.Date,
                StartTime = notNow2.Add(TimeSpan.FromHours(8)), // 08:00
                EndDate = notNow2.Date,
                EndTime = notNow2.Add(TimeSpan.FromHours(9)), // 09:00
            };
            var otherEvents = new List<BasicEvent>
            {
                otherEvent1,
                otherEvent2
            };

            var result = EventsBuilder.FindDuplicateOrDefault(myEvent, otherEvents);

            Assert.IsNull(result);
        }
    }
}