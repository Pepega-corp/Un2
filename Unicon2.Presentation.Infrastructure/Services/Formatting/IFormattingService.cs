using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.Services.Formatting
{
    public interface IFormattingService
    {
        Task<IFormattedValue> FormatValueAsenc(IUshortsFormatter ushortsFormatter, ushort[] ushorts,DeviceContext.DeviceContext deviceContext);

        IFormattedValue FormatValue(IUshortsFormatter ushortsFormatter, ushort[] ushorts);
        ushort[] FormatBack(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue);
    }
}