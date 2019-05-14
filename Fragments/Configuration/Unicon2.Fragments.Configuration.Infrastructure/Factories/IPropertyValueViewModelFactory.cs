using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.Infrastructure.Factories
{
    public interface IPropertyValueViewModelFactory:IValueViewModelFactory
    {

        IEditableValueViewModel CreateEditableFormattedValueViewModel(IFormattedValue formattedValue, IProperty property, IUshortsFormatter ushortsFormatter);


    }
}