using System.Windows.Input;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources
{
    public interface IResourcesAddingViewModel:IViewModel
    {
        bool IsResourceAdded { get; }
        string NameKey { get; set; }
        ICommand SubmitCommand { get; }
        ICommand CloseCommand { get; }
        void Initialize(IDeviceSharedResources deviceSharedResources);
    }
}