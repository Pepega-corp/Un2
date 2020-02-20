using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels.Connection;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Device
{
    public interface IDeviceViewModel : IViewModel, INotifyPropertyChanged, IDisposable
    {
        ObservableCollection<IFragmentViewModel> FragmentViewModels { get; set; }
        string DeviceName { get; set; }

        string DeviceSignature { get; set; }

        ICommand NavigateToDeviceEditingCommand { get; set; }
        ICommand DeleteSelectedDeviceCommand { get; set; }
        IConnectionStateViewModel ConnectionStateViewModel { get; set; }
        IDeviceLoggerViewModel DeviceLoggerViewModel { get; set; }
    }
}