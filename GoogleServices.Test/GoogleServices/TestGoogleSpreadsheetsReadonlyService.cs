using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleSpreadsheetsReadonlyService
    {
        private static string SpreadsheetId { get; set; } = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";

        public static GoogleSpreadsheetsReadonlyService GoogleSpreadsheetsReadonlyService = new();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GoogleSpreadsheetsReadonlyService = new GoogleSpreadsheetsReadonlyService();
            GoogleSpreadsheetsReadonlyService.Initialize();
        }

        [TestMethod]
        public async Task TestGetSpreadsheet()
        {
            var spreadsheet = await GoogleSpreadsheetsReadonlyService.GetSpreadsheetAsync(SpreadsheetId);
            Assert.AreEqual(SpreadsheetId, spreadsheet.SpreadsheetId);
        }
    }
}