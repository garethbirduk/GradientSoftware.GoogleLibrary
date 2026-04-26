using PostSharp.Patterns.Contracts;

namespace GoogleLibrary.Custom.Events
{
    public static class EventsBuilder
    {
        public static List<BasicEvent> Create([Required] List<Tuple<string, EnumEventFieldType>> fields, [Required] IEnumerable<IEnumerable<string>> data)
        {
            return data
                .Select(x => EventBuilder.Create(fields, x.ToList()))
                .Where(x => !string.IsNullOrWhiteSpace(string.Join(" ", x.Summary)))
                .ToList();
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