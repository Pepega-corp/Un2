using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
   public class BoolValueViewModel:FormattableValueViewModelBase,IBoolValueViewModel
    {
        private string _strongName;
        private bool _boolValueProperty;

        public override string StrongName => nameof(BoolValueViewModel);

        public override void InitFromValue(IFormattedValue value)
        {
            if (value is IBoolValue)
            {
                Header = value.Header;
                BoolValueProperty = (value as IBoolValue).BoolValueProperty;
            }
        }

        public bool BoolValueProperty
        {
            get { return _boolValueProperty; }
            set
            {
                _boolValueProperty = value; 
                RaisePropertyChanged();
                
            }
        }
    }
}
