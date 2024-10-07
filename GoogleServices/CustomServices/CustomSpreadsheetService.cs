using GoogleLibrary.Custom.Events;
using GoogleLibrary.GoogleEventBuilders;
using GoogleServices.GoogleServices;

namespace GoogleServices.CustomServices
{
    public class CustomSpreadsheetService : GoogleAuthorizationService
    {
        public static List<string> RequiredScopes =
            GoogleSpreadsheetReadonlyService.RequiredScopes
            .Union(GoogleCalendarService.RequiredScopes)
            .ToList();

        public CustomSpreadsheetService(GoogleSpreadsheetReadonlyService googleSpreadsheetReadonlyService, GoogleCalendarService googleCalendarService) : base(RequiredScopes)
        {
            GoogleSpreadsheetReadonlyService = googleSpreadsheetReadonlyService;
            GoogleCalendarService = googleCalendarService;
        }

        public GoogleCalendarService GoogleCalendarService { get; }
        public GoogleSpreadsheetReadonlyService GoogleSpreadsheetReadonlyService { get; }

        public override void SetupExternalServices()
        {
            var xx = 1;
        }

        public async Task WorksheetToCalendarAsync(string spreadsheetId, string worksheetName, string calendarId, int headerRowsCount = 1, int maxEvents = 0)
        {
            var valueRange = await GoogleSpreadsheetReadonlyService.GetData(spreadsheetId, worksheetName, "");

            var headers = valueRange.Values.Take(headerRowsCount).Last().Select(x => x.ToString());
            var data = valueRange.Values.Skip(headerRowsCount).Select(x => x.Select(y => y.ToString()).ToList());

            var events = EventsBuilder.Create(headers, data);
            var googleEvents = GoogleEventsBuilder.Create(events.ToArray());
            if (maxEvents > 0)
                googleEvents = googleEvents.Take(maxEvents).ToList();
            GoogleCalendarService.CreateEvents(calendarId, googleEvents);
            GoogleCalendarService.SetDescription(calendarId, worksheetName);
        }
    }
}