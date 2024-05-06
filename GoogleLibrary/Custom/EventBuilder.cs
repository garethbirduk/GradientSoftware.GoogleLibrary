using Google.Apis.Calendar.v3.Data;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.Custom
{
    public static class EventBuilder
    {
        internal static BasicEvent Create(Event x)
        {
            return new BasicEvent();
        }

        public static BasicEvent Create([Required] List<Tuple<string, EnumEventFieldType>> fields, [Required] List<string> data)
        {
            for (var i = data.Count; i < fields.Count; i++)
                data.Add("");

            var category = BasicEvent.GetEnum<EnumEventFieldType, EventCategory>(EnumEventFieldType.Category, fields, data);

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