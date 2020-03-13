using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Connections.ModBusTcpConnection.Interfaces
{
    public interface IModbusTcpConnection : IDeviceConnection, IDataProvider
    {
        int Port { get; set; }
        string IpAddress { get; set; }
    }
}