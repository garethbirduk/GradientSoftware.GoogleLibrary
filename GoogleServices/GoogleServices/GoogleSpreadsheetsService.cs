using Google.Apis.Sheets.v4;

namespace GoogleServices.GoogleServices
{
    public class GoogleSpreadsheetsService : GoogleSpreadsheetsReadonlyService
    {
        public override IEnumerable<string> Scopes => new List<string>() { SheetsService.Scope.Spreadsheets };
    }
}