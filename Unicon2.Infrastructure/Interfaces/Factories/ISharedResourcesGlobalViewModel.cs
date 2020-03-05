using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;

namespace Unicon2.Infrastructure.Interfaces.Factories
{
    public interface ISharedResourcesGlobalViewModel
    {
        void InitializeFromResources(IDeviceSharedResources deviceSharedResources);
        void OpenSharedResourcesForEditing();
        T OpenSharedResourcesForSelecting<T>();
        bool CheckDeviceSharedResourcesContainsElement(INameable resource);
        void AddSharedResource(INameable resourceToAdd);
    }
}