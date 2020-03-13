using Unicon2.Connections.ModBusTcpConnection.Interfaces;
using Unicon2.Connections.ModBusTcpConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusTcpConnection.Factories
{
    public class ModBusTcpConnectionFactory : IDeviceConnectionFactory
    {
        private readonly ITypesContainer _container;

        public ModBusTcpConnectionFactory(ITypesContainer container)
        {
            _container = container;
        }


        public IDeviceConnection CreateDeviceConnection()
        {
            return _container.Resolve<IModbusTcpConnection>();
        }

        public IViewModel CreateDeviceConnectionViewModel()
        {
            IViewModel deviceConnectionViewModel = _container.Resolve<IModbusTcpConnectionViewModel>(ModBusTcpKeys.MODBUS_TCP_CONNECTION 
                + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            deviceConnectionViewModel.Model = _container.Resolve<IDeviceConnection>(ModBusTcpKeys.MODBUS_TCP_CONNECTION);
            return deviceConnectionViewModel;
        }
    }
}
