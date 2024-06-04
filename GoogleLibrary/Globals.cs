#if DEBUG

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GoogleLibrary.Test")]
#endif

namespace GoogleLibrary
{
    internal static class Globals
    {
        internal static string ApplicationName = "MyTestAppClient";
    }
}