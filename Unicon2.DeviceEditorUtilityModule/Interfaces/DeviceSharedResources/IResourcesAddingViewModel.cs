using System.Windows.Input;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources
{
    public interface IResourcesAddingViewModel
    {
        bool IsResourceAdded { get; }
        ICommand SubmitCommand { get; }
        ICommand CloseCommand { get; }
        INameable ResourceViewModel { get; set; }
    }
}