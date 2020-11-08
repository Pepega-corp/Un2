using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class ErrorValueViewModel : FormattableValueViewModelBase, IErrorValueViewModel
    {
        private string _errorMessage;

        public override string StrongName => nameof(ErrorValueViewModel);
        public override string AsString()
        {
	        return ErrorMessage;
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