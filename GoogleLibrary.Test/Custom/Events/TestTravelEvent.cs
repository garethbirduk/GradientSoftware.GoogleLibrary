using GoogleLibrary.Custom.Events;
using GoogleLibrary.Custom.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestTravelEvent
    {
        [TestMethod]
        public void AddCustomSummary_ReturnsCorrectSummary()
        {
            var e = new TravelEvent();
            var summary = e.AddCustomSummary();

            Assert.AreEqual(1, summary.Count);
            Assert.AreEqual("", summary[0]);
        }

        [TestMethod]
        public void AddCustomSummary_ReturnsCorrectSummary1()
        {
            var e = new TravelEvent();
            e.Locations.Add(new Location("Home"));

            var summary = e.AddCustomSummary();

            Assert.AreEqual(1, summary.Count);
            Assert.AreEqual("Home", summary[0]);
        }

        [TestMethod]
        public void AddCustomSummary_ReturnsCorrectSummary2()
        {
            var e = new TravelEvent();
            e.Locations.Add(new Location("Home"));
            e.Locations.Add(new Location("Shops"));

            var summary = e.AddCustomSummary();

            Assert.AreEqual(1, summary.Count);
            Assert.AreEqual("Home - Shops", summary[0]);
        }

        [TestMethod]
        public void AddCustomSummary_ReturnsCorrectSummary3()
        {
            var e = new TravelEvent();
            e.Locations.Add(new Location("Home"));
            e.Locations.Add(new Location("Shops"));
            e.Locations.Add(new Location("Work"));

            var summary = e.AddCustomSummary();

            Assert.AreEqual(1, summary.Count);
            Assert.AreEqual("Home - Work", summary[0]);
        }
    }
}