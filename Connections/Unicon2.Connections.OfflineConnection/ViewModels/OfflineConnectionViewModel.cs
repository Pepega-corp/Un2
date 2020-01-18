using Unicon2.Connections.OfflineConnection.Interfaces;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.OfflineConnection.ViewModels
{
    public class OfflineConnectionViewModel : ViewModelBase, IOfflineConnectionViewModel
    {
        private IDeviceConnection _model;


        public OfflineConnectionViewModel()
        {
            this.DeviceConnection = new OfflineConnection();
        }

        public IDeviceConnection DeviceConnection { get; set; }

        public string StrongName => nameof(OfflineConnectionViewModel);
        public object Model
        {
            get => this.DeviceConnection;
            set
            {
                if (value is OfflineConnection)
                    this.DeviceConnection = (OfflineConnection) value;
            }
        }

        public string ConnectionName
        {
            get { return this.DeviceConnection.ConnectionName; }
        }
    }
}
