using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;

namespace Unicon2.Fragments.Measuring.Model.Address
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WritingValueContext : IWritingValueContext
    {
        [JsonProperty] public ushort Address { get; set; }
        [JsonProperty] public ushort Bit { get; set; }
        [JsonProperty] public ushort NumberOfFunction { get; set; }
        [JsonProperty] public ushort ValueToWrite { get; set; }
    }
}