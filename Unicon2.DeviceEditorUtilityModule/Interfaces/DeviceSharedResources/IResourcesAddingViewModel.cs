using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources
{
    public interface IResourcesAddingViewModel
    {
        bool IsResourceAdded { get; set; }
        ICommand SubmitCommand { get; }
        ICommand CloseCommand { get; }
        INameable ResourceWithName { get; set; }
    }
}