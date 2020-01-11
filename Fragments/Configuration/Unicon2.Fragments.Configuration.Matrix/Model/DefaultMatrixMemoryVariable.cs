using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(DefaultMatrixMemoryVariable), IsReference = true)]

    public class DefaultMatrixMemoryVariable : IMatrixMemoryVariable
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ushort StartAddressWord { get; set; }
        [DataMember]
        public ushort StartAddressBit { get; set; }
    }
}
