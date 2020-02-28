using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Infrastructure.Services.Formatting
{
    public interface IFormattingService
    {
        IFormattedValue FormatValue(IUshortsFormatter ushortsFormatter, ushort[] ushorts);
        ushort[] FormatBack(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue);
    }
}