using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Infrastructure.Model
{
    public interface IDictionaryMatchingFormatter : IUshortsFormatter, IInitializableFromContainer
    {
        Dictionary<ushort, string> StringDictionary { get; set; }
        bool IsKeysAreNumbersOfBits { get; set; }
    }
}