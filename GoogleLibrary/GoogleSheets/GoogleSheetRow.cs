﻿using System.Collections.Generic;

namespace GoogleLibrary.GoogleSheets
{
    public class GoogleSheetRow
    {
        public GoogleSheetRow() => Cells = new List<GoogleSheetCell>();

        public List<GoogleSheetCell> Cells { get; set; }
    }
}