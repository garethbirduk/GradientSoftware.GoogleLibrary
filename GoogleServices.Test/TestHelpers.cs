using Gradient.Utils;

namespace GoogleServices.Test
{
    internal static class TestHelpers
    {
        internal static string RandomCalendarName()
        {
            return StringHelpers.RandomName(prefix: "_deleteme_");
        }
    }
}