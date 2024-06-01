using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.IntegrationTest
{
    [TestClass]
    public class TestGoogleAllScopesService : GoogleAuthenticatedUnitTest
    {
        [TestMethod]
        public void TestExecuteSomething()
        {
            GoogleAllScopesService.ExecuteSomething();
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(GoogleAllScopesService);
        }
    }
}