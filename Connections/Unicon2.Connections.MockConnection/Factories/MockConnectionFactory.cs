using Unicon2.Connections.MockConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.MockConnection.Factories
{   
     public class MockConnectionFactory : IDeviceConnectionFactory
    {
        private readonly ITypesContainer _container;

        public MockConnectionFactory(ITypesContainer container)
        {
            _container = container;
        }
        /// <summary>
        /// создание объекта класса подключения
        /// </summary>
        /// <returns></returns>
        public IDeviceConnection CreateDeviceConnection()
        {
            return _container.Resolve<Model.MockConnection>();
        }

        public IViewModel CreateDeviceConnectionViewModel()
        {
            IViewModel deviceConnectionViewModel = _container.Resolve<IDeviceConnectionViewModel>(StringKeys.MOCK_CONNECTION
                + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            deviceConnectionViewModel.Model = _container.Resolve<IDeviceConnection>(StringKeys.MOCK_CONNECTION);
            return deviceConnectionViewModel;
        }
    }
}
