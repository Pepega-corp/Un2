using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Connections.ModBusTcpConnection.Interfaces
{
    public interface IModbusTcpConnection : IDeviceConnection, IDataProvider
    {
        int Port { get; set; }
        string IpAddress { get; set; }
    }
}