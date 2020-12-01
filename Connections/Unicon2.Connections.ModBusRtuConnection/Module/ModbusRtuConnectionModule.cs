using Unicon2.Connections.ModBusRtuConnection.Factories;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.ComPortInterrogation;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories;
using Unicon2.Connections.ModBusRtuConnection.Keys;
using Unicon2.Connections.ModBusRtuConnection.Model;
using Unicon2.Connections.ModBusRtuConnection.Services;
using Unicon2.Connections.ModBusRtuConnection.ViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Module
{
    public class ModbusRtuConnectionModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IDeviceConnection, Model.ModBusRtuConnection>(StringKeys.MODBUS_RTU_CONNECTION);
            container.Register<IModbusRtuConnection, Model.ModBusRtuConnection>();
            container.Register<IDeviceConnectionViewModel, ModBusConnectionViewModel>(StringKeys.MODBUS_RTU_CONNECTION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            //регистрация фабрики 
            container.Register<IDeviceConnectionFactory, ModBusRtuConnectionFactory>(StringKeys.MODBUSRTU_CONNECTION_FACTORY_NAME);

            container.Register<IComPortConfigurationFactory, ComPortConfigurationFactory>();
            container.Register<IComPortConfigurationViewModelFactory, ComPortConfigurationViewModelFactory>();
            container.Register<IComPortConfiguration, ComPortConfiguration>();
            container.Register<IComPortConfigurationViewModel, ComPortConfigurationViewModel>();
            container.Register<IComPortInterrogationViewModel, ComPortInterrogationViewModel>();
            container.Register<IModBusConnectionViewModel, ModBusConnectionViewModel>();

            //регистрация менеджера подключений
            container.Register<IComConnectionManager, ComConnectionManager>(true);
            
            //регистрация известных для сериализации типов
            //ISerializerService serializerService = container.Resolve<ISerializerService>();
            //serializerService.AddKnownTypeForSerialization(typeof(Model.ModBusRtuConnection));
            //serializerService.AddKnownTypeForSerialization(typeof(ComPortConfiguration));
            //serializerService.AddNamespaceAttribute("modBusRtuConnection", "ModBusRtuConnectionNS");
            //serializerService.AddNamespaceAttribute("comPortConfiguration", "ComPortConfigurationNS");

            //регистрация ресурсов
            IXamlResourcesService xamlResourcesService = container.Resolve<IXamlResourcesService>();
            xamlResourcesService.AddResourceAsGlobal("Resources/ModBusRtuConnectionResources.xaml", GetType().Assembly);
            xamlResourcesService.AddResourceAsGlobal("Resources/ComPortConfigurationResources.xaml", GetType().Assembly);
        }
    }
}