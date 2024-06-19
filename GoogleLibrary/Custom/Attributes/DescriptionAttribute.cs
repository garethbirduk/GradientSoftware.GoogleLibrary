namespace GoogleLibrary.Custom.Attributes
{
    /// <summary>
    /// Properties or enums with this attribute are collated for storage in the Description free-text within the Google calendar object.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class DescriptionAttribute : Attribute
    {
    }
}