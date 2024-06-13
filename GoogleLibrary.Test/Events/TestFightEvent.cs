using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestFightEvent
    {
        [DataTestMethod]
        [DataRow(" ", "  ", "", "", "")]
        [DataRow("British Airways", "BA1234", "British Airways (BA1234)", "(BA1234)", "https://www.flightradar24.com/BA1234")]
        public void TestFlightInformation(string carrier, string number, string expectedDetails, string expectedSummary, string expectedTracker)
        {
            var e = new FlightEvent();
            e.FlightInformation.Carrier = carrier;
            e.FlightInformation.Number = number;
            Assert.AreEqual(expectedDetails, e.FlightInformation.FlightDetails);
            Assert.AreEqual(expectedSummary, e.FlightInformation.FlightSummary);
            Assert.AreEqual(expectedTracker, e.FlightInformation.FlightTracker);
        }
    }
}