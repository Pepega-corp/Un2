using System;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IConnectable : IDisposable
    {
        void InitializeConnection(IDeviceConnection deviceConnection);
        IDeviceConnection DeviceConnection { get;  }
        string Name { get; set; }
        IConnectionState ConnectionState { get; set; }
    }
}