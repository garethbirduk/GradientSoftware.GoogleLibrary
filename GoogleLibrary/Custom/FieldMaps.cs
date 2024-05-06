using GoogleLibrary.Custom;
using GoogleLibrary.GoogleSheets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GoogleLibrary.GoogleCalendar
{
    public class FieldMaps
    {
        public static Dictionary<string, EnumEventFieldType> KnownEventTypes
        {
            get
            {
                var dictionary = new Dictionary<string, EnumEventFieldType>();
                foreach (EnumEventFieldType type in Enum.GetValues(typeof(EnumEventFieldType)))
                {
                    var name = Enum.GetName(typeof(EnumEventFieldType), type);
                    if (name == null)
                        continue;
                    var field = typeof(EnumEventFieldType).GetField(name);
                    if (field == null)
                        continue;

                    dictionary.Add(name, type);
                    var list = EnumExtensions.Aliases(type);
                    foreach (var item in list)
                        dictionary.Add(item, type);
                }
                return dictionary;
            }
        }

        public static List<Tuple<string, EnumEventFieldType>> EventTypes(params string[] names)
        {
            var dictionary = new List<Tuple<string, EnumEventFieldType>>();
            foreach (var name in names)
            {
                if (KnownEventTypes.ContainsKey(name))
                    dictionary.Add(new Tuple<string, EnumEventFieldType>(name, KnownEventTypes[name]));
                else
                    dictionary.Add(new Tuple<string, EnumEventFieldType>(name, EnumEventFieldType.Unknown));
            }
            return dictionary;
        }
    }
}