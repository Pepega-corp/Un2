using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Validators
{
    public class MissingDependenciesConfigurationValidator
    {
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private readonly ILocalizerService _localizerService;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private readonly IDependenciesService _dependenciesService;
        private readonly DependencyFillHelper _dependencyFillHelper;

        public MissingDependenciesConfigurationValidator(IFormatterEditorFactory formatterEditorFactory,
            ILocalizerService localizerService, ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,
            IDependenciesService dependenciesService, DependencyFillHelper dependencyFillHelper)
        {
            _formatterEditorFactory = formatterEditorFactory;
            _localizerService = localizerService;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            _dependenciesService = dependenciesService;
            _dependencyFillHelper = dependencyFillHelper;
        }


        public List<EditorValidationErrorViewModel> CheckItem(List<EditorValidationErrorViewModel> initialErrors,
            IConfigurationItemViewModel configurationEditorViewModel)
        {
            if (configurationEditorViewModel is IPropertyEditorViewModel propertyEditorViewModel &&
                !(configurationEditorViewModel is IComplexPropertyEditorViewModel))
            {
                var dependenciesWithResources = propertyEditorViewModel.DependencyViewModels
                    .Where(model => model is ConditionResultDependencyViewModel)
                    .Select(model => (model as ConditionResultDependencyViewModel).SelectedConditionViewModel)
                    .Where(model => model is CompareResourceConditionViewModel)
                    .Cast<CompareResourceConditionViewModel>()
                    .ToList();

                if (dependenciesWithResources.Any(model =>
                        string.IsNullOrEmpty(model.ReferencedResourcePropertyName)) ||
                    dependenciesWithResources.Any(model =>
                       !_sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesContainsViewModel(
                            model.ReferencedResourcePropertyName)))
                {
                    initialErrors.Add(new EditorValidationErrorViewModel(
                        $"{propertyEditorViewModel.Name} ({_localizerService.GetLocalizedString("Address")}:{propertyEditorViewModel.Address}): {_localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.RESOURCE_FOR_DEPENDENCY_NOT_FOUND_MESSAGE)}",
                        Result<Action>.Create(
                            () =>
                            {
                                _dependenciesService.EditDependencies(propertyEditorViewModel,
                                    new DependenciesConfiguration(("ConditionResultDependency",
                                        () => _dependencyFillHelper.CreateEmptyConditionResultDependencyViewModel())));
                            }, true)));
                }

            }

            if (configurationEditorViewModel is IConfigurationGroupEditorViewModel configurationGroupEditorViewModel)
            {
                configurationGroupEditorViewModel.ChildStructItemViewModels.ForEach(model =>
                    CheckItem(initialErrors, model));
            }

            if (configurationEditorViewModel is IComplexPropertyEditorViewModel complexPropertyEditorViewModel)
            {
                complexPropertyEditorViewModel.SubPropertyEditorViewModels.ForEach(model =>
                    CheckItem(initialErrors, model));
            }

            return initialErrors;
        }


        public Func<IFragmentEditorViewModel, List<EditorValidationErrorViewModel>> CreateValidator()
        {
            return fragmentEditorViewModel =>
            {

                var configurationEditorViewModel = fragmentEditorViewModel as ConfigurationEditorViewModel;
                return configurationEditorViewModel.RootConfigurationItemViewModels
                    .SelectMany(model => CheckItem(new List<EditorValidationErrorViewModel>(), model)).ToList();

            };
        }
    }
}