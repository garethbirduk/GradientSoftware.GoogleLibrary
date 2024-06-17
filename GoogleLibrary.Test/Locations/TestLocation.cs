using GoogleLibrary.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Locations
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