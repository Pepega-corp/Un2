using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Model
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