using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IValueViewModelFactory
    {
        IFormattedValueViewModel CreateFormattedValueViewModel(IFormattedValue formattedValue);
        IEditableValueViewModel CreateEditableValueViewModel(IFormattedValue formattedValue);
    }
}