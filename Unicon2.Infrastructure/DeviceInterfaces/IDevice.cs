using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IDevice : IConnectable, ISerializableInFile
    {
        IEnumerable<IDeviceFragment> DeviceFragments { get; set; }
        IDeviceSharedResources DeviceSharedResources { get; set; }
        IDeviceLogger DeviceLogger { get; set; }
        string DeviceSignature { get; set; }

    }
}