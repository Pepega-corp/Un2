
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.ConfigurationSettings;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Results;
using Unicon2.Fragments.Configuration.Editor.ViewModels.ElementAdding;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Validators;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.FragmentSettings;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Module
{
    public class ConfigurationEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IFragmentEditorViewModel), typeof(ConfigurationEditorViewModel),
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<PropertyEditorViewModel>();

            container.Register<IPropertyEditorViewModel, PropertyEditorViewModel>();
            container.Register<IConfigurationGroupEditorViewModel, ConfigurationGroupEditorViewModel>();
            container.Register<IComplexPropertyEditorViewModel, ComplexPropertyEditorViewModel>();
            container.Register<ISubPropertyEditorViewModel, SubPropertyEditorViewModel>();

            container.Register<IResultViewModel, ApplyFormatterResultViewModel>(
                ConfigurationKeys.APPLY_FORMATTER_RESULT);
            container.Register<IResultViewModel, BlockInteractionResultViewModel>(ConfigurationKeys
                .BLOCK_INTERACTION_RESULT);
            container.Register<IConditionViewModel, CompareResourceConditionViewModel>(ConfigurationKeys
                .COMPARE_RESOURCE_CONDITION);
            container.Register<IResultViewModel, HidePropertyResultViewModel>(ConfigurationKeys
                .HIDE_PROPERTY_RESULT);

            container.Register<IElementAddingCommand, ElementAddingCommand>();

            container.Register(typeof(IFragmentSettingViewModel),
                typeof(ActivatedConfigurationSettingViewModel),
                ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<DependencyFillHelper>();
            container.Register<ResultFillHelper>();
            container.Register<ConditionFillHelper>();
            container.Register<FilterFillHelper>();
            container.Register<BaseValuesFillHelper>();
            container.Register<ImportPropertiesFromExcelTypeAHelper>();

            container.Register<ConfigurationEditorViewModelValidator>();

            container.Resolve<IDeviceEditorViewModelValidator>()
                .RegisterFragmentValidator<ConfigurationEditorViewModel>(container
                    .Resolve<ConfigurationEditorViewModelValidator>().CreateValidator());

            container.Resolve<IDeviceEditorViewModelValidator>()
                .RegisterFragmentValidator<ConfigurationEditorViewModel>(container
                    .Resolve<MissingDependenciesConfigurationValidator>().CreateValidator());

            container.Resolve<IDeviceEditorViewModelValidator>()
                .RegisterFragmentValidator<ConfigurationEditorViewModel>(container
                    .Resolve<RegexConditionsValidator>().CreateValidator());
            //регистрация ресурсов
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/DeviceStructDataTemplates.xaml",
                GetType().Assembly);


        }
    }
}
