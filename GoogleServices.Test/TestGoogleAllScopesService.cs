using GoogleServices.GoogleAuthentication;
using GoogleServices.Test.GoogleServices;

namespace GoogleServices.Test
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
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>(GoogleAllScopesService);
        }
    }
}