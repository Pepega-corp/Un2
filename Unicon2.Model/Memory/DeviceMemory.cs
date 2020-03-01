using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Configuration.Model.Memory
{
    [DataContract(Name = nameof(DeviceMemory), Namespace = "ConfigurationMemoryNS", IsReference = true)]
    public class DeviceMemory : IDeviceMemory
    {
        public DeviceMemory()
        {
            DeviceMemoryValues = new Dictionary<ushort, ushort>();
            LocalMemoryValues = new Dictionary<ushort, ushort>();
        }

        [DataMember] public Dictionary<ushort, ushort> DeviceMemoryValues { get; set; }
        [DataMember] public Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
    }
}