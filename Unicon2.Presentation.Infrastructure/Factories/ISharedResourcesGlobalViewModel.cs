using System;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface ISharedResourcesGlobalViewModel
    {
        void InitializeFromResources(IDeviceSharedResources deviceSharedResources);
        void OpenSharedResourcesForEditing();
        T OpenSharedResourcesForSelecting<T>();
        bool CheckDeviceSharedResourcesContainsModel(INameable resource);
        bool CheckDeviceSharedResourcesContainsViewModel(INameable resource);
        INameable GetResourceViewModel(string name);
        void AddAsSharedResource(INameable resourceToAdd);
        void AddSharedResourceViewModel(INameable resourceToAdd);

        INameable GetOrAddResourceModelFromCache(string name, Func<INameable> factoryIfEmpty);
        IDeviceSharedResources GetSharedResources();
    }
}