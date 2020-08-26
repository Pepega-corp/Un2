using Unicon2.Connections.OfflineConnection.Interfaces;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.OfflineConnection.ViewModels
{
    public class OfflineConnectionViewModel : ViewModelBase, IOfflineConnectionViewModel
    {
        public OfflineConnectionViewModel()
        {
            DeviceConnection = new OfflineConnection();
        }

        public IDeviceConnection DeviceConnection { get; set; }

        public string StrongName => nameof(OfflineConnectionViewModel);
        public object Model
        {
            get => DeviceConnection;
            set
            {
                if (value is OfflineConnection)
                    DeviceConnection = (OfflineConnection) value;
            }
        }

        public string ConnectionName
        {
            get { return DeviceConnection.ConnectionName; }
        }
    }
}
