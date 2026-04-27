using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.Custom.Locations;
using Gradient.Utils.Attributes;
using PostSharp.Patterns.Contracts;
using System.Text.RegularExpressions;

namespace GoogleLibrary.Custom.Events
{
    public static class EventBuilder
    {
        // Status annotation prefix written by GoogleEventBuilder.WithAnnotation; e.g. "(p) Hotel".
        private static readonly Regex _statusPrefix = new(@"^\((i|p|r|x|£)\) (.*)$", RegexOptions.Compiled);

        // Custom-field line written by BasicEventExtensions.GetCustomFieldsAsDescription; e.g. "Flight: BA (175)".
        private static readonly Regex _customFieldLine = new(@"^([A-Za-z0-9_£$€][\w \-£$€]*?): (.+)$", RegexOptions.Compiled);

        private static readonly Regex _mapsSearch = new(@"^https://www\.google\.com/maps/search/(.+)$", RegexOptions.Compiled);
        private static readonly Regex _mapsDir = new(@"^https://www\.google\.com/maps/dir/(.+)$", RegexOptions.Compiled);

        /// <summary>
        /// Best-effort inverse of GoogleEventBuilder.Create — parses a Google Calendar event back into a BasicEvent.
        /// Lossy: original spreadsheet headers are not recovered; description lines containing ": " may be misclassified
        /// as custom fields; ColorId.Green resolves to Confirmed (Reserved is indistinguishable without an annotation).
        /// </summary>
        public static BasicEvent Create([Required] Event x)
        {
            var (status, title) = ParseStatusAndTitle(x.Summary, x.ColorId);
            var (description, customFields, additionalData) = ParseDescription(x.Description);
            var (startDate, startTime, endDate, endTime) = ParseDates(x.Start, x.End);

            var basicEvent = new BasicEvent
            {
                Title = title,
                Status = status,
                StartDate = startDate,
                StartTime = startTime,
                EndDate = endDate,
                EndTime = endTime,
                Description = description,
                Locations = ParseLocations(x.Location),
                Reminders = x.Reminders?.Overrides?
                    .Select(r => r.Minutes ?? 0)
                    .Where(m => m > 0)
                    .ToList() ?? []
            };

            foreach (var kv in customFields)
                basicEvent.CustomFields[kv.Key] = kv.Value;
            foreach (var item in additionalData)
                basicEvent.AdditionalData.Add(item);

            return basicEvent;
        }

        [Precondition("fields", "fields.Count > 0")]
        [Precondition("data", "data.Count > 0")]
        public static BasicEvent Create([Required] List<Tuple<string, EnumEventFieldType>> fields, [Required] List<string> data)
        {
            for (var i = data.Count; i < fields.Count; i++)
                data.Add("");

            var category = fields.GetEnumOrDefault<EnumEventFieldType, EventCategory>(EnumEventFieldType.Category, data);

            BasicEvent e;
            switch (category)
            {
                case EventCategory.Flight:
                    {
                        e = new FlightEvent();
                        break;
                    }
                case EventCategory.Drive:
                case EventCategory.Train:
                case EventCategory.Taxi:
                    {
                        e = new TravelEvent();
                        break;
                    }
                case EventCategory.Accommodation:
                    {
                        e = new AccommodationEvent();
                        break;
                    }
                default:
                    {
                        e = new BasicEvent();
                        break;
                    }
            }
            e.Build(fields, data);
            return e;
        }

        private static (DateTime startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime) ParseDates(EventDateTime? start, EventDateTime? end)
        {
            if (start == null)
                return (DateTime.UtcNow, null, null, null);

            if (!string.IsNullOrEmpty(start.Date))
            {
                var s = DateTime.Parse(start.Date);
                DateTime? e = null;
                if (!string.IsNullOrEmpty(end?.Date))
                {
                    // SetMultiDay adds +1 day for Google's exclusive end; undo that here.
                    e = DateTime.Parse(end.Date).AddDays(-1);
                }
                return (s, null, e, null);
            }

            if (start.DateTimeDateTimeOffset.HasValue)
            {
                var s = start.DateTimeDateTimeOffset.Value.LocalDateTime;
                DateTime? eDate = null;
                DateTime? eTime = null;
                if (end?.DateTimeDateTimeOffset.HasValue == true)
                {
                    var eDt = end.DateTimeDateTimeOffset.Value.LocalDateTime;
                    eDate = eDt.Date;
                    eTime = eDt;
                }
                return (s.Date, s, eDate, eTime);
            }

            return (DateTime.UtcNow, null, null, null);
        }

        private static (string description, Dictionary<string, string> customFields, List<string> additionalData) ParseDescription(string? raw)
        {
            var customFields = new Dictionary<string, string>();
            var additionalData = new List<string>();
            if (string.IsNullOrWhiteSpace(raw))
                return ("", customFields, additionalData);

            var lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // Format written by BasicEventExtensions.ToDescriptionString:
            //     <description lines, may include "Key: value" patterns>
            //     ---
            //     <Key: value lines>
            //     <additional-data lines>
            // The sentinel makes the Description boundary unambiguous. If absent (legacy events,
            // or events authored outside this library), fall back to the older heuristic that
            // treats the first kv-pattern line as the start of the structured tail.
            var sentinelIdx = Array.IndexOf(lines, BasicEventExtensions.DescriptionSectionDelimiter);

            if (sentinelIdx >= 0)
            {
                var description = string.Join("\r\n", lines.Take(sentinelIdx));
                ClassifyTail(lines, sentinelIdx + 1, customFields, additionalData);
                return (description, customFields, additionalData);
            }

            // Heuristic fallback: lines before first kv pattern → Description, kv lines →
            // CustomFields, anything after the kv block → AdditionalData.
            var descriptionLines = new List<string>();
            var seenKvBlock = false;
            foreach (var line in lines)
            {
                var match = _customFieldLine.Match(line);
                if (match.Success)
                {
                    seenKvBlock = true;
                    customFields[match.Groups[1].Value.Trim()] = match.Groups[2].Value.Trim();
                }
                else if (!seenKvBlock)
                {
                    descriptionLines.Add(line);
                }
                else
                {
                    additionalData.Add(line);
                }
            }
            return (string.Join("\r\n", descriptionLines), customFields, additionalData);
        }

        private static void ClassifyTail(string[] lines, int startIdx, Dictionary<string, string> customFields, List<string> additionalData)
        {
            for (var i = startIdx; i < lines.Length; i++)
            {
                var line = lines[i];
                var match = _customFieldLine.Match(line);
                if (match.Success)
                    customFields[match.Groups[1].Value.Trim()] = match.Groups[2].Value.Trim();
                else
                    additionalData.Add(line);
            }
        }

        private static List<Location> ParseLocations(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return [];

            var searchMatch = _mapsSearch.Match(raw);
            if (searchMatch.Success)
            {
                var addr = Uri.UnescapeDataString(searchMatch.Groups[1].Value).Replace("+", " ");
                return [new Location(addr, addr)];
            }

            var dirMatch = _mapsDir.Match(raw);
            if (dirMatch.Success)
            {
                return dirMatch.Groups[1].Value
                    .Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Uri.UnescapeDataString(s).Replace("+", " "))
                    .Select(addr => new Location(addr, addr))
                    .ToList();
            }

            return [new Location(raw, raw)];
        }

        private static (EventStatus status, string title) ParseStatusAndTitle(string? summary, string? colorId)
        {
            summary ??= "";

            var match = _statusPrefix.Match(summary);
            if (match.Success)
            {
                var status = match.Groups[1].Value switch
                {
                    "i" => EventStatus.Idea,
                    "p" => EventStatus.Planned,
                    "r" => EventStatus.Reserved,
                    "x" => EventStatus.Cancelled,
                    "£" => EventStatus.Paid,
                    _ => EventStatus.None
                };
                return (status, match.Groups[2].Value);
            }

            if (!string.IsNullOrEmpty(colorId) && int.TryParse(colorId, out var colorInt))
            {
                var status = (ColorId)colorInt switch
                {
                    ColorId.Orange => EventStatus.Idea,
                    ColorId.Yellow => EventStatus.Planned,
                    ColorId.Green => EventStatus.Confirmed,
                    ColorId.Cyan => EventStatus.Paid,
                    ColorId.Red => EventStatus.Cancelled,
                    _ => EventStatus.None
                };
                return (status, summary);
            }

            return (EventStatus.None, summary);
        }
    }
}
