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
        bool Is1200Checked { get; set; }
        bool Is2400Checked { get; set; }
        bool Is4800Checked { get; set; }
        bool Is9600Checked { get; set; }
        bool Is19200Checked { get; set; }
        bool Is38400Checked { get; set; }
        bool Is57600Checked { get; set; }
        bool Is115200Checked { get; set; }
        bool Is230400Checked { get; set; }
        bool Is460800Checked { get; set; }
        bool Is921600Checked { get; set; }
        byte SlaveId { get; set; }
        bool IsInterrogationNotInProcess { get; }
        bool IsDevicesNotFound { get; }
    }
}