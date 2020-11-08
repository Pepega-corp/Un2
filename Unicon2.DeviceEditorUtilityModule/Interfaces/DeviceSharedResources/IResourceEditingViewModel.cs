using System.Windows.Input;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources
{
    public interface IResourceEditingViewModel
    {
        object ResourceEditorViewModel { get; set; }
        ICommand CloseCommand { get; }
    }
}