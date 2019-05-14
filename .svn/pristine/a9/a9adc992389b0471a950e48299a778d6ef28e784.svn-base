using Unicon2.Infrastructure;
using Unicon2.ModuleDeviceEditing.ViewModels;
using Unicon2.ModuleDeviceEditing.Views;
using Unicon2.Unity.Interfaces;

namespace Unicon2.ModuleDeviceEditing
{
    public class ModuleDeviceEditingModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<DeviceEditingViewModel>();
            container.Register<object, DeviceEditingView>(ApplicationGlobalNames.ViewNames.DEVICEEDITING_VIEW_NAME);
        }
    }
}