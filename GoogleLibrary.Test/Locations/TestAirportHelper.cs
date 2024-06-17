using GoogleLibrary.Events;
using GoogleLibrary.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Locations
{
    [TestClass]
    public class TestAirportHelper
    {
        [TestMethod]
        public void TestCreateAirportToAirport()
        {
            var from = new AirportLocation("VVI");
            var to = new AirportLocation("MAD");
            AirportHelper.SetAddresses(from, to);
            Assert.AreEqual("Viru Viru International Airport", from.Address);
            Assert.AreEqual("Adolfo Suárez Madrid–Barajas Airport", to.Address);
        }

        [TestMethod]
        public void TestCreateAirportToOtherLocation()
        {
            var from = new AirportLocation("LAX");
            var to = new Location("Tombstone");
            AirportHelper.SetAddresses(from, to);
            Assert.AreEqual("Los Angeles International Airport Arrivals", from.Address);
            Assert.AreEqual("Tombstone", to.Address);
        }

        [TestMethod]
        public void TestCreateOtherLocationToAirport()
        {
            var from = new Location("Manchester");
            var to = new AirportLocation("MAN");
            AirportHelper.SetAddresses(from, to);
            Assert.AreEqual("Manchester", from.Address);
            Assert.AreEqual("Manchester Airport Departures", to.Address);
        }

        [DataTestMethod]
        [DataRow("", false)]
        [DataRow("lhr", true)]
        [DataRow("LHR", true)]
        [DataRow("XYZ", false)]
        [DataRow("aaaa", false)]
        [DataRow("London Heathrow", false)]
        public void TestIsAirport(string shortName, bool expected)
        {
            Assert.AreEqual(expected, shortName.IsAirport());
        }
    }
}