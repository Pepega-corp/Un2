using System;
using System.IO.Ports;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IComPortConfiguration : ICloneable
    {
        int BaudRate { get; set; }
        int DataBits { get; set; }
        StopBits StopBits { get; set; }
        Parity Parity { get; set; }
        int WaitAnswer { get; set; }
        int WaitByte { get; set; }
        int OnTransmission { get; set; }
        int OffTramsmission { get; set; }
    }
}