using Unicon2.Infrastructure.Connection;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IDeviceCreator
    {

        string DeviceDescriptionFilePath { get; set; }
        string DeviceName { get; set; }
        IConnectionState ConnectionState { get; set; }
        IDeviceConnection AvailableConnection { get; set; }
        IDevice Create();
    }
}
