using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Interfaces
{
    public interface IModbusRtuConnection : IDeviceConnection, IDataProvider,IInitializableFromContainer
    {
        string PortName { get; set; }
        byte SlaveId { get; set; }
        IComPortConfiguration ComPortConfiguration { get; set; }
        List<string> GetAvailablePorts();
    }
}