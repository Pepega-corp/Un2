using System.Collections.Generic;

namespace Unicon2.Infrastructure.Values
{
    public interface IBitMaskValue : IFormattedValue
    {
        List<List<bool>> BitArray { get; set; }
        List<string> BitSignatures { get; set; }
        List<bool> GetAllBits();
    }
}