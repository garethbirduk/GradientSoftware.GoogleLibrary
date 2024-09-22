using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace GoogleServices.GoogleServices
{
    public class GoogleSpreadsheetsReadonlyService : GoogleWebAuthorizationBrokeredScopedService
    {
        public SheetsService GoogleService { get; set; }

        public override IEnumerable<string> Scopes => new List<string>() { SheetsService.Scope.SpreadsheetsReadonly };

        public async Task<Spreadsheet> GetSpreadsheetAsync(string spreadsheetId)
        {
            return await GoogleService.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
        }

        public async Task<Spreadsheet> GetSpreadsheetsAsync()
        {
            await Task.CompletedTask;
            throw new NotSupportedException();
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