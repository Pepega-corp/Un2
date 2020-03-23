using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;

namespace Unicon2.Fragments.Measuring.Model.Address
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AddressOfBit : IAddressOfBit
    {
        [JsonProperty]
        public int NumberOfFunction { get; set; }
        [JsonProperty]
        public ushort Address { get; set; }
        [JsonProperty]
        public ushort BitAddressInWord { get; set; }
    }
}
