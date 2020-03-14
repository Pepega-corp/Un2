using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Subscription;
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
            IRangeable rangeable)
        {
            FormattedValue = formattedValue;
            Measurable = measurable;
            Formatter = formatter;
            Rangeable = rangeable;
        }

        public IFormattedValue FormattedValue { get; }
        public IRangeable Rangeable { get; }
        public IMeasurable Measurable { get; }
        public IUshortsFormatter Formatter { get; }

    }
}