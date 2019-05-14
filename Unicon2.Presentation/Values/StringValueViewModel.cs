using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
   public class StringValueViewModel:FormattableValueViewModelBase,IStringValueViewModel
    {
     
        private string _stringValue;

        #region Implementation of IStronglyNamed

        public override string StrongName => nameof(StringValueViewModel);

        #endregion

        #region Implementation of IFormattedValueViewModel

        //public object Value
        //{
        //    get { return _value; }
        //    set
        //    {
        //        _value = value; 
        //        RaisePropertyChanged();
        //    }
        //}

        //public string Header
        //{
        //    get { return _header; }
        //    set
        //    {
        //        _header = value; 
        //        RaisePropertyChanged();
        //    }
        //}

        public override void InitFromValue(IFormattedValue value)
        {
            Header = (value as IStringValue).Header;
            StringValue = (value as IStringValue).StrValue;
        }

        #endregion

        #region Implementation of IStringValueViewModel

        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                _stringValue = value; 
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}
