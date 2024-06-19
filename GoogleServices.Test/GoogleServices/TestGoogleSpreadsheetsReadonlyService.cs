using GoogleServices.GoogleAuthentication;

namespace GoogleServices.Test.GoogleServices
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
            await GoogleOAuthAuthenticatorHelper.CreateAsync<GoogleAuthenticatedUnitTest>(
                GoogleSpreadsheetsReadonlyService);
        }
    }
}