using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels.Validation
{
    public class DeviceEditorViewModelValidator : IDeviceEditorViewModelValidator
    {
        private Dictionary<Type, List<Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>>>>
            _validatorsDictionary =
                new Dictionary<Type, List<Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>>>>();


        private List<EditorValidationErrorViewModel> GetValidatedListFromFragment(
            List<Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>>> validator,
            IFragmentEditorViewModel fragmentEditorViewModel)
        {
            return validator.SelectMany(func => func(fragmentEditorViewModel)).ToList();
        }

        public List<EditorValidationErrorViewModel> ValidateDeviceEditor(
            List<IFragmentEditorViewModel> fragmentEditorViewModels)
        {
            var validators =
                fragmentEditorViewModels.Select(model =>
                    _validatorsDictionary.GetElement(model.GetType())
                        .OnSuccess<List<EditorValidationErrorViewModel>>(list => list.SelectMany(func => func(model)).ToList())).Choose().SelectMany(list =>list);
            return validators.ToList();
        }

        public void RegisterFragmentValidator<T>(
            Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>> validator)
            where T : IFragmentEditorViewModel
        {
            _validatorsDictionary.AddElementInList(typeof(T), validator);
        }
    }
}