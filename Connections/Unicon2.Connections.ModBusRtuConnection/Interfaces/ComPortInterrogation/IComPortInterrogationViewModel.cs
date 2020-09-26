using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Connections.ModBusRtuConnection.Interfaces.ComPortInterrogation
{
    public interface IComPortInterrogationViewModel
    {
        ObservableCollection<IDeviceDefinitionViewModel> DeviceDefinitionViewModels { get; }
        ICommand InterrogateCommand { get; }
        ICommand StopInterrogationCommand { get; }

        ICommand AddDeviceCommand { get; }
     
        byte SlaveId { get; set; }
        bool IsInterrogationNotInProcess { get; }
        bool IsDevicesNotFound { get; }
    }
}