using GoogleLibrary.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary
{
    public static class Utils
    {
        public static List<string> GetNames(EnumEventFieldType type)
        {
            var list = new List<string>()
            {
                type.ToString()
            };
            return list;
        }

        public static string RandomName(string prefix = "", string suffix = "", int length = 8)
        {
            return $"{prefix}{string.Join("", Guid.NewGuid().ToString().Take(length))}{suffix}";
        }
    }
}