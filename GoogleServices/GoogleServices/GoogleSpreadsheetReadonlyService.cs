using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleLibrary.GoogleExtensions;

namespace GoogleServices
{
    public class GoogleSpreadsheetReadonlyService : GoogleWebAuthorizationBrokeredScopedService
    {
        public SheetsService GoogleService { get; set; }

        public override IEnumerable<string> Scopes => new List<string>() { SheetsService.Scope.SpreadsheetsReadonly };

        public static string BuildRange(int columnStart, int rowStart, int? columnEnd = null, int? rowEnd = null, IndexBase indexBasis = IndexBase.Zero)
        {
            if (indexBasis == IndexBase.Zero)
            {
                rowStart++;
                if (rowEnd != null)
                    rowEnd++;
            }
            return BuildRange(columnStart.ToGoogleColumn(indexBasis), rowStart, columnEnd.ToGoogleColumn(indexBasis), rowEnd);
        }

        public static string BuildRange(string columnStart, int rowStart, string columnEnd = null, int? rowEnd = null)
        {
            if (string.IsNullOrWhiteSpace(columnEnd))
                columnEnd = columnStart;

            return $"{columnStart}{rowStart}:{columnEnd}{rowEnd}";
        }

        public async Task<ValueRange> GetData(string spreadsheetId, string worksheetName, int columnStart = 0, int rowStart = 0, int? columnEnd = null, int? rowEnd = null)
        {
            return await GetData(spreadsheetId, worksheetName, BuildRange(columnStart, rowStart, columnEnd, rowEnd));
        }

        public async Task<ValueRange> GetData(string spreadsheetId, string worksheetName, string worksheetRange)
        {
            var range = $"{worksheetName}";
            if (!string.IsNullOrWhiteSpace(worksheetRange))
                range = $"{worksheetName}!{worksheetRange}";
            return await GetData(spreadsheetId, range);
        }

        public async Task<ValueRange> GetData(string spreadsheetId, string range)
        {
            return await GoogleService.Spreadsheets.Values.Get(spreadsheetId, range).ExecuteAsync();
        }

        public async Task<Spreadsheet> GetSpreadsheetAsync(string spreadsheetId)
        {
            return await GoogleService.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
        }

        public async Task<ValueRange> GetWorksheetData(string spreadsheetId, string worksheetName)
        {
            return await GetData(spreadsheetId, worksheetName);
        }

        public async Task<List<Sheet>> GetWorksheets(string spreadsheetId)
        {
            return (await GetSpreadsheetAsync(spreadsheetId)).Sheets.ToList();
        }

        public override void SetupExternalServices()
        {
            GoogleService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = "Google Calender API v3",
            });
        }
    }
}