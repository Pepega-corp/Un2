using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultVariableColumnSignature : IVariableColumnSignature
    {
        [JsonProperty]
        public string Signature { get; set; }
        [JsonProperty]
        public bool IsMultipleAssignmentAllowed { get; set; }
    }

}

