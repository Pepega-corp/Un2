using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IBoolValueViewModel : IFormattedValueViewModel<IBoolValue>
    {
        bool BoolValueProperty { get; set; }
    }
}