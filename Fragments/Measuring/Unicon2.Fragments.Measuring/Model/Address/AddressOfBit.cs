using System.Runtime.Serialization;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;

namespace Unicon2.Fragments.Measuring.Model.Address
{
    [DataContract(Namespace = "AddressOfBitNS")]
    public class AddressOfBit : IAddressOfBit
    {
        [DataMember]
        public int NumberOfFunction { get; set; }
        [DataMember]
        public ushort Address { get; set; }
        [DataMember]
        public ushort BitAddressInWord { get; set; }
    }
}
