using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Validators
{
    public class ConfigurationEditorViewModelValidator
    {
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private readonly ILocalizerService _localizerService;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;

        public ConfigurationEditorViewModelValidator(IFormatterEditorFactory formatterEditorFactory,
            ILocalizerService localizerService, ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
        {
            _formatterEditorFactory = formatterEditorFactory;
            _localizerService = localizerService;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
        }


        public List<EditorValidationErrorViewModel> CheckItem(List<EditorValidationErrorViewModel> initialErrors,
            IConfigurationItemViewModel configurationEditorItemViewModel,
            IConfigurationEditorViewModel configurationEditorViewModel)
        {
            if (configurationEditorItemViewModel is IPropertyEditorViewModel propertyEditorViewModel &&
                !(configurationEditorItemViewModel is IComplexPropertyEditorViewModel))
            {
                if (propertyEditorViewModel.FormatterParametersViewModel == null ||
                    (propertyEditorViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel == null &&
                     !propertyEditorViewModel.FormatterParametersViewModel.IsFromSharedResources))
                {
                    initialErrors.Add(new EditorValidationErrorViewModel(
                        $"{propertyEditorViewModel.Name} ({_localizerService.GetLocalizedString("Address")}:{propertyEditorViewModel.Address}): {_localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.EMPTY_FORMATTING_MESSAGE)}",
                        Result<Action>.Create(
                            () =>
                            {
                                _formatterEditorFactory.EditFormatterByUser(propertyEditorViewModel,
                                    configurationEditorViewModel.RootConfigurationItemViewModels.ToList());
                            }, true)));
                }

                if (propertyEditorViewModel.FormatterParametersViewModel != null &&

                    propertyEditorViewModel.FormatterParametersViewModel.IsFromSharedResources &&
                    _sharedResourcesGlobalViewModel.GetResourceByName(propertyEditorViewModel
                        .FormatterParametersViewModel.Name) == null)
                {
                    initialErrors.Add(new EditorValidationErrorViewModel(
                        $"{propertyEditorViewModel.Name} ({_localizerService.GetLocalizedString("Address")}:{propertyEditorViewModel.Address}): {_localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMATTING_RESOURCE_NOT_FOUND_MESSAGE)}",
                        Result<Action>.Create(
                            () =>
                            {
                                _formatterEditorFactory.EditFormatterByUser(propertyEditorViewModel,
                                    configurationEditorViewModel.RootConfigurationItemViewModels.ToList());
                            }, true)));
                }


            }

            if (configurationEditorItemViewModel is IConfigurationGroupEditorViewModel configurationGroupEditorViewModel
            )
            {
                configurationGroupEditorViewModel.ChildStructItemViewModels.ForEach(model =>
                    CheckItem(initialErrors, model, configurationEditorViewModel));
            }

            if (configurationEditorItemViewModel is IComplexPropertyEditorViewModel complexPropertyEditorViewModel)
            {
                complexPropertyEditorViewModel.SubPropertyEditorViewModels.ForEach(model =>
                    CheckItem(initialErrors, model, configurationEditorViewModel));
            }

            return initialErrors;
        }

        public Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>> CreateValidator()
        {
            return fragmentEditorViewModel =>
            {

                var configurationEditorViewModel = fragmentEditorViewModel as ConfigurationEditorViewModel;
                return configurationEditorViewModel.RootConfigurationItemViewModels
                    .SelectMany(model => CheckItem(new List<EditorValidationErrorViewModel>(), model,
                        configurationEditorViewModel)).ToList();

            };
        }
    }
}