using System.Collections.Generic;
using NModbus4.Serial;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Interfaces
{
    public interface IComConnectionManager
    {
        List<string> GetSerialPortNames();

        Dictionary<string, IComPortConfiguration> ComPortConfigurationsDictionary { get; set; }

        IComPortConfiguration GetComPortConfiguration(string portName);
        void SetComPortConfigurationByName(IComPortConfiguration comPortConfiguration, string portName);
        SerialPortAdapter GetSerialPortAdapter(string portName);
        SerialPortAdapter GetSerialPortAdapter(string portName,IComPortConfiguration comPortConfiguration);

    }
}