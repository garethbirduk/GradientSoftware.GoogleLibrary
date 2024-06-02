using PostSharp.Patterns.Contracts;
using System;

namespace GoogleLibrary
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class TypeAttribute([Required] Type type) : Attribute
    {
        public Type Type { get; } = type;
    }
}