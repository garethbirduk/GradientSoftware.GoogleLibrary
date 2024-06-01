using PostSharp.Patterns.Contracts;
using System.Linq;
using System.Threading.Tasks;
using GoogleLibrary.Custom;
using Microsoft.Extensions.DependencyInjection;
using GoogleLibrary.GoogleServices;

namespace GoogleLibrary.CustomServices
{
    public class CustomSpreadsheetService
    {
        public CustomSpreadsheetService(GoogleSpreadsheetReadonlyService googleSpreadsheetReadonlyService, GoogleCalendarService googleCalendarService)
        {
            GoogleSpreadsheetReadonlyService = googleSpreadsheetReadonlyService;
            GoogleCalendarService = googleCalendarService;
        }

        public GoogleCalendarService GoogleCalendarService { get; }
        public GoogleSpreadsheetReadonlyService GoogleSpreadsheetReadonlyService { get; }

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