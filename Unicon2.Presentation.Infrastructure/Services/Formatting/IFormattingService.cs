using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.Services.Formatting
{
    public interface IFormattingService
    {
        Task<IFormattedValue> FormatValueAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts,DeviceContext.DeviceContext deviceContext, bool isLocal);
            
        IFormattedValue FormatValue(IUshortsFormatter ushortsFormatter, ushort[] ushorts, bool isLocal);
        ushort[] FormatBack(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue, bool isLocal);
        Task<Result<ushort[]>> FormatBackAsync(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue, DeviceContext.DeviceContext deviceContext, bool isLocal);


    }
}