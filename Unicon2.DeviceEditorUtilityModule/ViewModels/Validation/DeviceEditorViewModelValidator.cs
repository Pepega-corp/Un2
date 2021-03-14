using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels.Validation
{
    public class DeviceEditorViewModelValidator : IDeviceEditorViewModelValidator
    {
        private Dictionary<Type, List<Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>>>>
            _validatorsDictionary =
                new Dictionary<Type, List<Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>>>>();


        public List<EditorValidationErrorViewModel> ValidateDeviceEditor(
            List<IFragmentEditorViewModel> fragmentEditorViewModels)
        {
            return fragmentEditorViewModels
                .SelectMany(model => _validatorsDictionary[model.GetType()]
                    .SelectMany(validator => validator(model))).ToList();
        }

        public void RegisterFragmentValidator<T>(
            Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>> validator)
            where T : IFragmentEditorViewModel
        {
            _validatorsDictionary.AddElementInList(typeof(T), validator);
        }
    }
}