using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
   public class NumericValueViewModel:FormattableValueViewModelBase,INumericValueViewModel
    {
        private string _numValue;

        public override string StrongName => nameof(NumericValueViewModel);

        public override void InitFromValue(IFormattedValue value)
        {
            INumericValue numVal=value as INumericValue;
            NumValue = numVal.NumValue.ToString();
            Header = value.Header;
            base.InitFromValue(value);
        }

        public string NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                RaisePropertyChanged();
            }
        }
    }
}
