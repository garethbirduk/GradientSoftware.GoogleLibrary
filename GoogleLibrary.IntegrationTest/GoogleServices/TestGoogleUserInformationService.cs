using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.GoogleServices;
using GoogleLibrary.IntegrationTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GoogleLibrary.IntegrationTest.GoogleServices
{
    [TestClass]
    public class TestGoogleUserInformationService : GoogleAuthenticatedUnitTest
    {
        public GoogleUserInformationService Service { get; private set; }

        [TestMethod]
        public async Task TestGetUserInformation()
        {
            await Service.GetUserInformationAsync();
            Assert.IsTrue(Service.UserInformation.Contains("Gareth"));
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            var authenticator = new GoogleOAuthAuthenticatorHelper();
            await authenticator.SetupAuthAsync();
            Service = new GoogleUserInformationService(authenticator.Authenticator.GoogleOAuthAuthenticatedResponse);
        }
    }
}