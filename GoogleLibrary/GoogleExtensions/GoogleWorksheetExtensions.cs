﻿using GoogleLibrary.Custom.Maths;

namespace GoogleLibrary.GoogleExtensions
{
    public static class GoogleWorksheetExtensions
    {
        public static string ToGoogleColumn(this int myInt, IndexBase indexBase = IndexBase.Zero)
        {
            if (indexBase == IndexBase.One)
                myInt--;

            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = "";

            if (myInt >= letters.Length)
                result += letters[myInt / letters.Length - 1];

            result += letters[myInt % letters.Length];
            return result;
        }

        public static string ToGoogleColumn(this int? myInt, IndexBase indexBase = IndexBase.Zero)
        {
            if (myInt == null)
                return "";
            return myInt.Value.ToGoogleColumn(indexBase);
        }
    }
}