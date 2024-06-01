using System.Globalization;

namespace GoogleLibrary.Events
{
    public static class DateTimeHelper
    {
        public static List<string> DateTimeFormats => new()
        {
            "ddd d MMM",
            "ddd dd MMM",
            "d MMM",
            "dd MMM",

            "dd/MM/yyyy",
            "dd/M/yyyy",
            "d/MM/yyyy",
            "d/M/yyyy",

            "dd/MM/yy",
            "dd/M/yy",
            "d/MM/yy",
            "d/M/yy",
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
            foreach (var format in DateTimeFormats)
            {
                try
                {
                    DateTime result;
                    if (DateTime.TryParseExact(value, format, cultureInfo, DateTimeStyles.AssumeLocal, out result))
                        return result;
                }
                catch
                {
                    // do nothing, try next format
                }
            }

            // maybe the year is missing and gives annoying fail because of eg Mon 31 Dec, where "Mon" is wrong date for 2031
            // instead add yyyy to end and try again
            var valueYearAppend = $"{value} {DateTime.UtcNow.Year}";
            foreach (var format in DateTimeFormats)
            {
                try
                {
                    DateTime result;
                    if (DateTime.TryParseExact(valueYearAppend, format, cultureInfo, DateTimeStyles.AssumeLocal, out result))
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