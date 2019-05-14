using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(DefaultVariableSignature), IsReference = true)]

    public class DefaultVariableSignature : IVariableSignature
    {
        [DataMember]
        public string Signature { get; set; }
        [DataMember]
        public bool IsMultipleAssgnmentAllowed { get; set; }
    }
}
