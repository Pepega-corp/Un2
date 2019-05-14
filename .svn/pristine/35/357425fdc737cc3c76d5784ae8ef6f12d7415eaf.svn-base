using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;

namespace Unicon2.Infrastructure.Interfaces.Resourcres
{
    public interface IInitializableFromResource : IResourceContaining
    {
        bool IsInitializeNeeded { get; set; }
        List<Guid> RelatedResourceGuidList { get; set; }
        IDeviceSharedResources DeviceSharedResources { get; }
    }
}