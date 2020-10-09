using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.ConfigurationSettings;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Editor.ViewModels.DependentProperty;
using Unicon2.Fragments.Configuration.Editor.ViewModels.ElementAdding;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.FragmentSettings;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
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

            container.Register<IPropertyEditorViewModel, PropertyEditorViewModel>();
            container.Register<IConfigurationGroupEditorViewModel, ConfigurationGroupEditorViewModel>();
            container.Register<IDependentPropertyEditorViewModel, DependentPropertyEditorViewModel>();
            container.Register<IComplexPropertyEditorViewModel, ComplexPropertyEditorViewModel>();
            container.Register<ISubPropertyEditorViewModel, SubPropertyEditorViewModel>();
            
            
            container.Register<IElementAddingCommand, ElementAddingCommand>();

            container.Register(typeof(IFragmentSettingViewModel),
                typeof(ActivatedConfigurationSettingViewModel),
                ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<IConditionViewModel, ConditionViewModel>();
            container.Register<DependencyFillHelper>();
            container.Register<ResultFillHelper>();
            container.Register<ConditionFillHelper>();

            //регистрация ресурсов
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/DeviceStructDataTemplates.xaml",
                GetType().Assembly);
            
            container.Register<Interfaces.Dependencies.IConditionViewModel,CompareResourceConditionViewModel>();
        }
    }
}
