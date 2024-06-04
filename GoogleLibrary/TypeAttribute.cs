using PostSharp.Patterns.Contracts;

namespace GoogleLibrary
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class TypeAttribute([Required] Type type) : Attribute
    {
        public Type Type { get; } = type;
    }
}