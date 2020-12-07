using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IValueViewModelFactory
    {
        IFormattedValueViewModel CreateFormattedValueViewModel(IFormattedValue formattedValue);
        IEditableValueViewModel CreateEditableValueViewModel(FormattedValueInfo formattedValue);
        
    }

    public class FormattedValueInfo
    {
	    public FormattedValueInfo(IFormattedValue formattedValue, IMeasurable measurable, IUshortsFormatter formatter,
		    IRangeable rangeable, bool isEditingEnabled = true, bool isChangedByDefault=false)
        {
            FormattedValue = formattedValue;
            Measurable = measurable;
            Formatter = formatter;
            Rangeable = rangeable;
            IsEditingEnabled = isEditingEnabled;
            IsChangedByDefault = isChangedByDefault;
        }

        public IFormattedValue FormattedValue { get; }
        public IRangeable Rangeable { get; }
        public IMeasurable Measurable { get; }
        public IUshortsFormatter Formatter { get; }
		public bool IsEditingEnabled { get; }
        public bool IsChangedByDefault { get; }
    }
}