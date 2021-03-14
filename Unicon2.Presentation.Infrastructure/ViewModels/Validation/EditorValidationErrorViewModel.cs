using System;
using System.Windows.Input;
using Unicon2.Infrastructure.Functional;
using Unicon2.Unity.Commands;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Validation
{
    public class EditorValidationErrorViewModel
    {
        public EditorValidationErrorViewModel(string errorMessage, Result<Action> onOpenError)
        {
            ErrorMessage = errorMessage;
            onOpenError
                .OnSuccess(action => { OpenCommand = new RelayCommand(action); })
                .OnFail(_ => { OpenCommand = new RelayCommand(null, () => false); });
        }

        public string ErrorMessage { get; }
        public ICommand OpenCommand { get; set; }
    }
}