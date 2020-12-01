using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.DefaultDevice
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceSharedResources : Disposable, IDeviceSharedResources
    {
        public DeviceSharedResources()
        {
            SharedResources = new List<INameable>();
			SharedResourcesInContainers=new List<IResourceContainer>();
        }

        [JsonProperty]
        public List<INameable> SharedResources { get; set; }
		[JsonProperty]
        public List<IResourceContainer> SharedResourcesInContainers { get; set; }
    }
}