using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]

    public class DefaultMatrixMemoryVariable : IMatrixMemoryVariable
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public ushort StartAddressWord { get; set; }
        [JsonProperty]
        public ushort StartAddressBit { get; set; }
    }
}
