using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
   public class BoolValueViewModel:FormattableValueViewModelBase,IBoolValueViewModel
    {
        private string _strongName;
        private bool _boolValueProperty;

        #region Overrides of FormattableValueViewModelBase

        public override string StrongName => nameof(BoolValueViewModel);

        public override void InitFromValue(IFormattedValue value)
        {
            if (value is IBoolValue)
            {
                Header = value.Header;
                BoolValueProperty = (value as IBoolValue).BoolValueProperty;
            }
        }

        #endregion

        #region Implementation of IBoolValueViewModel

        public bool BoolValueProperty
        {
            get { return _boolValueProperty; }
            set
            {
                _boolValueProperty = value; 
                RaisePropertyChanged();
                
            }
        }

        #endregion
    }
}
