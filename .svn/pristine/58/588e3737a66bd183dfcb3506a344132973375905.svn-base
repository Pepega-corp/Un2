using Prism.Mvvm;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Values
{
   public class BitGroupValueViewModel:BindableBase, IBitGroupValueViewModel
    {
        private object _value;
        private string _header;

        #region Implementation of IBitGroupValueViewModel

        public string StrongName => nameof(BitGroupValueViewModel);

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                RaisePropertyChanged();
            }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value; 
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of IFormattedValueViewModel

        public void InitFromValue(IFormattedValue value)
        {
            if (value is IBitGroupValue)
            {
                Header = (value as IBitGroupValue).Header;
                Value = (value as IBitGroupValue).Value;
                if ((value as IBitGroupValue).IsMeasureUnitEnabled)
                {
                    MeasureUnit = (value as IBitGroupValue).MeasureUnit;
                }
            }
        }

        #endregion

        #region Implementation of IMeasurable

        public string MeasureUnit { get; set; }

        public bool IsMeasureUnitEnabled { get; set; }

        #endregion

    
    }
}
