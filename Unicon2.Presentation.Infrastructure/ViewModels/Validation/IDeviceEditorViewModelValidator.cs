using System;
using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Validation
{
    public interface IDeviceEditorViewModelValidator
    {
        List<EditorValidationErrorViewModel> ValidateDeviceEditor(
            List<IFragmentEditorViewModel> fragmentEditorViewModels);

        void RegisterFragmentValidator<T>(Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>> validator)
            where T : IFragmentEditorViewModel;
    }
}