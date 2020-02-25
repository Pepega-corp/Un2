using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Values
{
    public interface INumericValue : IMeasurable, IFormattedValue
    {
        double NumValue { get; set; }
    }
}