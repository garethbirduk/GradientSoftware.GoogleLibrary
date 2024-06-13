﻿using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestLocation
    {
        [TestMethod]
        public void TestCreateHome()
        {
            var location = new Location("home");
            Assert.AreEqual("Home", location.ShortName);
            Assert.AreEqual("10 Bourne Close, Beeston, NG9 3BZ, UK", location.Address);
        }
    }
}