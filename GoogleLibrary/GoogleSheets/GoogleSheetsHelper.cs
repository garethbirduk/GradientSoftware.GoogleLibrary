//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Sheets.v4;
//using Google.Apis.Sheets.v4.Data;
//using Google.Apis.Services;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Dynamic;
//using GoogleLibrary.GoogleSheets;

//namespace GoogleLibrary
//{
//    public class GoogleSheetsHelper
//    {
//        public GoogleSheetsHelper(string credentialFileName, string spreadsheetId)
//        {
//            var credential = GoogleCredential.FromStream(new FileStream(credentialFileName, FileMode.Open)).CreateScoped(Scopes);

//            _sheetsService = new GoogleService(new BaseClientService.Initializer()
//            {
//                HttpClientInitializer = credential,
//                ApplicationName = ApplicationName,
//            });

//            _spreadsheetId = spreadsheetId;
//        }

//        public GoogleSheetsHelper(string spreadsheetId, GoogleSpreadsheetsService
//        googleSheetsService)
//        {
//            _spreadsheetId = spreadsheetId; GoogleSpreadsheetsService =
//        googleSheetsService;
//        }

//        public void AddCells(GoogleSheetParameters googleSheetParameters, List<GoogleSheetRow> rows)
//        {
//            var requests = new BatchUpdateSpreadsheetRequest { Requests = new List<Request>() };

//            var sheetId = GetSheetId(_sheetsService, _spreadsheetId, googleSheetParameters.SheetName);

//            GridCoordinate gc = new GridCoordinate
//            {
//                ColumnIndex = googleSheetParameters.RangeColumnStart
//            - 1,
//                RowIndex = googleSheetParameters.RangeRowStart - 1,
//                SheetId = sheetId
//            };

//            var request = new Request
//            {
//                UpdateCells = new UpdateCellsRequest
//                {
//                    Start = gc,
//                    Fields =
//            "*"
//                }
//            };

//            var listRowData = new List<RowData>();

//            foreach (var row in rows)
//            {
//                var rowData = new RowData(); var listCellData = new List<CellData>();
//                foreach (var cell in row.Cells)
//                {
//                    var cellData = new CellData();
//                    var extendedValue = new ExtendedValue { StringValue = cell.CellValue };

//                    cellData.UserEnteredValue = extendedValue; var cellFormat = new CellFormat
//                    {
//                        TextFormat = new TextFormat()
//                    };

//                    if (cell.IsBold) { cellFormat.TextFormat.Bold = true; }

//                    cellFormat.BackgroundColor = new Color
//                    {
//                        Blue = (float)cell.BackgroundColor.B / 255,
//                        Red = (float)cell.BackgroundColor.R / 255,
//                        Green = (float)cell.BackgroundColor.G / 255
//                    };

//                    cellData.UserEnteredFormat = cellFormat; listCellData.Add(cellData);
//                }
//                rowData.Values = listCellData; listRowData.Add(rowData);
//            }
//            request.UpdateCells.Rows = listRowData;

//            // It's a batch request so you can create more than one request and send them all in one
//            // batch.Just use reqs.Requests.Add() to add additional requests for the same
//            // spreadsheet requests.Requests.Add(request);

//            _sheetsService.Spreadsheets.BatchUpdateData(requests, _spreadsheetId).Execute();
//        }

//        public List<ExpandoObject> GetDataFromSheet(GoogleSheetParameters googleSheetParameters)
//        {
//            googleSheetParameters =
//        MakeGoogleSheetDataRangeColumnsZeroBased(googleSheetParameters); var range = $"{googleSheetParameters.SheetName}!{GetColumnName(googleSheetParameters.RangeColumnStart)}{googleSheetParameters.RangeRowStart}:{GetColumnName(googleSheetParameters.RangeColumnEnd)}{googleSheetParameters.RangeRowEnd}";

//            SpreadsheetsResource.ValuesResource.GetRequest request;

//            if (GoogleSpreadsheetsService != null) request =
//            GoogleSpreadsheetsService.GoogleService.Spreadsheets.Values.Get(_spreadsheetId, range);
//            else request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);

//            var numberOfColumns = googleSheetParameters.RangeColumnEnd -
//            googleSheetParameters.RangeColumnStart; var columnNames = new List<string>(); var
//            returnValues = new List<ExpandoObject>();

//            if (!googleSheetParameters.FirstRowIsHeaders)
//            {
//                for (var i = 0; i <= numberOfColumns;
//            i++) { columnNames.Add($"Column{i}"); }
//            }

//            var response = request.Execute();

//            int rowCounter = 0; IList<IList<Object>> values = response.Values; if (values != null &&
//            values.Count > 0)
//            {
//                foreach (var row in values)
//                {
//                    if (googleSheetParameters.FirstRowIsHeaders && rowCounter == 0)
//                    {
//                        for (var i = 0; i <= numberOfColumns; i++)
//                        {
//                            columnNames.Add(row[i].ToString());
//                        }
//                        rowCounter++; continue;
//                    }

//                    var expando = new ExpandoObject();
//                    var expandoDict = expando as IDictionary<String, object>;
//                    var columnCounter = 0; foreach (var columnName in columnNames)
//                    {
//                        expandoDict.Add(columnName, row[columnCounter].ToString()); columnCounter++;
//                    }
//                    returnValues.Add(expando); rowCounter++;
//                }
//            }

//            return returnValues;
//        }

//        private string GetColumnName(int index)
//        {
//            const string letters =
//        "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; var value = "";

//            if (index >= letters.Length) value += letters[index / letters.Length - 1];

//            value += letters[index % letters.Length]; return value;
//        }

//        private int GetSheetId(GoogleService service, string spreadSheetId, string spreadSheetName)
//        {
//            var spreadsheet = service.Spreadsheets.Get(spreadSheetId).Execute();
//            var sheet = spreadsheet.Sheets.Where(s => s.Properties.Title == spreadSheetName).FirstOrDefault();
//            int sheetId = (int)sheet.Properties.SheetId;
//            return sheetId;
//        }

//    }
//}