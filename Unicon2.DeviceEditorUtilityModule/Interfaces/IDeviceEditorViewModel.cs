using System.Windows.Input;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces
{
    public interface IDeviceEditorViewModel
    {
        ICommand LoadExistingDevice { get; }
        ICommand CreateDeviceCommand { get; }
        ICommand SaveInFileCommand { get; }
        ICommand OpenSharedResourcesCommand { get; }
        ICommand DeleteFragmentCommand { get; }

        bool IsOpen { get; set; }
    }
}