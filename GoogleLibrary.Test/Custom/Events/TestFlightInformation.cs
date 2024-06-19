using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestFlightInformation
    {
        [TestMethod]
        public void FlightDetails_CarrierNoNumber_ReturnsCarrier()
        {
            var flightInfo = new FlightInformation { Carrier = "Delta" };

            Assert.AreEqual("Delta", flightInfo.FlightDetails);
        }

        [TestMethod]
        public void FlightDetails_CarrierWithNumber_ReturnsCarrierWithFlightSummary()
        {
            var flightInfo = new FlightInformation { Carrier = "Delta", Number = "DL1234" };

            Assert.AreEqual("Delta (DL1234)", flightInfo.FlightDetails);
        }

        [TestMethod]
        public void FlightDetails_NoCarrierNoNumber_ReturnsEmptyString()
        {
            var flightInfo = new FlightInformation();

            Assert.AreEqual(string.Empty, flightInfo.FlightDetails);
        }

        [TestMethod]
        public void FlightSummary_NoNumber_ReturnsEmptyString()
        {
            var flightInfo = new FlightInformation();

            Assert.AreEqual(string.Empty, flightInfo.FlightSummary);
        }

        [TestMethod]
        public void FlightSummary_WithNumber_ReturnsFormattedNumber()
        {
            var flightInfo = new FlightInformation { Number = "DL1234" };

            Assert.AreEqual("(DL1234)", flightInfo.FlightSummary);
        }

        [TestMethod]
        public void FlightTracker_NoNumber_ReturnsEmptyString()
        {
            var flightInfo = new FlightInformation();

            Assert.AreEqual(string.Empty, flightInfo.FlightTracker);
        }

        [TestMethod]
        public void FlightTracker_WithNumber_ReturnsFormattedUrl()
        {
            var flightInfo = new FlightInformation { Number = "DL1234" };

            Assert.AreEqual("https://www.flightradar24.com/DL1234", flightInfo.FlightTracker);
        }
    }
}