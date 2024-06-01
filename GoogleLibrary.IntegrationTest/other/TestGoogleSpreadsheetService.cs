using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4.Data;
using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestGoogleSpreadsheetService : GoogleAuthenticatedUnitTest
    {
        [TestMethod]
        public async Task TestCreateWorksheetsAsync()
        {
            var name1 = Utils.RandomName(prefix: "_deleteme_");
            var name2 = Utils.RandomName(prefix: "_deleteme_");

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

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleSpreadsheetService, GoogleSpreadsheetReadonlyService);
        }
    }
}