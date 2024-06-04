using Google.Apis.Sheets.v4.Data;
using GoogleLibrary.GoogleServices;
using GoogleLibrary.GoogleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLibrary.EventsServices
{
    public class CustomCalendarService
    {
        public CustomCalendarService(GoogleCalendarReadonlyService googleCalendarReadonlyService, GoogleSpreadsheetService googleSpreadsheetService)
        {
            GoogleCalendarReadonlyService = googleCalendarReadonlyService;
            GoogleSpreadsheetService = googleSpreadsheetService;
        }

        public GoogleCalendarReadonlyService GoogleCalendarReadonlyService { get; }
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; }

        public async Task CalendarToWorksheetAsync(string calendarId, string spreadsheetId, string worksheetName = "", int maxResults = 100)
        {
            var events = GoogleCalendarReadonlyService.GetEvents(calendarId, maxResults, DateTime.MinValue);

            if (worksheetName == "")
                worksheetName = calendarId;

            var description = GoogleCalendarReadonlyService.Calendar(calendarId).Description;
            var header = description.Split("\r\n").Where(x => x.StartsWith("Headers")).FirstOrDefault();
            if (header != null)
            {
                var headers = header.Split("*/*").Skip(1).ToList();
            }

            var worksheet = (await GoogleSpreadsheetService.CreateWorksheetsAsync(spreadsheetId, worksheetName)).Single();

            var googleSheetParameters = new GoogleSheetParameters(0, 0, IndexBase.Zero);
            var rows = new List<GoogleSheetRow>();
            foreach (var myEvent in events.Items)
            {
                var row = new GoogleSheetRow();
                row.Cells.Add(new GoogleSheetCell()
                {
                    CellValue = myEvent.Summary
                });
                rows.Add(row);
            }

            await GoogleSpreadsheetService.AddCells(spreadsheetId, worksheet.Key, googleSheetParameters, rows);
        }
    }
}