using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class StringValueViewModel : FormattableValueViewModelBase<IStringValue>, IStringValueViewModel
    {
        private string _stringValue;

        public override string StrongName => nameof(StringValueViewModel);

        public override void InitFromValue(IStringValue value)
        {
            StringValue = value.StrValue;
        }

        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                _stringValue = value;
                RaisePropertyChanged();
            }
        }
    }
}