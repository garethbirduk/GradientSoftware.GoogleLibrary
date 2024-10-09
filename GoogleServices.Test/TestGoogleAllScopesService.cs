using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test
{
    [TestClass]
    public class TestGoogleAllScopesService : GoogleAuthenticatedUnitTest
    {
        [TestMethod]
        public void TestExecuteSomething()
        {
            Assert.AreEqual("primary", GoogleAllScopesService.ExecuteSomething().Summary);
        }
    }
}