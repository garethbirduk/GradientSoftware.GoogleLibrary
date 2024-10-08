﻿using GoogleLibrary.Custom.Maths;
using GoogleLibrary.GoogleSheets;
using GoogleServices.GoogleServices;

namespace GoogleServices.CustomServices
{
    public class CustomCalendarService
    {
        public GoogleCalendarReadonlyService GoogleCalendarReadonlyService { get; } = new();
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; } = new();

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