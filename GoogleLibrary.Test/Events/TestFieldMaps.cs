using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestFieldMaps
    {
        [TestMethod]
        public void TestEventTypes()
        {
            var types = FieldMaps.EventTypes("Summary", "Title", "Date", "Start");
            Assert.IsNotNull(types);

            Assert.AreEqual("Summary", types[0].Item1);
            Assert.AreEqual(EnumEventFieldType.Summary, types[0].Item2);

            Assert.AreEqual("Title", types[1].Item1);
            Assert.AreEqual(EnumEventFieldType.Summary, types[1].Item2);

            Assert.AreEqual("Date", types[2].Item1);
            Assert.AreEqual(EnumEventFieldType.StartDate, types[2].Item2);

            Assert.AreEqual("Start", types[3].Item1);
            Assert.AreEqual(EnumEventFieldType.From, types[3].Item2);
        }
    }
}