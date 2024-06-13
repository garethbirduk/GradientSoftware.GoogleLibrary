using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.Collections.Generic;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestFightEvent
    {
        [TestMethod]
        public void Test1()
        {
            var e = new FlightEvent();
            Assert.IsNotNull(e);
        }
    }
}