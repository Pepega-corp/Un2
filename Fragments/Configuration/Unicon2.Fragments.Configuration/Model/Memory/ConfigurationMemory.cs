using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Model.Memory
{
    [DataContract(Name = nameof(ConfigurationMemory), Namespace = "ConfigurationMemoryNS", IsReference = true)]
    public class ConfigurationMemory : IConfigurationMemory
    {
        public ConfigurationMemory()
        {
            DeviceMemoryValues = new Dictionary<ushort, ushort>();
            LocalMemoryValues = new Dictionary<ushort, ushort>();
        }

        [DataMember] public Dictionary<ushort, ushort> DeviceMemoryValues { get; set; }
        [DataMember] public Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
    }
}