namespace GoogleLibrary.Custom.Events
{
    /// <summary>
    /// Pure functions used by the spreadsheet ⇄ calendar round-trip.
    ///
    /// Headers are persisted in the calendar's description so the original column ordering survives
    /// a calendar→sheet trip. Format:
    ///
    ///     <freeText>
    ///     Headers*/*Date*/*Time*/*Summary*/*...
    ///     <moreFreeText>
    ///
    /// Any line in the description starting with "Headers*/*" is interpreted as the encoded header row.
    /// </summary>
    public static class WorksheetCalendarMapping
    {
        private const string HeadersLinePrefix = "Headers";
        private const string HeadersDelimiter = "*/*";

        /// <summary>
        /// Default header set used when no Headers line is present in the calendar description.
        /// Covers the most-common columns; any unknown headers in the actual sheet would have been
        /// preserved if WorksheetToCalendarAsync wrote them.
        /// </summary>
        public static readonly IReadOnlyList<string> DefaultHeaders =
        [
            "StartDate", "StartTime", "EndDate", "EndTime",
            "Summary", "Status", "Category",
            "From", "To", "Description"
        ];

        public static string EncodeHeaders(IEnumerable<string> headers)
        {
            return HeadersLinePrefix + HeadersDelimiter + string.Join(HeadersDelimiter, headers);
        }

        public static bool TryDecodeHeaders(string? description, out List<string> headers)
        {
            headers = [];
            if (string.IsNullOrWhiteSpace(description))
                return false;

            var line = description
                .Split(["\r\n", "\n"], StringSplitOptions.None)
                .FirstOrDefault(x => x.StartsWith(HeadersLinePrefix + HeadersDelimiter, StringComparison.Ordinal));
            if (line == null)
                return false;

            headers = line
                .Substring(HeadersLinePrefix.Length)
                .Split(HeadersDelimiter, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            return true;
        }

        /// <summary>
        /// Project a single BasicEvent field into the cell value to write under a given header.
        /// Unknown / Description-flagged headers fall through to a CustomFields lookup keyed on the header name.
        /// </summary>
        public static string GetCellValue(BasicEvent e, EnumEventFieldType fieldType, string headerName)
        {
            switch (fieldType)
            {
                case EnumEventFieldType.Summary:
                    return e.Title;
                case EnumEventFieldType.StartDate:
                    return e.StartDate == default ? "" : e.StartDate.ToString("yyyy-MM-dd");
                case EnumEventFieldType.StartTime:
                    return e.StartTime?.ToString("HH:mm") ?? "";
                case EnumEventFieldType.EndDate:
                    return e.EndDate?.ToString("yyyy-MM-dd") ?? "";
                case EnumEventFieldType.EndTime:
                    return e.EndTime?.ToString("HH:mm") ?? "";
                case EnumEventFieldType.Status:
                    return e.Status == EventStatus.None ? "" : e.Status.ToString();
                case EnumEventFieldType.Category:
                    return e.Category == EventCategory.None ? "" : e.Category.ToString();
                case EnumEventFieldType.Description:
                    return e.Description;
                case EnumEventFieldType.Reminders:
                    return string.Join(", ", e.Reminders);
                case EnumEventFieldType.From:
                    return e.Locations.FirstOrDefault()?.ShortName ?? "";
                case EnumEventFieldType.To:
                    return e.Locations.LastOrDefault()?.ShortName ?? "";
                case EnumEventFieldType.FromAddress:
                    return e.Locations.FirstOrDefault()?.Address ?? "";
                case EnumEventFieldType.ToAddress:
                    return e.Locations.LastOrDefault()?.Address ?? "";
                case EnumEventFieldType.FlightCarrier:
                    return e is FlightEvent fc ? fc.FlightInformation.Carrier : "";
                case EnumEventFieldType.FlightNumber:
                    return e is FlightEvent fn ? fn.FlightInformation.Number : "";
                default:
                    if (!string.IsNullOrWhiteSpace(headerName) && e.CustomFields.TryGetValue(headerName, out var v))
                        return v;
                    return "";
            }
        }

        /// <summary>
        /// Project a BasicEvent into the ordered list of cell values for a given header row.
        /// </summary>
        public static List<string> ToCellValues(BasicEvent e, IEnumerable<string> headers)
        {
            var fieldTypes = FieldMaps.EventTypes(headers.ToArray());
            return fieldTypes
                .Select(t => GetCellValue(e, t.Item2, t.Item1))
                .ToList();
        }
    }
}
