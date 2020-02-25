using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface INumericValueViewModel : IFormattedValueViewModel<INumericValue>
    {
        string NumValue { get; set; }
    }
}