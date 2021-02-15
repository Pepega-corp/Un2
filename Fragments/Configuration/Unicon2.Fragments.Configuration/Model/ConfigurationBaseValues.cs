using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigurationBaseValues : IConfigurationBaseValues
    {
        [JsonProperty] public List<IConfigurationBaseValue> BaseValues { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigurationBaseValue : IConfigurationBaseValue
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
    }
}