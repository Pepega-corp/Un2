using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [DataContract(Namespace = "DescretMeasuringElementNS")]
    public class DescretMeasuringElement : MeasuringElementBase, IDiscretMeasuringElement
    {

        public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT;

        [DataMember]
        public IAddressOfBit AddressOfBit { get; set; }

        public ushort Address => this.AddressOfBit.Address;

        public ushort NumberOfPoints => 1;
    }
}
