using Google.Apis.Sheets.v4;

namespace GoogleServices.GoogleServices
{
    public class GoogleSpreadsheetsService : GoogleSpreadsheetsReadonlyService
    {
        public static List<string> RequiredScopes = new List<string>()
            { SheetsService.Scope.Spreadsheets };

        public GoogleSpreadsheetsService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }
    }
}