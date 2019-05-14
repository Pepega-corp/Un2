using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.Model.Base;
using Unicon2.Fragments.Configuration.Model.ConfigurationSettings;
using Unicon2.Fragments.Configuration.Model.DependentProperty;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Module
{
    public class ConfigurationModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IFragmentViewModel), typeof(RuntimeConfigurationViewModel),
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION_VIEWMODEL);

            container.RegisterInstance<IConfigurationItemFactory>(new ConfigurationItemFactory(container.Resolve<ITypesContainer>()));
            container.Register(typeof(IRuntimeConfigurationItemViewModelFactory), typeof(RuntimeConfigurationItemViewModelFactory));
            container.Register(typeof(IDependentProperty), typeof(DependentProperty));
            container.Register(typeof(IDependancyCondition), typeof(DependancyCondition));

            container.Register(typeof(IComplexProperty), typeof(ComplexProperty));
            container.Register(typeof(ISubProperty), typeof(SubProperty));
            container.Register(typeof(IDeviceConfiguration), typeof(DefaultDeviceConfiguration));
            container.Register(typeof(IItemsGroup), typeof(DefaultItemsGroup));
            container.Register(typeof(IProperty), typeof(DefaultProperty));

            container.Register(typeof(IConfigurationItemViewModel), typeof(RuntimeItemGroupViewModel),
                ConfigurationKeys.RUNTIME_DEFAULT_ITEM_GROUP +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IConfigurationItemViewModel), typeof(RuntimePropertyViewModel),
                ConfigurationKeys.RUNTIME_DEFAULT_PROPERTY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IConfigurationItemViewModel), typeof(RuntimeDependentPropertyViewModel),
                ConfigurationKeys.RUNTIME + ConfigurationKeys.DEPENDENT_PROPERTY +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register(typeof(IConfigurationItemViewModel), typeof(RuntimeComplexPropertyViewModel),
                ConfigurationKeys.RUNTIME + ConfigurationKeys.COMPLEX_PROPERTY +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IConfigurationItemViewModel), typeof(RuntimeSubPropertyViewModel),
                ConfigurationKeys.RUNTIME + ConfigurationKeys.SUB_PROPERTY +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register(typeof(IRuntimeConfigurationViewModel), typeof(RuntimeConfigurationViewModel));
            container.Register(typeof(IValueViewModelFactory), typeof(ValueViewModelFactory));

            container.Register(typeof(IFragmentSetting), typeof(ActivatedConfigurationSetting), ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING);

            ISerializerService serializerService = container.Resolve<ISerializerService>();
            serializerService.AddNamespaceAttribute("defaultDeviceConfiguration", "DefaultDeviceConfigurationNS");
            serializerService.AddNamespaceAttribute("defaultItemsGroup", "DefaultItemsGroupNS");
            serializerService.AddNamespaceAttribute("defaultProperty", "DefaultPropertyNS");
            serializerService.AddNamespaceAttribute("activatedConfigurationSetting",
                "ActivatedConfigurationSettingNS");
            serializerService.AddNamespaceAttribute("dependentProperty", "DependentPropertyNS");
            serializerService.AddNamespaceAttribute("dependancyCondition", "DependancyConditionNS");
            serializerService.AddNamespaceAttribute("complexProperty", "ComplexPropertyNS");
            serializerService.AddNamespaceAttribute("subProperty", "SubPropertyNS");
            serializerService.AddNamespaceAttribute("localDeviceValuesConfigurationItemBase", "LocalDeviceValuesConfigurationItemBaseNS");

            serializerService.AddKnownTypeForSerializationRange(new[]
            {
                typeof(LocalDeviceValuesConfigurationItemBase),typeof(DefaultDeviceConfiguration), typeof(DefaultItemsGroup), typeof(DefaultProperty),
                typeof(List<DefaultItemsGroup>), typeof(List<DefaultProperty>), typeof(ActivatedConfigurationSetting),
                typeof(DependentProperty), typeof(DependancyCondition), typeof(ComplexProperty), typeof(SubProperty)
            });

            //регистрация ресурсов
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/ConfigurationTemplates.xaml", this.GetType().Assembly);
        }
    }
}