using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
   public class ErrorValueViewModel:FormattableValueViewModelBase,IErrorValueViewModel
    {
        private string _errorMessage;

        public override string StrongName => nameof(ErrorValueViewModel);

        public override void InitFromValue(IFormattedValue value)
        {
            Header = value.Header;
            ErrorMessage = (value as IErrorValue).ErrorMessage;
            base.InitFromValue(value);
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value; 
                RaisePropertyChanged();
            }
        }
    }
}
