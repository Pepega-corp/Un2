using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Factories
{
    public class ValueViewModelFactory : IValueViewModelFactory
    {
        public IFormattedValueViewModel CreateFormattedValueViewModel(IFormattedValue formattedValue)
        {
	        if (formattedValue == null)
	        {
		        return null;
	        }
            return formattedValue.Accept(new FormattedValueViewModelFactory(formattedValue as IRangeable,
                formattedValue as IMeasurable));
        }

        public IEditableValueViewModel CreateEditableValueViewModel(FormattedValueInfo formattedValueInfo)
        {
	        if (formattedValueInfo.FormattedValue != null)
		        return formattedValueInfo.FormattedValue.Accept(new EditableValueViewModelFactory(
			        formattedValueInfo.IsEditingEnabled, formattedValueInfo.Rangeable,
			        formattedValueInfo.Measurable));
	        return null;
        }
    }
}