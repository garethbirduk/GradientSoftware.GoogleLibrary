using GoogleLibrary.CustomServices;
using GoogleLibrary.GoogleAuthentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GoogleLibrary.Test
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