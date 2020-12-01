using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model
{
	[JsonObject(MemberSerialization.OptIn)]

	public class PropertyResourceContainer : INameable
	{
		[JsonProperty] public IProperty RelatedProperty { get; set; }
		[JsonProperty] public string Name { get; set; }
	}
}