using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestGoogleSpreadsheetsReadonlyService : GoogleAuthenticatedUnitTest
    {
        [TestMethod]
        public async Task TestGetSpreadsheet()
        {
            var spreadsheet = await GoogleSpreadsheetsReadonlyService.GetSpreadsheetAsync(SpreadsheetId);
            Assert.AreEqual(SpreadsheetId, spreadsheet.SpreadsheetId);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleSpreadsheetsReadonlyService);
        }
    }
}