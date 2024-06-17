using GoogleLibrary.Events;
using GoogleLibrary.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestAccomodationEvent
    {
        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow(" ")]
        public void AddCustomSummary_WithEmptyShortNameLocation_AddsTBC(string? shortName)
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent
            {
                Location = new Location(shortName)
            };

            // Act
            var result = accommodationEvent.AddCustomSummary();

            // Assert
            CollectionAssert.Contains(result, "TBC");
        }

        [TestMethod]
        public void AddCustomSummary_WithNonEmptyShortNameLocation_DoesNotAddTBC()
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent
            {
                Location = new Location("Location Name")
            };

            // Act
            var result = accommodationEvent.AddCustomSummary();

            // Assert
            CollectionAssert.DoesNotContain(result, "TBC");
        }

        [TestMethod]
        public void AddCustomSummary_WithNullLocation_AddsTBC()
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent();

            // Act
            var result = accommodationEvent.AddCustomSummary();

            // Assert
            CollectionAssert.Contains(result, "TBC");
        }

        [TestMethod]
        public void LocationSummary_WithNonEmptySummary_ReturnsSummary()
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent();
            var summary = "Sample Summary";

            // Act
            var result = accommodationEvent.LocationSummary(summary);

            // Assert
            Assert.AreEqual(summary, result);
        }

        [TestMethod]
        public void LocationSummary_WithNullOrWhiteSpaceSummaryAndEmptyShortNameLocation_ReturnsTBC()
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent
            {
                Location = new Location("")
            };

            // Act
            var result = accommodationEvent.LocationSummary(null);

            // Assert
            Assert.AreEqual("TBC", result);
        }

        [TestMethod]
        public void LocationSummary_WithNullOrWhiteSpaceSummaryAndNonEmptyShortNameLocation_ReturnsShortName()
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent
            {
                Location = new Location("Location Name")
            };

            // Act
            var result = accommodationEvent.LocationSummary(null);

            // Assert
            Assert.AreEqual("Location Name", result);
        }

        [TestMethod]
        public void LocationSummary_WithNullOrWhiteSpaceSummaryAndNullLocation_ReturnsTBC()
        {
            // Arrange
            var accommodationEvent = new AccommodationEvent();

            // Act
            var result = accommodationEvent.LocationSummary(null);

            // Assert
            Assert.AreEqual("TBC", result);
        }
    }
}