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

		bool CheckDeviceSharedResourcesContainsModel(INameable resource);
        bool CheckDeviceSharedResourcesWithContainersContainsModel(object resource);

		bool CheckDeviceSharedResourcesContainsViewModel(string resourceName);
        bool CheckDeviceSharedResourcesContainsViewModel(object viewModel);

		INameable GetResourceViewModel(string name);
        void AddAsSharedResource(INameable resourceToAdd);
        void AddAsSharedResourceWithContainer(INameable resourceToAdd);

		void AddSharedResourceViewModel(INameable resourceToAdd,string nameOfTheResource);

        INameable GetOrAddResourceModelFromCache(string name, Func<INameable> factoryIfEmpty);
        IDeviceSharedResources GetSharedResources();

        void AddExistingResourceWithContainer(object viewModel, object resourceModel);
        void AddResourceFromViewModel(object viewModel,object resourceModel);
        void ClearCaches();
    }
}