using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(DefaultVariableColumnSignature), IsReference = true)]

    public class DefaultVariableColumnSignature : IVariableColumnSignature
    {
        [DataMember]
        public string Signature { get; set; }
        [DataMember]
        public bool IsMultipleAssignmentAllowed { get; set; }
    }

}

