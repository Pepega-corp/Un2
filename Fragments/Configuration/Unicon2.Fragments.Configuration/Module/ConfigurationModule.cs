using Unicon2.Fragments.Configuration.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.Services;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.Model.ConfigurationSettings;
using Unicon2.Fragments.Configuration.Model.DependentProperty;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Fragments.Configuration.Services;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Module
{
    public class ConfigurationModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<ExportSelectionViewModel>();
            container.Register(typeof(IFragmentViewModel), typeof(RuntimeConfigurationViewModel),
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION_VIEWMODEL);

            container.RegisterInstance<IConfigurationItemFactory>(
                new ConfigurationItemFactory(container.Resolve<ITypesContainer>()));
            container.Register<IRuntimeConfigurationItemViewModelFactory,
                RuntimeConfigurationItemViewModelFactory>();
            container.Register(typeof(IDependancyCondition), typeof(DependancyCondition));

            container.Register(typeof(IComplexProperty), typeof(ComplexProperty));
            container.Register(typeof(ISubProperty), typeof(SubProperty));
            container.Register(typeof(IDeviceConfiguration), typeof(DefaultDeviceConfiguration));
            container.Register(typeof(IItemsGroup), typeof(DefaultItemsGroup));
            container.Register(typeof(IProperty), typeof(DefaultProperty));
            container.Register(typeof(IGroupWithReiterationInfo), typeof(GroupWithReiterationInfo));
            container.Register(typeof(IReiterationSubGroupInfo), typeof(ReiterationSubGroupInfo));

            container.Register(typeof(IRuntimeItemGroupViewModel), typeof(RuntimeItemGroupViewModel));
            container.Register(typeof(IRuntimePropertyViewModel), typeof(RuntimePropertyViewModel));
            container.Register(typeof(IRuntimeConfigurationViewModel), typeof(RuntimeConfigurationViewModel));
            container.Register<IPropertyValueService, PropertyValueService>();
            container.Register(typeof(IFragmentSetting), typeof(ActivatedConfigurationSetting),
                ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING);

            container.Register(typeof(IConfigurationTreeWalker), typeof(ConfigurationTreeWalker));

            //ISerializerService serializerService = container.Resolve<ISerializerService>();
            //serializerService.AddNamespaceAttribute("defaultDeviceConfiguration", "DefaultDeviceConfigurationNS");
            //serializerService.AddNamespaceAttribute("defaultItemsGroup", "DefaultItemsGroupNS");
            //serializerService.AddNamespaceAttribute("defaultProperty", "DefaultPropertyNS");
            //serializerService.AddNamespaceAttribute("activatedConfigurationSetting",
            //    "ActivatedConfigurationSettingNS");
            //serializerService.AddNamespaceAttribute("dependentProperty", "DependentPropertyNS");
            //serializerService.AddNamespaceAttribute("dependancyCondition", "DependancyConditionNS");
            //serializerService.AddNamespaceAttribute("complexProperty", "ComplexPropertyNS");
            //serializerService.AddNamespaceAttribute("subProperty", "SubPropertyNS");
            //serializerService.AddNamespaceAttribute("localDeviceValuesConfigurationItemBase",
            //    "LocalDeviceValuesConfigurationItemBaseNS");
            //serializerService.AddNamespaceAttribute("deviceMemory",
            //    "ConfigurationMemoryNS");
            //serializerService.AddNamespaceAttribute("addressValue",
            //    "AddressValueNS");
            //serializerService.AddKnownTypeForSerializationRange(new[]
            //{
            //    typeof(DefaultDeviceConfiguration),
            //    typeof(DefaultItemsGroup), typeof(DefaultProperty),
            //    typeof(List<DefaultItemsGroup>), typeof(List<DefaultProperty>), typeof(ActivatedConfigurationSetting),
            //    typeof(DependentProperty), typeof(DependancyCondition), typeof(ComplexProperty), typeof(SubProperty),
            //    typeof(GroupWithReiterationInfo), typeof(ReiterationSubGroupInfo),
            //    typeof(List<ReiterationSubGroupInfo>),
            //    typeof(DeviceMemory)
            //});

            //регистрация ресурсов
            container.Resolve<IXamlResourcesService>()
                .AddResourceAsGlobal("Resources/ConfigurationTemplates.xaml", GetType().Assembly);

            container.Resolve<ILoadAllService>().RegisterFragmentLoadHandler(
                ApplicationGlobalNames.FragmentInjectcionStrings.RUNTIME_CONFIGURATION_VIEWMODEL,
                LoadAllConfigurationHelper.GetConFigurationLoadingHelper());
        }
    }
}