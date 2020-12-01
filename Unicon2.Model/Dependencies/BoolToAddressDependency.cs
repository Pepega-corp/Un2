using Newtonsoft.Json;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.Dependencies
{
	[JsonObject(MemberSerialization.OptIn)]
	public class BoolToAddressDependency : IBoolToAddressDependency
	{
		[JsonProperty] public string RelatedResourceName { get; set; }
		[JsonProperty] public ushort ResultingAddressIfTrue { get; set; }
		[JsonProperty] public ushort ResultingAddressIfFalse { get; set; }
		[JsonProperty] public IUshortsFormatter FormatterIfTrue { get; set; }
		[JsonProperty] public IUshortsFormatter FormatterIfFalse { get; set; }
	}
}
