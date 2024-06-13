using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestFieldsExtensions
    {
        [DataTestMethod]
        [DataRow("drive", EventCategory.Drive, false)]
        [DataRow("drive", EventCategory.Drive, true)]
        [DataRow("car", EventCategory.Unknown, false)]
        [DataRow("car", EventCategory.Drive, true)]
        public void TestGetEnumOrDefault(string categoryData, EventCategory expected, bool allowAlias = false)
        {
            var fields = new List<Tuple<string, EnumEventFieldType>>()
            {
                new("Summary", EnumEventFieldType.Summary),
                new("EventType", EnumEventFieldType.Category),
            };
            var data = new List<string>()
            {
                "MySummary",
                categoryData
            };
            var category = fields.GetEnumOrDefault<EnumEventFieldType, EventCategory>(EnumEventFieldType.Category, data, allowAlias);
            Assert.AreEqual(expected, category);
        }

        [TestMethod]
        public void TestGetStringOrDefault()
        {
            var fields = new List<Tuple<string, EnumEventFieldType>>()
            {
                new("Summary", EnumEventFieldType.Summary),
            };
            var data = new List<string>()
            {
                "MySummary",
            };
            var value = fields.GetStringOrDefault(EnumEventFieldType.Summary, data);
            Assert.AreEqual("MySummary", value);
        }
    }
}