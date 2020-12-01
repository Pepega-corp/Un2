using Unicon2.Connections.OfflineConnection.ViewModels;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Connections.OfflineConnection
{
    public class OfflineConnectionFactory : IDeviceConnectionFactory
    {
        public IDeviceConnection CreateDeviceConnection()
        {
            return new OfflineConnection();
        }

        public IViewModel CreateDeviceConnectionViewModel()
        {
            return new OfflineConnectionViewModel();
        }
    }
}