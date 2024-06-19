using Google.Apis.Calendar.v3.Data;
using Gradient.Utils.Attributes;
using PostSharp.Patterns.Contracts;

namespace GoogleLibrary.Custom.Events
{
    public static class EventBuilder
    {
        internal static BasicEvent Create([Required] Event x)
        {
            // todo...
            Console.WriteLine(x);
            return new BasicEvent()
            {
                Title = x.Summary
            };
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
    }
}