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
    }
}