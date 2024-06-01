using Google;
using Google.Apis.Sheets.v4.Data;
using GoogleLibrary.EventsServices;
using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.GoogleServices;
using GoogleLibrary.IntegrationTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLibrary.IntegrationTest.other
{
    [TestClass]
    public class TestGoogleSpreadsheetReadonlyService : GoogleAuthenticatedUnitTest
    {
        [DataTestMethod]
        [DataRow(0, 0, null, null, IndexBase.Zero, "A1:A")]
        [DataRow(1, 0, null, null, IndexBase.Zero, "B1:B")]
        [DataRow(0, 1, null, null, IndexBase.Zero, "A2:A")]
        [DataRow(1, 1, null, null, IndexBase.Zero, "B2:B")]
        [DataRow(1, 1, null, null, IndexBase.One, "A1:A")]
        [DataRow(2, 1, null, null, IndexBase.One, "B1:B")]
        [DataRow(1, 2, null, null, IndexBase.One, "A2:A")]
        [DataRow(2, 2, null, null, IndexBase.One, "B2:B")]
        public void TestBuildRange_ByInt(int columnStart, int rowStart, int? columnEnd, int? rowEnd, IndexBase indexBase, string expected)
        {
            Assert.AreEqual(expected, GoogleSpreadsheetReadonlyService.BuildRange(columnStart, rowStart, columnEnd, rowEnd, indexBase));
        }

        [DataTestMethod]
        [DataRow("A", 1, null, null, "A1:A")]
        [DataRow("B", 1, null, null, "B1:B")]
        [DataRow("A", 2, null, null, "A2:A")]
        [DataRow("B", 2, null, null, "B2:B")]
        public void TestBuildRange_ByString(string columnStart, int rowStart, string? columnEnd, int? rowEnd, string expected)
        {
            Assert.AreEqual(expected, GoogleSpreadsheetReadonlyService.BuildRange(columnStart, rowStart, columnEnd, rowEnd));
        }

        [TestMethod]
        public async Task TestGetData()
        {
            var data = await GoogleSpreadsheetReadonlyService.GetData(SpreadsheetId, "TestGetData", 0, 0, 7, null);
            Assert.AreEqual(10, data.Values.Count());
        }

        [TestMethod]
        public async Task TestGetData_WorksheetNameException()
        {
            Assert.IsTrue((await GoogleSpreadsheetReadonlyService.GetWorksheetData(SpreadsheetId, "USA2024")).Range.Count() > 0);
            await Assert.ThrowsExceptionAsync<GoogleApiException>(() => GoogleSpreadsheetReadonlyService.GetData(SpreadsheetId, "USA2024"));
        }

        [DataTestMethod]
        [DataRow("TestGetData!A1:A", 10)]
        [DataRow("TestGetData!A1:A10", 10)]
        [DataRow("TestGetData!A:A", 10)]
        [DataRow("TestGetData!A2:A", 9)]
        [DataRow("TestGetData!B1:B", 10)]
        [DataRow("TestGetData!B1:B10", 10)]
        [DataRow("TestGetData!B:B", 10)]
        [DataRow("TestGetData!B2:B", 9)]
        [DataRow("TestGetData!A1:B", 10, 2)]
        [DataRow("TestGetData!A1:B10", 10, 2)]
        [DataRow("TestGetData!A:B", 10, 2)]
        [DataRow("TestGetData!A2:B", 9, 2)]
        public async Task TestGetData2(string range, int expectedRows, int expectedColumns = 1)
        {
            var data = await GoogleSpreadsheetReadonlyService.GetData(SpreadsheetId, range);
            Assert.AreEqual(expectedRows, data.Values.Count);
            Assert.AreEqual(expectedColumns, data.Values.First().Count);
        }

        [DataTestMethod]
        [DataRow("", 10, 2)]
        [DataRow("A1:A", 10)]
        [DataRow("A1:A10", 10)]
        [DataRow("A:A", 10)]
        [DataRow("A2:A", 9)]
        [DataRow("B1:B", 10)]
        [DataRow("B1:B10", 10)]
        [DataRow("B:B", 10)]
        [DataRow("B2:B", 9)]
        [DataRow("A1:B", 10, 2)]
        [DataRow("A1:B10", 10, 2)]
        [DataRow("A:B", 10, 2)]
        [DataRow("A2:B", 9, 2)]
        public async Task TestGetData3(string range, int expectedRows, int expectedColumns = 1)
        {
            var data = await GoogleSpreadsheetReadonlyService.GetData(SpreadsheetId, "TestGetData", range);
            Assert.AreEqual(expectedRows, data.Values.Count);
            Assert.AreEqual(expectedColumns, data.Values.Select(x => x.Count()).Max());
        }

        [TestMethod]
        public async Task TestGetWorksheetData()
        {
            var data = await GoogleSpreadsheetReadonlyService.GetWorksheetData(SpreadsheetId, "TestGetData");
            Assert.AreEqual(10, data.Values.Count());
            Assert.AreEqual(2, data.Values.Select(x => x.Count()).Max());
        }

        [TestMethod]
        public async Task TestGetWorksheets()
        {
            var worksheets = await GoogleSpreadsheetReadonlyService.GetWorksheets(SpreadsheetId);
            Assert.IsTrue(worksheets.Where(x => x.Properties.Title == "TestGetData").Count() > 0);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleSpreadsheetReadonlyService, GoogleCalendarService);
            CustomSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
        }
    }
}