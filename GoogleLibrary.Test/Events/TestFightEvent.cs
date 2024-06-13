using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestFightEvent
    {
        [TestMethod]
        public void TestFlightInformation()
        {
            var e = new FlightEvent();
            e.FlightInformation.Carrier = "British Airways";
            e.FlightInformation.Number = "BA1234";
            Assert.AreEqual("British Airways (BA1234)", e.FlightInformation.FlightDetails);
            Assert.AreEqual("(BA1234)", e.FlightInformation.FlightSummary);
            Assert.AreEqual("https://www.flightradar24.com/BA1234", e.FlightInformation.FlightTracker);
        }
    }
}