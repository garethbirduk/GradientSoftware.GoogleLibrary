using System.Globalization;
using System.Text.RegularExpressions;

namespace GoogleLibrary.Events
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Removes st, nd, rd, th from 1st, 2nd, 3rd, 4th etc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string RemoveDateSuffixes(string input)
        {
            return Regex.Replace(input, @"\b(\d{1,2})(st|nd|rd|th)\b", "$1", RegexOptions.IgnoreCase);
        }

        public static List<string> DateTimeFormats => new()
        {
            "ddd d MMM",          // Mon 1 Jan
            "ddd dd MMM",         // Mon 01 Jan
            "d MMM",              // 1 Jan
            "dd MMM",             // 01 Jan

            "dddd d MMM",         // Monday 1 Jan
            "dddd dd MMM",        // Monday 01 Jan
            "dddd d MMM yyyy",    // Monday 1 Jan 2024
            "dddd dd MMM yyyy",   // Monday 01 Jan 2024

            "ddd d MMMM",         // Mon 1 January
            "ddd dd MMMM",        // Mon 01 January
            "d MMMM",             // 1 January
            "dd MMMM",            // 01 January

            "dddd d MMMM",        // Monday 1 January
            "dddd dd MMMM",       // Monday 01 January
            "dddd d MMMM yyyy",   // Monday 1 January 2024
            "dddd dd MMMM yyyy",  // Monday 01 January 2024

            "dd/MM/yyyy",         // 01/01/2024
            "dd/M/yyyy",          // 01/1/2024
            "d/MM/yyyy",          // 1/01/2024
            "d/M/yyyy",           // 1/1/2024

            "yyyy/MM/dd",         // 2024/01/01
            "yyyy/M/dd",          // 2024/1/01
            "yyyy/MM/d",          // 2024/01/1
            "yyyy/M/d",           // 2024/1/1

            "yyyy-MM-dd",         // 2024-01-01 (ISO format)
            "yyyy-M-dd",          // 2024-1-01 (ISO format)
            "yyyy-MM-d",          // 2024-01-1 (ISO format)
            "yyyy-M-d",           // 2024-1-1 (ISO format)

            "dd/MM/yy",           // 01/01/24
            "dd/M/yy",            // 01/1/24
            "d/MM/yy",            // 1/01/24
            "d/M/yy",             // 1/1/24

            "yy/MM/dd",           // 24/01/01
            "yy/M/dd",            // 24/1/01
            "yy/MM/d",            // 24/01/1
            "yy/M/d",             // 24/1/1

            "d-MMM-yy",           // 1-Jan-24
            "dd-MMM-yy",          // 01-Jan-24
            "d-MMMM-yy",          // 1-January-24
            "dd-MMMM-yy",         // 01-January-24

            "d-MMM-yyyy",         // 1-Jan-2024
            "dd-MMM-yyyy",        // 01-Jan-2024
            "d-MMMM-yyyy",        // 1-January-2024
            "dd-MMMM-yyyy",       // 01-January-2024

            "MMM d, yyyy",        // Jan 1, 2024
            "MMMM d, yyyy",       // January 1, 2024
            "MMM dd, yyyy",       // Jan 01, 2024
            "MMMM dd, yyyy",      // January 01, 2024
        };

        /// <summary>
        /// DateTime.Parse() does not work well with Kestrel for some reason. Here we try to help support some additional formats to help with ParseExact() should Parse() fail.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static DateTime Parse(string value, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
                cultureInfo = CultureInfo.InvariantCulture;

            var preprocessedValue = RemoveDateSuffixes(value);

            foreach (var format in DateTimeFormats)
            {
                try
                {
                    if (DateTime.TryParseExact(preprocessedValue, format, cultureInfo, DateTimeStyles.AssumeLocal, out DateTime result))
                        return result;
                }
                catch
                {
                    // do nothing, try next format
                }
            }

            // Maybe the year is missing and gives annoying fail because of eg Mon 31 Dec, where "Mon" is wrong date for 2031
            // instead add yyyy to end and try again
            var valueYearAppend = $"{preprocessedValue} {DateTime.UtcNow.Year}";
            foreach (var format in DateTimeFormats)
            {
                try
                {
                    if (DateTime.TryParseExact(valueYearAppend, format, cultureInfo, DateTimeStyles.AssumeLocal, out DateTime result))
                        return result;
                }
                catch
                {
                    // do nothing, try next format
                }
            }
            // throw the default exception after all
            return DateTime.Parse(value, cultureInfo);
        }
    }
}