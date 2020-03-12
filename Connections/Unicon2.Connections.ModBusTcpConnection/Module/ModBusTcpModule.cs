using Unicon2.Connections.ModBusTcpConnection.Factories;
using Unicon2.Connections.ModBusTcpConnection.Interfaces;
using Unicon2.Connections.ModBusTcpConnection.Keys;
using Unicon2.Connections.ModBusTcpConnection.Model;
using Unicon2.Connections.ModBusTcpConnection.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusTcpConnection.Module
{
    public class ModBusTcpModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IDeviceConnection), typeof(ModbusTcpConnection),
                ModBusTcpKeys.MODBUS_TCP_CONNECTION);
            container.Register(typeof(IModbusTcpConnection), typeof(ModbusTcpConnection));
            container.Register(typeof(IModbusTcpConnectionViewModel), typeof(ModbusTcpConnectionViewModel),
                ModBusTcpKeys.MODBUS_TCP_CONNECTION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            //регистрация фабрики 
           // container.Register<IDeviceConnectionFactory, ModBusTcpConnectionFactory>(ModBusTcpKeys.MODBUSTCP_CONNECTION_FACTORY_NAME);

           // container.Resolve<ISerializerService>().AddKnownTypeForSerialization(typeof(ModbusTcpConnection));
           // container.Resolve<ISerializerService>().AddNamespaceAttribute("modbusTcpConnection", "ModbusTcpConnectionNS");

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/ModBusTcpConnectionResources.xaml", this.GetType().Assembly);
        }
    }
}
