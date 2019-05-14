using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Values
{
    public interface IBitGroupValue
    {
        IFormattedValue FormattedValue { get; set; }
        IUshortsFormatter UshortsFormatter { get; set; }
    }
}