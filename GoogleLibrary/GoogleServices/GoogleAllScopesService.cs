using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System.Collections.Generic;

namespace GoogleLibrary.GoogleServices
{
    /// <summary>
    /// Service to authenticate all scopes required by library interaction with Google APIs.
    /// </summary>
    public class GoogleAllScopesService : GoogleWebAuthorizationBrokeredScopedService
    {
        private CalendarService GoogleService { get; set; }

        public override List<string> Scopes => new()
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

        /// <summary>
        /// A dummy executable for ensuring the scopes get checked and requested if missing.
        /// </summary>
        public void ExecuteSomething()
        {
            GoogleService.CalendarList.List().Execute();
        }

        public override void SetupExternalServices()
        {
            GoogleService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = "Google Calender API v3",
            });
        }
    }
}