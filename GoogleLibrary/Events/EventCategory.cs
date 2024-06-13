using Gradient.Utils.Attributes;

namespace GoogleLibrary.Events
{
    public enum EventCategory
    {
        Unknown,

        None,

        [Alias("plane")]
        Flight,

        [Alias("car")]
        Drive,

        Train,

        Taxi,

        Admin,

        Accommodation,
    }
}