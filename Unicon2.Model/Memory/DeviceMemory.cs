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
            DeviceMemoryBoolValues = new Dictionary<ushort, bool>();
            LocalMemoryBoolValues = new Dictionary<ushort, bool>();
        }

        [JsonProperty] public Dictionary<ushort, ushort> DeviceMemoryValues { get; set; }
        [JsonProperty] public Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
        [JsonProperty] public Dictionary<ushort, bool> DeviceMemoryBoolValues { get; set; }
        [JsonProperty] public Dictionary<ushort, bool> LocalMemoryBoolValues { get; set; }
    }
}