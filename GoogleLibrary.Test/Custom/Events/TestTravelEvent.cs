using GoogleLibrary.Custom.Events;
using GoogleLibrary.Custom.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestTravelEvent
    {
        [TestMethod]
        public void RouteSummary_Empty_WhenNoLocations()
        {
            var e = new TravelEvent();
            Assert.AreEqual("", e.RouteSummary);
        }

        [TestMethod]
        public void RouteSummary_SingleLocation_ReturnsName()
        {
            var e = new TravelEvent();
            e.Locations.Add(new Location("Home"));
            Assert.AreEqual("Home", e.RouteSummary);
        }

        [TestMethod]
        public void RouteSummary_TwoLocations_ReturnsFromTo()
        {
            var e = new TravelEvent();
            e.Locations.Add(new Location("Home"));
            e.Locations.Add(new Location("Shops"));
            Assert.AreEqual("Home - Shops", e.RouteSummary);
        }

        [TestMethod]
        public void RouteSummary_ManyLocations_FirstAndLast()
        {
            var e = new TravelEvent();
            e.Locations.Add(new Location("Home"));
            e.Locations.Add(new Location("Shops"));
            e.Locations.Add(new Location("Work"));
            Assert.AreEqual("Home - Work", e.RouteSummary);
        }

        [TestMethod]
        public void Build_WithExplicitSummary_KeepsTitle()
        {
            var e = new TravelEvent();
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new("Summary", EnumEventFieldType.Summary),
                new("From", EnumEventFieldType.From),
                new("To", EnumEventFieldType.To),
            };
            var data = new List<string> { "School run", "Home", "School" };

            e.Build(fields, data);

            Assert.AreEqual("School run", e.Title);
        }

        [TestMethod]
        public void Build_WithEmptySummary_FallsBackToRouteSummary()
        {
            var e = new TravelEvent();
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new("Summary", EnumEventFieldType.Summary),
                new("From", EnumEventFieldType.From),
                new("To", EnumEventFieldType.To),
            };
            var data = new List<string> { "", "Home", "School" };

            e.Build(fields, data);

            Assert.AreEqual("Home - School", e.Title);
        }
    }
}
