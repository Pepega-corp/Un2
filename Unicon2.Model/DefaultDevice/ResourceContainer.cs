using Newtonsoft.Json;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;

namespace Unicon2.Model.DefaultDevice
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ResourceContainer : IResourceContainer
	{
		[JsonProperty] public string ResourceName { get; set; }
		[JsonProperty] public object Resource { get; set; }
	}
}
