using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Infrastructure.Model
{
    public interface IBitMaskFormatter : IUshortsFormatter
    {
        string BitSignaturesInOneLine { get; set; }
        List<string> BitSignatures { get; set; }
    }
}