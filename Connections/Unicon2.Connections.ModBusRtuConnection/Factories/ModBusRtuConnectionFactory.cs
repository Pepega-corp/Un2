using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Factories
{
    /// <summary>
    /// фабрика класса подключения по ModBusRTu
    /// </summary>
    public class ModBusRtuConnectionFactory : IDeviceConnectionFactory
    {
        private readonly ITypesContainer _container;

        public ModBusRtuConnectionFactory(ITypesContainer container)
        {
            this._container = container;
        }
        /// <summary>
        /// создание объекта класса подключения
        /// </summary>
        /// <returns></returns>
        public IDeviceConnection CreateDeviceConnection()
        {
            return this._container.Resolve<IModbusRtuConnection>();
        }

        public IViewModel CreateDeviceConnectionViewModel()
        {
            IViewModel deviceConnectionViewModel = this._container.Resolve<IDeviceConnectionViewModel>(StringKeys.MODBUS_RTU_CONNECTION
                + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            deviceConnectionViewModel.Model = this._container.Resolve<IDeviceConnection>(StringKeys.MODBUS_RTU_CONNECTION);
            ((IModbusRtuConnection)deviceConnectionViewModel.Model).SlaveId = 1;
            return deviceConnectionViewModel;
        }
    }
}
