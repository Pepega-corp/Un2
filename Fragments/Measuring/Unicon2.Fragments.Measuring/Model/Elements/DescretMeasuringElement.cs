using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
	[JsonObject(MemberSerialization.OptIn)]

	public class DescretMeasuringElement : MeasuringElementBase, IDiscretMeasuringElement
	{

		public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT;

		[JsonProperty] public IAddressOfBit AddressOfBit { get; set; }

		public ushort Address => this.AddressOfBit.Address;

		public ushort NumberOfPoints => 1;
	}
}