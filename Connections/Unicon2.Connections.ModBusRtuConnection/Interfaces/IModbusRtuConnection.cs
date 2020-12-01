using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Interfaces
{
    public interface IModbusRtuConnection : IDeviceConnection, IDataProvider
    {
        string PortName { get; set; }
        byte SlaveId { get; set; }
        IComPortConfiguration ComPortConfiguration { get; set; }
        List<string> GetAvailablePorts();
    }
}