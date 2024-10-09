using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace GoogleServices.GoogleServices
{
    public class GoogleSpreadsheetsReadonlyService : GoogleAuthorizationService
    {
        public static List<string> RequiredScopes = new List<string>()
            { SheetsService.Scope.SpreadsheetsReadonly };

        public GoogleSpreadsheetsReadonlyService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }

        public SheetsService GoogleService { get; set; }

        public async Task<Spreadsheet> GetSpreadsheetAsync(string spreadsheetId)
        {
            return await GoogleService.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
        }

        public async Task<Spreadsheet> GetSpreadsheetsAsync()
        {
            await Task.CompletedTask;
            throw new NotSupportedException();
        }

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
            GoogleService = new SheetsService(initializer);
        }
    }
}