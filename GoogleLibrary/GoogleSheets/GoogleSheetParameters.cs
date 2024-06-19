using Google.Apis.Sheets.v4.Data;
using GoogleLibrary.Custom.Maths;

namespace GoogleLibrary.GoogleSheets
{
    public class GoogleSheetParameters
    {
        public GoogleSheetParameters(int rangeColumnStart, int rangeColumnEnd, IndexBase indexBase)
        {
            if (indexBase == IndexBase.One)
            {
                rangeColumnStart--;
                rangeColumnEnd--;
            }

            RangeColumnStart = rangeColumnStart;
            RangeColumnEnd = rangeColumnEnd;
        }

        public bool FirstRowIsHeaders { get; set; }

        public int RangeColumnEnd { get; set; }

        public int RangeColumnStart { get; set; }

        public int RangeRowEnd { get; set; }

        public int RangeRowStart { get; set; }

        public string SheetName { get; set; }

        public static GridCoordinate GridCoordinate(int worksheetId, int rangeColumnStart, int rangeRowStart)
        {
            return new GridCoordinate
            {
                ColumnIndex = rangeColumnStart,
                RowIndex = rangeRowStart,
                SheetId = worksheetId
            };
        }

        public GridCoordinate ToGridCoordinate(int worksheetId)
        {
            return GridCoordinate(worksheetId, RangeColumnStart, RangeRowStart);
        }
    }
}