using Google.Apis.Services;
using GoogleLibrary.Custom.Events;
using GoogleLibrary.Custom.Maths;
using GoogleLibrary.GoogleEventBuilders;
using GoogleLibrary.GoogleSheets;
using GoogleServices.GoogleServices;

namespace GoogleServices.CustomServices
{
    public class CustomSpreadsheetService : GoogleAuthorizationService
    {
        public static IReadOnlyList<string> RequiredScopes =
            GoogleSpreadsheetService.RequiredScopes
            .Union(GoogleCalendarService.RequiredScopes)
            .Union(GoogleCalendarsService.RequiredScopes)
            .ToList();

        public CustomSpreadsheetService() : base(RequiredScopes)
        {
            GoogleSpreadsheetService.Initialize();
            GoogleCalendarService.Initialize();
            GoogleCalendarsService.Initialize();
        }

        public GoogleCalendarService GoogleCalendarService { get; } = new();
        public GoogleCalendarsService GoogleCalendarsService { get; } = new();
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; } = new();

        public async Task CalendarToWorksheetAsync(string calendarId, string spreadsheetId, string worksheetName = "", int maxResults = 100)
        {
            var events = GoogleCalendarService.GetEvents(calendarId, maxResults, DateTime.MinValue);

            if (worksheetName == "")
                worksheetName = calendarId;

            var description = GoogleCalendarService.Calendar(calendarId).Description;
            var headers = WorksheetCalendarMapping.TryDecodeHeaders(description, out var decoded)
                ? decoded
                : WorksheetCalendarMapping.DefaultHeaders.ToList();

            var worksheet = (await GoogleSpreadsheetService.CreateWorksheetsAsync(spreadsheetId, worksheetName)).Single();

            var googleSheetParameters = new GoogleSheetParameters(0, 0, IndexBase.Zero);
            var rows = new List<GoogleSheetRow>
            {
                BuildRow(headers, isHeader: true)
            };
            foreach (var googleEvent in events.Items)
            {
                var basicEvent = EventBuilder.Create(googleEvent);
                var values = WorksheetCalendarMapping.ToCellValues(basicEvent, headers);
                rows.Add(BuildRow(values, isHeader: false));
            }

            await GoogleSpreadsheetService.AddCells(spreadsheetId, worksheet.Key, googleSheetParameters, rows);
        }

        private static GoogleSheetRow BuildRow(IEnumerable<string> values, bool isHeader)
        {
            var row = new GoogleSheetRow();
            foreach (var value in values)
            {
                row.Cells.Add(new GoogleSheetCell { CellValue = value, IsBold = isHeader });
            }
            return row;
        }

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
            var valueRange = await GoogleSpreadsheetService.GetData(spreadsheetId, worksheetName, "");

            var headers = valueRange.Values.Take(headerRowsCount).Last().Select(x => x?.ToString() ?? "").ToList();
            var data = valueRange.Values.Skip(headerRowsCount).Select(x => x.Select(y => y?.ToString() ?? "").ToList());

            var events = EventsBuilder.Create(headers, data);
            var googleEvents = GoogleEventsBuilder.Create(events.ToArray());
            if (maxEvents > 0)
                googleEvents = googleEvents.Take(maxEvents).ToList();
            GoogleCalendarService.CreateEvents(calendarId, googleEvents);

            // Persist the original column ordering so CalendarToWorksheetAsync can round-trip.
            var description = $"{worksheetName}\r\n{WorksheetCalendarMapping.EncodeHeaders(headers)}";
            GoogleCalendarService.SetDescription(calendarId, description);
        }
    }
}