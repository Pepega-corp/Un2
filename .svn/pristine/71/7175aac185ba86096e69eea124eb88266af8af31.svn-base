using System;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;

namespace Unicon2.Infrastructure.Interfaces.Factories
{
    public interface ISharedResourcesViewModelFactory
    {
        void InitializeFromResources(IDeviceSharedResources deviceSharedResources);
        void OpenSharedResourcesForEditing();
        INameable OpenSharedResourcesForSelecting(Type typeNeeded);
        bool CheckDeviceSharedResourcesContainsElement(INameable resource);
        void AddSharedResource(INameable resourceToAdd);
    }
}