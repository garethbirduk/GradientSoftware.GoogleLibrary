using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace GoogleServices.GoogleServices
{
    /// <summary>
    /// Service to authenticate all scopes required by library interaction with Google APIs.
    /// </summary>
    public class GoogleAllScopesService : GoogleAuthorizationService
    {
        private GoogleCalendarsService GoogleCalendarsService { get; set; } = new();
        private CalendarService GoogleService { get; set; } = new();
        private GoogleSpreadsheetService GoogleSpreadsheetService { get; set; } = new();

        public static List<string> RequiredScopes = new List<string>()
        {
            CalendarService.Scope.Calendar,
            CalendarService.Scope.CalendarEvents,
            CalendarService.Scope.CalendarEventsReadonly,
            CalendarService.Scope.CalendarReadonly,
            CalendarService.Scope.CalendarSettingsReadonly,

            SheetsService.Scope.Drive,
            SheetsService.Scope.DriveFile,
            SheetsService.Scope.DriveReadonly,
            SheetsService.Scope.SpreadsheetsReadonly,
            SheetsService.Scope.Spreadsheets
        };

        public GoogleAllScopesService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
            GoogleCalendarsService.Initialize();
            GoogleSpreadsheetService.Initialize();
        }

        /// <summary>
        /// A dummy executable for ensuring the scopes get checked and requested if missing.
        /// </summary>
        public CalendarListEntry ExecuteSomething()
        {
            return GoogleCalendarsService.GetCalendar();
        }

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
        }
    }
}