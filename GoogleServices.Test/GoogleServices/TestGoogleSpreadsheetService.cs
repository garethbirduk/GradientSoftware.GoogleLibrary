using GoogleServices.GoogleServices;
using Gradient.Utils;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleSpreadsheetService
    {
        private static string SpreadsheetId { get; set; } = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";

        public static GoogleSpreadsheetReadonlyService GoogleSpreadsheetReadonlyService = new();

        public static GoogleSpreadsheetService GoogleSpreadsheetService = new();

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext context)
        {
            GoogleSpreadsheetReadonlyService = new GoogleSpreadsheetReadonlyService();
            GoogleSpreadsheetReadonlyService.Initialize();
            GoogleSpreadsheetService = new GoogleSpreadsheetService();
            GoogleSpreadsheetService.Initialize();
        }

        [TestMethod]
        public async Task TestCreateWorksheetsAsync()
        {
            var name1 = StringHelpers.RandomName(prefix: "_deleteme_");
            var name2 = StringHelpers.RandomName(prefix: "_deleteme_");

            var worksheets = await GoogleSpreadsheetReadonlyService.GetWorksheets(SpreadsheetId);
            Assert.IsNull(worksheets.Where(x => x.Properties.Title == name1).FirstOrDefault());
            Assert.IsNull(worksheets.Where(x => x.Properties.Title == name2).FirstOrDefault());

            await GoogleSpreadsheetService.CreateWorksheetsAsync(SpreadsheetId, name1, name2);
            worksheets = await GoogleSpreadsheetReadonlyService.GetWorksheets(SpreadsheetId);
            Assert.IsNotNull(worksheets.Where(x => x.Properties.Title == name1).FirstOrDefault());
            Assert.IsNotNull(worksheets.Where(x => x.Properties.Title == name2).FirstOrDefault());

            await GoogleSpreadsheetService.DeleteWorksheetsAsync(SpreadsheetId, name1, name2);
            worksheets = await GoogleSpreadsheetReadonlyService.GetWorksheets(SpreadsheetId);
            Assert.IsNull(worksheets.Where(x => x.Properties.Title == name1).FirstOrDefault());
            Assert.IsNull(worksheets.Where(x => x.Properties.Title == name2).FirstOrDefault());
        }
    }
}