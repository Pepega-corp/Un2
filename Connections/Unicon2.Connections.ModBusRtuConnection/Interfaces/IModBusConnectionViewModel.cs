using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.ComPortInterrogation;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;

namespace Unicon2.Connections.ModBusRtuConnection.Interfaces
{
    public interface IModBusConnectionViewModel : IDeviceConnectionViewModel
    {
        byte SlaveId { get; set; }
        ObservableCollection<string> AvailablePorts { get; set; }
        string SelectedPort { get; set; }
        IComPortConfigurationViewModel SelectedComPortConfigurationViewModel {get;}
        IComPortInterrogationViewModel ComPortInterrogationViewModel { get; }
        ICommand RefreshAvailablePorts { get; set; }
        bool IsInterrogationEnabled { get; set; }
    }
}