using Gradient.Utils;
using System.Globalization;

namespace GoogleLibrary.Events
{
    public static class FieldsExtensions
    {
        public static Location CreateLocation(this List<Tuple<string, EnumEventFieldType>> fields,
            EnumEventFieldType locationField, EnumEventFieldType addressField, List<string> data)
        {
            var shortName = fields.GetStringOrDefault(locationField, data);
            if (shortName.IsAirport())
            {
                return new AirportLocation(shortName);
            };
            return new Location(shortName, fields.GetStringOrDefault(addressField, data));
        }

        /// <summary>
        /// Find the enumType (T) in the given list of field names and corresponding types, and return the DateTime value of the data in that field
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeOrDefault<T>(this List<Tuple<string, T>> fields,
            T enumType, List<string> data)
            where T : Enum
        {
            var date = DateTime.UtcNow;
            var index = fields.FindIndex(x => x.Item2.Equals(enumType));
            if (index < 0)
                return date;
            var value = data[index].Trim();
            if (!string.IsNullOrWhiteSpace(value))
                date = value.ParseAdvanced(CultureInfo.InvariantCulture);
            return date;
        }

        /// <summary>
        /// Find the enumType (T) in the given list of field names and corresponding types, and return the enum value of the data in that field
        /// </summary>
        /// <typeparam name="T">The enum type of the field to be found</typeparam>
        /// <typeparam name="T2">The enum type of the data to be found in that field</typeparam>
        /// <param name="enumType"></param>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        /// <returns>The (T2) enum value or default</returns>
        public static T2 GetEnumOrDefault<T, T2>(this List<Tuple<string, T>> fields,
            T enumType, List<string> data, bool allowAlias = false)
            where T : Enum
            where T2 : Enum
        {
            var index = fields.FindIndex(x => x.Item2.Equals(enumType));
            if (index < 0)
                return default;
            var value = data[index].Trim();
            return EnumHelper.StringToEnumOrDefault<T2>(value, allowAlias);
        }

        /// <summary>
        /// Retrieve data as a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumEventFieldType"></param>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(this List<Tuple<string, EnumEventFieldType>> fields, EnumEventFieldType enumEventFieldType, List<string> data)
        {
            var index = fields.FindIndex(x => x.Item2 == enumEventFieldType);
            if (index < 0)
                return [];
            var value = data[index];
            var list = data[fields.FindIndex(x => x.Item2 == enumEventFieldType)].Split("").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            return list.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
        }

        /// <summary>
        /// Retrieve data as a string.
        /// </summary>
        /// <param name="enumEventFieldType"></param>
        /// <param name="fields"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetStringOrDefault(this List<Tuple<string, EnumEventFieldType>> fields,
            EnumEventFieldType enumEventFieldType, List<string> data)
        {
            var index = fields.FindIndex(x => x.Item2 == enumEventFieldType);
            if (index < 0)
                return "";
            return data[index];
        }
    }
}