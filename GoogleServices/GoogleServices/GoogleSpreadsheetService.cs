using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleLibrary.GoogleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleServices
{
    public class GoogleSpreadsheetService : GoogleSpreadsheetReadonlyService
    {
        /// <summary>
        /// It's a batch request so you can create more than one request and send them all in one
        /// batch.Just use reqs.Requests.Add() to add additional requests for the same
        /// spreadsheet requests.Requests.Add(request);
        /// </summary>
        /// <param name="requests"></param>
        private async Task BatchUpdateAsync(string spreadsheetId, params Request[] requests)
        {
            await GoogleService.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
            {
                Requests = requests
            },
            spreadsheetId).ExecuteAsync();
        }

        public override IEnumerable<string> Scopes => new List<string>() { SheetsService.Scope.Spreadsheets };

        public async Task AddCells(string spreadsheetId, int worksheetId, GoogleSheetParameters googleSheetParameters, List<GoogleSheetRow> rows)
        {
            var requests = new List<Request>();

            var request = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = googleSheetParameters.ToGridCoordinate(worksheetId),
                    Fields = "*"
                }
            };

            var listRowData = new List<RowData>();

            foreach (var row in rows)
            {
                var rowData = new RowData();
                var listCellData = new List<CellData>();
                foreach (var cell in row.Cells)
                {
                    var cellData = new CellData();
                    var extendedValue = new ExtendedValue
                    {
                        StringValue = cell.CellValue
                    };

                    cellData.UserEnteredValue = extendedValue;
                    var cellFormat = new CellFormat
                    {
                        TextFormat = new TextFormat()
                    };

                    if (cell.IsBold)
                        cellFormat.TextFormat.Bold = true;

                    cellFormat.BackgroundColor = new Color
                    {
                        Blue = (float)cell.BackgroundColor.B / 255,
                        Red = (float)cell.BackgroundColor.R / 255,
                        Green = (float)cell.BackgroundColor.G / 255
                    };

                    cellData.UserEnteredFormat = cellFormat; listCellData.Add(cellData);
                }
                rowData.Values = listCellData;
                listRowData.Add(rowData);
            }
            request.UpdateCells.Rows = listRowData;
            requests.Add(request);
            await BatchUpdateAsync(spreadsheetId, requests.ToArray());
        }

        public async Task<Dictionary<int, string>> CreateOrGetWorksheetsAsync(string spreadsheetId, params string[] worksheetNames)
        {
            var dictionary = new Dictionary<int, string>();
            var requests = new List<Request>();
            foreach (var worksheetName in worksheetNames.Except((await GetWorksheets(spreadsheetId)).Select(x => x.Properties.Title)))
            {
                var request = new AddSheetRequest
                {
                    Properties = new SheetProperties()
                    {
                        Title = worksheetName,
                    }
                };
                requests.Add(new Request
                {
                    AddSheet = request
                });
            }
            if (requests.Any())
                await BatchUpdateAsync(spreadsheetId, requests.ToArray());
            var worksheets = await GetWorksheets(spreadsheetId);
            foreach (var worksheetName in worksheetNames)
            {
                var worksheet = worksheets.Where(x => x.Properties.Title == worksheetName).Single();
                dictionary.Add(worksheet.Properties.SheetId.Value, worksheetName);
            }
            return dictionary;
        }

        public async Task<Dictionary<int, string>> CreateWorksheetsAsync(string spreadsheetId, params string[] worksheetNames)
        {
            if (worksheetNames.Distinct().Count() != worksheetNames.Length)
                throw new NotSupportedException("New worksheet names must be unique");

            var worksheets = await GetWorksheets(spreadsheetId);
            if (worksheetNames.Union(worksheets.Select(x => x.Properties.Title)).Distinct().Count() !=
                worksheetNames.Union(worksheets.Select(x => x.Properties.Title)).Count())
                throw new NotSupportedException("Union of new worksheet names and existing worksheet names must be unique");

            return await CreateOrGetWorksheetsAsync(spreadsheetId, worksheetNames);
        }

        public async Task DeleteWorksheetsAsync(string spreadsheetId, params string[] worksheetNames)
        {
            var worksheetIds = (await GetWorksheets(spreadsheetId)).Where(x => worksheetNames.Contains(x.Properties.Title)).Select(x => x.Properties.SheetId);
            await DeleteWorksheetsAsync(spreadsheetId, worksheetIds.ToArray());
        }

        public async Task DeleteWorksheetsAsync(string spreadsheetId, params int?[] worksheetIds)
        {
            var requests = new List<Request>();
            foreach (var worksheetIndex in worksheetIds)
            {
                var request = new DeleteSheetRequest()
                {
                    SheetId = worksheetIndex
                };
                requests.Add(new Request
                {
                    DeleteSheet = request
                });
            }
            await BatchUpdateAsync(spreadsheetId, requests.ToArray());
        }
    }
}