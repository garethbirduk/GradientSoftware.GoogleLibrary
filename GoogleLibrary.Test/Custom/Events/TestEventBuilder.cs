using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestEventBuilder
    {
        [TestMethod]
        public void TestCreate()
        {
            var fields = new List<Tuple<string, EnumEventFieldType>>()
            {
                new("Summary", EnumEventFieldType.Summary),
            };
            var data = new List<string>()
            {
                "MySummary"
            };
            Assert.IsNotNull(EventBuilder.Create(fields, data));
        }
    }
}