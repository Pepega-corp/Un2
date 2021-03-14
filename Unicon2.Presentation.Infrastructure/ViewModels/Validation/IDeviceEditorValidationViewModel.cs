using System.Collections.Generic;
using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Validation
{
    public interface IDeviceEditorValidationViewModel
    {
        List<EditorValidationErrorViewModel> ErrorViewModels { get; set; }
        bool IsSuccess { get; set; }
        ICommand RefreshErrors { get; }
    }
}