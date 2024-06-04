using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.IntegrationTest.GoogleServices
{
    [TestClass]
    public class TestGoogleOAuthAuthenticator : GoogleAuthenticatedUnitTest
    {
        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync();
        }

        [TestMethod]
        public async Task TestLogin()
        {
            await Task.CompletedTask;
            Assert.IsTrue(true);
        }
    }
}