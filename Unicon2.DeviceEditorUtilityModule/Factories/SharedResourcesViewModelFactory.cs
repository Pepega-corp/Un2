using System;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Unity.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.Factories
{
    public class SharedResourcesViewModelFactory : ISharedResourcesViewModelFactory
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ITypesContainer _container;
        private IDeviceSharedResources _deviceSharedResources;
        private bool _isInitialized;
        public SharedResourcesViewModelFactory(IApplicationGlobalCommands applicationGlobalCommands, ITypesContainer container)
        {
            this._applicationGlobalCommands = applicationGlobalCommands;
            this._container = container;
        }

        public void InitializeFromResources(IDeviceSharedResources deviceSharedResources)
        {
            this._deviceSharedResources = deviceSharedResources;
            this._isInitialized = true;
        }

        public void OpenSharedResourcesForEditing()
        {
            if (!this._isInitialized) throw new Exception();
            IDeviceSharedResourcesViewModel deviceSharedResourcesViewModel = this._container.Resolve<IDeviceSharedResourcesViewModel>();
          //  deviceSharedResourcesViewModel.Model = this._deviceSharedResources;
            deviceSharedResourcesViewModel.IsSelectingMode = false;
            this._applicationGlobalCommands.ShowWindowModal(() => new DeviceSharedResourcesView(), deviceSharedResourcesViewModel);

        }

        public INameable OpenSharedResourcesForSelecting(Type typeNeeded)
        {
            if (!this._isInitialized) throw new Exception();
            IDeviceSharedResourcesViewModel deviceSharedResourcesViewModel = this._container.Resolve<IDeviceSharedResourcesViewModel>();
          //  deviceSharedResourcesViewModel.Model = this._deviceSharedResources;
            deviceSharedResourcesViewModel.IsSelectingMode = true;
            deviceSharedResourcesViewModel.Initialize(typeNeeded);
            this._applicationGlobalCommands.ShowWindowModal((() => new DeviceSharedResourcesView()), deviceSharedResourcesViewModel);
            if (deviceSharedResourcesViewModel.SelectedResourceViewModel == null) return null;
            return deviceSharedResourcesViewModel.SelectedResourceViewModel.Model as INameable;
        }

        public bool CheckDeviceSharedResourcesContainsElement(INameable resource)
        {
            return this._deviceSharedResources.IsItemReferenced(resource.Name);
        }

        public void AddSharedResource(INameable resourceToAdd)
        {
            if (!this._isInitialized) throw new Exception();
            IResourcesAddingViewModel resourcesAddingViewModel = this._container.Resolve<IResourcesAddingViewModel>();
            resourcesAddingViewModel.Model = resourceToAdd;
            resourcesAddingViewModel.Initialize(this._deviceSharedResources);
            this._applicationGlobalCommands.ShowWindowModal(() => new ResourcesAddingWindow(), resourcesAddingViewModel);
        }
    }
}
