using System.Collections.Generic;

namespace GoogleLibrary.Services
{
    public interface IGooglePersonalData
    {
        List<string> ReservedList { get; set; }
    }
}