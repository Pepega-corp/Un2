using System.Collections.ObjectModel;
using System.IO.Ports;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Device
{
    public interface IComPortConfigurationViewModel : IViewModel
    {

        int SelectedBaudRate { get; set; }
        int SelectedDataBits { get; set; }

        StopBits SelectedStopBits { get; set; }

        Parity SelectedParity { get; set; }

        int WaitAnswer { get; set; }
        int WaitByte { get; set; }

        int OnTransmission { get; set; }

        int OffTramsmission { get; set; }

        ObservableCollection<int> BaudRates { get; set; }

        ObservableCollection<int> DataBitsCollection { get; set; }

        ObservableCollection<StopBits> StopBitsCollection { get; set; }
        ObservableCollection<Parity> ParityCollection { get; set; }

    }
}