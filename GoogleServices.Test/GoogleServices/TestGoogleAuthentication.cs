using GoogleServices.GoogleAuthentication;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleOAuthAuthenticator : GoogleAuthenticatedUnitTest
    {
        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>();
        }

        [TestMethod]
        public async Task TestLogin()
        {
            await Task.CompletedTask;
            Assert.IsTrue(true);
        }
    }
}