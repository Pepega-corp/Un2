using Unicon2.DeviceEditorUtilityModule.Factories;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.DeviceEditorUtilityModule.ViewModels;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.Module
{
    public class DeviceEditorUtilityModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<DeviceEditorViewModel>();
            container.Register<object, DeviceEditorView>(ApplicationGlobalNames.ViewNames.DEVICEEDITOR_VIEW_NAME);

            container.Register(typeof(IResultingDeviceViewModel), typeof(ResultingDeviceViewModel));
            container.Register(typeof(ISharedResourcesViewModelFactory), typeof(SharedResourcesViewModelFactory), true);
            container.Register(typeof(IDeviceSharedResourcesViewModel), typeof(DeviceSharedResourcesViewModel));
            container.Register(typeof(IResourcesAddingViewModel), typeof(ResourcesAddingViewModel));
            container.Register(typeof(IResourceViewModel), typeof(ResourceViewModel));
            container.Register(typeof(ISharedResourcesEditorFactory), typeof(SharedResourcesEditorFactory));
            container.Register(typeof(IResourceEditingViewModel), typeof(ResourceEditingViewModel));
            container.Register(typeof(IConnectionStateViewModelFactory), typeof(ConnectionStateViewModelFactory));
            container.Register(typeof(IConnectionStateViewModel), typeof(ConnectionStateViewModel));
        }
    }
}
