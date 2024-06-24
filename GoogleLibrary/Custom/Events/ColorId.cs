using Gradient.Utils.Attributes;

namespace GoogleLibrary.Custom.Events
{
    /// <summary>
    /// Google ColorId's are ints as string (ugly!).
    /// todo: make use of Aliases and process "1" as 1 or ColorId.PaleBlue, and in reverse too.
    /// </summary>
    public enum ColorId
    {
        None = 0,

        [Alias("LightBlue")]
        PaleBlue = 1,

        [Alias("LightGreen")]
        PaleGreen = 2,

        Mauve = 3,

        [Alias("LightRed", "Pink")]
        PaleRed = 4,

        Yellow = 5,
        Orange = 6,
        Cyan = 7,

        [Alias("Gray")]
        Grey = 8,

        Blue = 9,
        Green = 10,
        Red = 11,
    }
}