using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Model.Memory
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceMemory : IDeviceMemory
    {
        public DeviceMemory()
        {
            DeviceMemoryValues = new Dictionary<ushort, ushort>();
            LocalMemoryValues = new Dictionary<ushort, ushort>();
        }

        [JsonProperty] public Dictionary<ushort, ushort> DeviceMemoryValues { get; set; }
        [JsonProperty] public Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
    }
}