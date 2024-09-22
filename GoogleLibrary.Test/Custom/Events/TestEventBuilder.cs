using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            Event googleEvent = null;

            // Act
            EventBuilder.Create(googleEvent);
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
            EventBuilder.Create(fields, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateEventWithNullFields()
        {
            // Arrange
            var data = new List<string>();

            // Act
            EventBuilder.Create(null, data);
        }
    }
}