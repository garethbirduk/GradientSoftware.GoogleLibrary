using PostSharp.Patterns.Contracts;
using System.Data;

namespace GoogleLibrary.Events
{
    public static class EventsBuilder
    {
        private static void CombineEvent(BasicEvent primary, BasicEvent secondary)
        {
            //var descriptions = new List<Dictionary<string, string>>();
            //var list = new List<BasicEvent> { primary, secondary };
            //foreach (var item in list)
            //    descriptions.Add(GoogleCalendarEventConverter.FromDescription(item.Event.Description));

            //foreach (var description in descriptions.Skip(1))
            //{
            //    var duplicates = description.Where(x => descriptions[0].ContainsKey(x.Key) && descriptions[0][x.Key] == x.Value).Select(x => x.Key);
            //    foreach (var duplicate in duplicates)
            //        description.Remove(duplicate);
            //}

            //foreach (var item in list.Skip(1))
            //{
            //    foreach (var description in descriptions.Skip(1).Where(x => x.Any()))
            //    {
            //        var s = GoogleCalendarEventConverter.ToDescription(description);
            //        primary.Event.Description += $"\r\n\r\n{s}";
            //    }
            //}

            //foreach (var item in list.Skip(1))
            //{
            //    foreach (var attendee in item.Event.Attendees)
            //    {
            //        primary.Event.Attendees.Add(attendee);
            //    }
            //}
        }

        private static BasicEvent? FindDuplicateOrDefault(BasicEvent myEvent, IEnumerable<BasicEvent> otherEvents)
        {
            return otherEvents.SingleOrDefault(x =>
                x.EventId != myEvent.EventId
                && x.Summary == myEvent.Summary
                && x.StartDate == myEvent.StartDate
                && x.StartTime == myEvent.StartTime
                && x.EndDate == myEvent.EndDate
                && x.EndTime == myEvent.EndTime
                );
        }

        public static List<BasicEvent> Create([Required] List<Tuple<string, EnumEventFieldType>> fields, [Required] IEnumerable<IEnumerable<string>> data)
        {
            var list = new List<BasicEvent>();
            var events = data.Select(x => EventBuilder.Create(fields, x.ToList())).ToList();
            foreach (var myEvent in events)
            {
                var duplicate = FindDuplicateOrDefault(myEvent, list);
                if (duplicate == null)
                {
                    if (!string.IsNullOrWhiteSpace(string.Join(" ", myEvent.Summary)))
                    {
                        list.Add(myEvent);
                    }
                }
                else
                {
                    CombineEvent(duplicate, myEvent);
                }
            }
            return list;
        }

        public static List<BasicEvent> Create(Google.Apis.Calendar.v3.Data.Events googleEvents)
        {
            return googleEvents.Items.Select(x => EventBuilder.Create(x)).ToList();
        }

        public static List<BasicEvent> Create(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> data)
        {
            var fields = FieldMaps.EventTypes(headers.Select(x => x.ToString()).ToArray());
            return Create(fields, data);
        }
    }
}