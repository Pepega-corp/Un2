using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IValueViewModelFactory
    {
        IFormattedValueViewModel CreateFormattedValueViewModel(IFormattedValue formattedValue,IMeasurable measurable=null, IRangeable rangeable = null);
    }
}