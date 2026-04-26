using GoogleLibrary.Custom.Events;
using GoogleLibrary.Custom.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestWorksheetCalendarMapping
    {
        [TestMethod]
        public void EncodeHeaders_ProducesHeadersPrefixedDelimitedLine()
        {
            var encoded = WorksheetCalendarMapping.EncodeHeaders(["Date", "Time", "Summary"]);
            Assert.AreEqual("Headers*/*Date*/*Time*/*Summary", encoded);
        }

        [TestMethod]
        public void TryDecodeHeaders_FindsHeadersLineWithinMultiLineDescription()
        {
            var description = "MyWorksheet\r\nHeaders*/*Date*/*Time*/*Summary\r\nsome other line";

            var ok = WorksheetCalendarMapping.TryDecodeHeaders(description, out var headers);

            Assert.IsTrue(ok);
            CollectionAssert.AreEqual(new[] { "Date", "Time", "Summary" }, headers);
        }

        [TestMethod]
        public void TryDecodeHeaders_ReturnsFalseWhenNoHeadersLine()
        {
            var ok = WorksheetCalendarMapping.TryDecodeHeaders("just a worksheet name", out var headers);
            Assert.IsFalse(ok);
            Assert.AreEqual(0, headers.Count);
        }

        [TestMethod]
        public void EncodeThenDecodeHeaders_RoundTrips()
        {
            var original = new[] { "StartDate", "Start Time", "Summary", "£" };

            var encoded = WorksheetCalendarMapping.EncodeHeaders(original);
            var ok = WorksheetCalendarMapping.TryDecodeHeaders(encoded, out var decoded);

            Assert.IsTrue(ok);
            CollectionAssert.AreEqual(original, decoded);
        }

        [TestMethod]
        public void ToCellValues_ProjectsKnownFieldsInHeaderOrder()
        {
            var basicEvent = new BasicEvent
            {
                Title = "Hotel Hilton",
                Status = EventStatus.Planned,
                Category = EventCategory.Accommodation,
                StartDate = new DateTime(2026, 5, 10),
                StartTime = new DateTime(2026, 5, 10, 15, 0, 0),
                EndDate = new DateTime(2026, 5, 11),
                EndTime = new DateTime(2026, 5, 11, 11, 0, 0),
                Description = "Booked",
                Reminders = new List<int> { 60, 120 },
                Locations = new List<Location>
                {
                    new("Manchester"),
                    new("London"),
                },
            };

            var headers = new[] { "Summary", "StartDate", "StartTime", "EndDate", "EndTime", "Status", "Category", "From", "To", "Description", "Reminders" };

            var values = WorksheetCalendarMapping.ToCellValues(basicEvent, headers);

            CollectionAssert.AreEqual(new[]
            {
                "Hotel Hilton",
                "2026-05-10",
                "15:00",
                "2026-05-11",
                "11:00",
                "Planned",
                "Accommodation",
                "Manchester",
                "London",
                "Booked",
                "60, 120",
            }, values);
        }

        [TestMethod]
        public void ToCellValues_EmitsBlanksForMissingOptionalFields()
        {
            var basicEvent = new BasicEvent
            {
                Title = "Meeting",
                StartDate = new DateTime(2026, 5, 10),
            };

            var headers = new[] { "Summary", "StartTime", "Status", "Category", "From" };

            var values = WorksheetCalendarMapping.ToCellValues(basicEvent, headers);

            CollectionAssert.AreEqual(new[] { "Meeting", "", "", "", "" }, values);
        }

        [TestMethod]
        public void ToCellValues_RoutesUnknownHeaderToCustomFieldsLookup()
        {
            var basicEvent = new BasicEvent { Title = "Trip" };
            basicEvent.CustomFields["Booking"] = "ABC123";

            var headers = new[] { "Summary", "Booking", "NotPresent" };

            var values = WorksheetCalendarMapping.ToCellValues(basicEvent, headers);

            CollectionAssert.AreEqual(new[] { "Trip", "ABC123", "" }, values);
        }

        [TestMethod]
        public void ToCellValues_FlightCarrierAndNumberRoutedToFlightInformation()
        {
            var flight = new FlightEvent();
            flight.FlightInformation.Carrier = "BA";
            flight.FlightInformation.Number = "175";

            var values = WorksheetCalendarMapping.ToCellValues(flight, ["FlightCarrier", "FlightNumber"]);

            CollectionAssert.AreEqual(new[] { "BA", "175" }, values);
        }
    }
}
