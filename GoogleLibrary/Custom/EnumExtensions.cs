using System;
using System.Collections.Generic;
using Utils.Attributes;

namespace GoogleLibrary.Custom
{
    public static class EnumExtensions
    {
        // This method creates a specific call to the above method, requesting the
        // Description MetaData attribute.
        public static IEnumerable<string> Aliases(this Enum value)
        {
            var attribute = value.GetAttribute<AliasAttribute>();
            return attribute == null ? new List<string>() : attribute.List;
        }

        // This extension method is broken out so you can use a similar pattern with
        // other MetaData elements in the future. This is your base method for each.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }
    }
}