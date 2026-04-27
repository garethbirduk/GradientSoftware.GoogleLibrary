using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestAccomodationEvent
    {
        [TestMethod]
        public void Build_WithExplicitSummary_KeepsTitle()
        {
            var e = new AccommodationEvent();
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new("Summary", EnumEventFieldType.Summary),
                new("From", EnumEventFieldType.From),
            };
            var data = new List<string> { "Hotel Hilton", "Heathrow" };

            e.Build(fields, data);

            Assert.AreEqual("Hotel Hilton", e.Title);
        }

        [TestMethod]
        public void Build_WithEmptySummary_FallsBackToFirstLocationName()
        {
            var e = new AccommodationEvent();
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new("Summary", EnumEventFieldType.Summary),
                new("From", EnumEventFieldType.From),
            };
            var data = new List<string> { "", "Hotel Hilton" };

            e.Build(fields, data);

            Assert.AreEqual("Hotel Hilton", e.Title);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("   ")]
        public void Build_WithNoSummaryAndNoLocation_FallsBackToTBC(string summary)
        {
            var e = new AccommodationEvent();
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new("Summary", EnumEventFieldType.Summary),
            };
            var data = new List<string> { summary };

            e.Build(fields, data);

            Assert.AreEqual("TBC", e.Title);
        }

        [TestMethod]
        public void DefaultRemindersInMinutes_Is12HoursBeforeEvent()
        {
            var e = new AccommodationEvent();
            CollectionAssert.AreEqual(new List<int> { 12 * 60 }, e.DefaultRemindersInMinutes);
        }
    }
}
