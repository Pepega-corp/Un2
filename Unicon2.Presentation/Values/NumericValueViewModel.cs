using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class NumericValueViewModel : FormattableValueViewModelBase, INumericValueViewModel
    {
        private string _numValue;

        public override string StrongName => nameof(NumericValueViewModel);
        public string NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                RaisePropertyChanged();
            }
        }

        public void SetNumValue(string value)
        {
	        NumValue = value;
        }
    }
}