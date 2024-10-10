using Google.Apis.Services;
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
            .Union(GoogleCalendarsService.RequiredScopes)
            .ToList();

        public CustomSpreadsheetService() : base(RequiredScopes)
        {
            GoogleSpreadsheetReadonlyService.Initialize();
            GoogleCalendarService.Initialize();
            GoogleCalendarsService.Initialize();
        }

        public GoogleCalendarService GoogleCalendarService { get; } = new();
        public GoogleCalendarsService GoogleCalendarsService { get; } = new();
        public GoogleSpreadsheetReadonlyService GoogleSpreadsheetReadonlyService { get; } = new();

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
        }

        public async Task WorksheetToCalendarAsync(string spreadsheetId, string name, int headerRowsCount = 1, int maxEvents = 0)
        {
            var calendarId = (await GoogleCalendarsService.CreateOrGetCalendarAsync(name, true)).Id;
            await WorksheetToCalendarAsync(spreadsheetId, name, calendarId, headerRowsCount, maxEvents);
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