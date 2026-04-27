using GoogleLibrary.Custom.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.Custom.Locations
{
    [TestClass]
    public class TestLocation
    {
        [TestInitialize]
        public void TestInitialize() => KnownLocations.Clear();

        [TestCleanup]
        public void TestCleanup() => KnownLocations.Clear();

        [TestMethod]
        public void Construct_WhenNameNotRegistered_PreservesInput()
        {
            var location = new Location("home");
            Assert.AreEqual("home", location.ShortName);
            Assert.AreEqual("home", location.Address);
        }

        [TestMethod]
        public void Construct_WhenNameRegistered_ResolvesToCanonicalAndAddress()
        {
            KnownLocations.Register("Home", "1 Test Street, Testville");

            var location = new Location("home");

            Assert.AreEqual("Home", location.ShortName);
            Assert.AreEqual("1 Test Street, Testville", location.Address);
        }

        [TestMethod]
        public void Construct_WithExplicitAddress_AddressTakesPrecedenceOverInput()
        {
            var location = new Location("Some Place", "123 Real Address");
            Assert.AreEqual("Some Place", location.ShortName);
            Assert.AreEqual("123 Real Address", location.Address);
        }
    }
}
