using System;

namespace GoogleLibrary
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class TypeAttribute(Type type) : Attribute
    {
        public Type Type { get; } = type;
    }
}