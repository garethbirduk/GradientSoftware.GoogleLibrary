using Gradient.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleLibrary.Test
{
    public enum EnumAttendeeField1
    {
        Unknown,

        [Alias("A")]
        Andy,

        [Alias("B")]
        Bob,

        [Alias("C")]
        Carl,
    }

    public enum EnumAttendeeField2
    {
        Unknown,

        [Alias("B")]
        Bob,

        [Alias("D")]
        Dave,

        [Alias("C")]
        Charlie,
    }

    public interface IAttendee
    {
        IEnumerable<string> Aliases();
    }

    public static class EnumExtensions
    {
        public static T AttributeFirstOrDefault<T>(this Enum value)
            where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
    }

    public class EnumAttendeeWrapper<T>(T enumValue) : IAttendee where T : Enum
    {
        private readonly T _enumValue = enumValue;

        public IEnumerable<string> Aliases()
        {
            var attribute = _enumValue.AttributeFirstOrDefault<AliasAttribute>();
            return attribute != null ? attribute.List : Enumerable.Empty<string>();
        }
    }

    public class MyClass
    {
        public List<IAttendee> Attendees { get; set; } = [];

        public List<string> GetAttendeesAlias()
        {
            var list = new List<string>();
            foreach (var attendee in Attendees)
            {
                var alias = attendee.Aliases().FirstOrDefault();
                if (alias != null)
                    list.Add(alias);
            }
            return list;
        }
    }
}