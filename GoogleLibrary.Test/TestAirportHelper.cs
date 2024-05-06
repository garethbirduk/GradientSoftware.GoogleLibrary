using GoogleLibrary.Custom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestAirportHelper
    {
        [DataTestMethod]
        [DataRow("", false)]
        [DataRow("lhr", true)]
        [DataRow("LHR", true)]
        [DataRow("XYZ", true)]
        [DataRow("aaaa", false)]
        [DataRow("London Heathrow", false)]
        public void TestIsAirport(string shortName, bool expected)
        {
            Assert.AreEqual(expected, new Location(shortName, "").IsAirport());
        }

        [DataTestMethod]
        [DataRow("", "", false)]
        [DataRow("LHR", "LHR", false)]
        [DataRow("LHR", "LGW", true)]
        public void TestIsAirportToAirport(string shortName1, string shortName2, bool expected)
        {
            Assert.AreEqual(expected, new Location(shortName1, "").IsAirportToAirport(new Location(shortName2, "")));
        }
    }
}