using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources
{
    public interface IResourceEditingViewModel
    {
        object ResourceEditorViewModel { get; set; }
        ICommand CloseCommand { get; }
    }
}