using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Presentation.Infrastructure.Services.Formatting
{
    public interface IFormattingService
    {
        Task<IFormattedValue> FormatValueAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, FormattingContext formattingContext);

        Task<Result<ushort[]>> FormatBackAsync(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue, FormattingContext formattingContext);


    }
}