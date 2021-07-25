﻿using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;

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
        Result<INameable> GetResourceViewModelByName(string viewModelName);
        bool CheckDeviceSharedResourcesContainsViewModel(object viewModelName);

        void AddAsSharedResource(INameable resourceToAdd,bool askUser=true);
        void AddAsSharedResourceWithContainer(INameable resourceToAdd ,string initialName = null, bool askUser= true);
        void UpdateSharedResource(INameable resourceToAdd);
        void RemoveResourceByViewModel(object viewModel);

        IDeviceSharedResources GetSharedResources();
        INameable GetResourceByName(string name);
        Result<string> GetNameByResourceViewModel(object viewModel);
        void AddExistingResourceWithContainer(object viewModel, object resourceModel);
        void AddResourceFromViewModel(object viewModel,object resourceModel);
        void ClearCaches();
    }
}