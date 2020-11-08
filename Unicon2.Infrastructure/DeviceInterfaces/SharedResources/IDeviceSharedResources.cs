using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.DeviceInterfaces.SharedResources
{
    public interface IDeviceSharedResources : IDisposable
    {
        List<INameable> SharedResources { get; set; }
		List<IResourceContainer> SharedResourcesInContainers { get; set; }

	}
}