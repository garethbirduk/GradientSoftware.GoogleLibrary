﻿using Google.Apis.Sheets.v4;
using System.Collections.Generic;

namespace GoogleLibrary.GoogleServices
{
    public class GoogleSpreadsheetsService : GoogleSpreadsheetsReadonlyService
    {
        public override IEnumerable<string> Scopes => new List<string>() { SheetsService.Scope.Spreadsheets };
    }
}