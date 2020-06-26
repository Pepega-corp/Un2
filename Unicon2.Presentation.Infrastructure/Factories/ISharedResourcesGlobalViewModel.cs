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
        string OpenSharedResourcesForSelectingString<T>();

		//bool CheckDeviceSharedResourcesContainsModel(INameable resource);
        bool CheckDeviceSharedResourcesWithContainersContainsModel(object resource);
        bool CheckDeviceSharedResourcesContainsViewModel(string viewModelName);
        bool CheckDeviceSharedResourcesContainsViewModel(object viewModelName);

        void AddAsSharedResource(INameable resourceToAdd);
        void AddAsSharedResourceWithContainer(INameable resourceToAdd);


        IDeviceSharedResources GetSharedResources();
        INameable GetResourceByName(string name);
        void AddExistingResourceWithContainer(object viewModel, object resourceModel);
        void AddResourceFromViewModel(object viewModel,object resourceModel);
        void ClearCaches();
    }
}