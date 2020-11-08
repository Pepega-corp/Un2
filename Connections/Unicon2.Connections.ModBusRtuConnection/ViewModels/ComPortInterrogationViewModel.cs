using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.ComPortInterrogation;
using Unicon2.Connections.ModBusRtuConnection.Model;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels
{
    public class ComPortInterrogationViewModel : ViewModelBase, IComPortInterrogationViewModel
    {
        private readonly Func<IDeviceDefinitionViewModel> _deviceDefinitionCreator;
        private readonly IDevicesContainerService _devicesContainerService;
        private readonly IModbusRtuConnection _modbusRtuConnection;
        private readonly IComConnectionManager _comConnectionManager;
        private readonly IConnectionService _connectionService;
        private readonly IFlyoutService _flyoutService;
        private bool _is1200Checked;
        private bool _is2400Checked;
        private bool _is4800Checked;
        private bool _is9600Checked;
        private bool _is19200Checked;
        private bool _is38400Checked;
        private bool _is57600Checked;
        private bool _is115200Checked;
        private bool _is230400Checked;
        private bool _is460800Checked;
        private bool _is921600Checked;
        private byte _slaveId;
        private bool _isInterrogationStopped;

        public ComPortInterrogationViewModel(Func<IDeviceDefinitionViewModel> deviceDefinitionCreator, IDevicesContainerService devicesContainerService,
            IModbusRtuConnection modbusRtuConnection, IComConnectionManager comConnectionManager, 
            IConnectionService connectionService,IFlyoutService flyoutService)
        {
            _deviceDefinitionCreator = deviceDefinitionCreator;
            _devicesContainerService = devicesContainerService;
            _modbusRtuConnection = modbusRtuConnection;
            _comConnectionManager = comConnectionManager;
            this._connectionService = connectionService;
            _flyoutService = flyoutService;
            InterrogateCommand = new RelayCommand(OnInterrogateExecute);
            DeviceDefinitionViewModels = new ObservableCollection<IDeviceDefinitionViewModel>();
            SlaveId = 1;
            IsInterrogationNotInProcess = true;
            AddDeviceCommand = new RelayCommand<object>(OnAddDeviceExecute, (b) =>
            {
                if (b is IDeviceDefinitionViewModel deviceDefinitionViewModel)
                {
                    if (deviceDefinitionViewModel.IsAddedToProject)
                    {
                        return false;
                    }
                }
                return IsInterrogationNotInProcess;
            });
            StopInterrogationCommand = new RelayCommand(OnStopInterrogationExecute, (() => !_isInterrogationStopped));
        }

        private async void OnAddDeviceExecute(object obj)
        {
            IDeviceCreator deviceCreator = (obj as IDeviceDefinitionViewModel)?.Model as IDeviceCreator;
            if (deviceCreator == null) return;
            if (deviceCreator.AvailableConnection == null) return;
            _comConnectionManager.SetComPortConfigurationByName((deviceCreator.AvailableConnection as IModbusRtuConnection).ComPortConfiguration, (deviceCreator.AvailableConnection as IModbusRtuConnection).PortName);
            var device = deviceCreator.Create();
            device.DeviceSignature = device.Name;
            await _devicesContainerService.ConnectDeviceAsync(device, deviceCreator.AvailableConnection);
            if (!_devicesContainerService.ConnectableItems.Contains(device))
            {
	            _devicesContainerService.AddConnectableItem(device);
            }
            //DeviceDefinitionViewModels.Remove(obj as IDeviceDefinitionViewModel);
            IsDevicesNotFound = false;
            (obj as IDeviceDefinitionViewModel).IsAddedToProject = true;
            await Task.Delay(300);
            _flyoutService.CloseFlyout();

        }

        private void OnStopInterrogationExecute()
        {
            _isInterrogationStopped = true;
            IsInterrogationNotInProcess = true;
            (StopInterrogationCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsInterrogationNotInProcess));

        }

        private async void OnInterrogateExecute()
        {
            IsDevicesNotFound = false;
            RaisePropertyChanged(nameof(IsDevicesNotFound));
            IsInterrogationNotInProcess = false;
            (AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsInterrogationNotInProcess));
            DeviceDefinitionViewModels.Clear();
            await CheckDevices();
            IsInterrogationNotInProcess = true;
            _isInterrogationStopped = false;
            (StopInterrogationCommand as RelayCommand)?.RaiseCanExecuteChanged();

            (AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();



            IsDevicesNotFound = DeviceDefinitionViewModels.Count == 0;
            RaisePropertyChanged(nameof(IsDevicesNotFound));

            RaisePropertyChanged(nameof(IsInterrogationNotInProcess));
        }

        private async Task CheckDevices()
        {
            if (_isInterrogationStopped)
            {
                return;
            }

            if (_devicesContainerService?.Creators != null)
                foreach (IDeviceCreator creator in _devicesContainerService.Creators)
                {
                    try
                    {
                        if (creator?.ConnectionState?.DefaultComPortConfiguration == null) continue;
                        _modbusRtuConnection.ComPortConfiguration = creator.ConnectionState.DefaultComPortConfiguration;
                        _modbusRtuConnection.SlaveId = SlaveId;
                        System.Collections.Generic.List<string> ports = _modbusRtuConnection.GetAvailablePorts();
                        var device = creator.Create();
                        foreach (string port in ports)
                        {
                            _modbusRtuConnection.PortName = port;
                            _comConnectionManager.SetComPortConfigurationByName(
                                _modbusRtuConnection.ComPortConfiguration,
                                port);
                            if ((await _modbusRtuConnection.TryOpenConnectionAsync(null)).IsSuccess)
                            {
                                try
                                {
                                    var res = await this._connectionService.CheckConnection(creator.ConnectionState,
                                        new DeviceContext(null, null, "test",
                                            new ConnectionContainer(this._modbusRtuConnection),
                                            device.DeviceSharedResources));
                                    bool isMatches = res.IsSuccess;
                                    if (isMatches)
                                    {
                                        IDeviceDefinitionViewModel deviceDefinitionViewModel =
                                            _deviceDefinitionCreator();
                                        creator.AvailableConnection = _modbusRtuConnection.Clone() as IDeviceConnection;
                                        deviceDefinitionViewModel.Model = creator;
                                        deviceDefinitionViewModel.ConnectionDescription = _modbusRtuConnection.PortName;
                                        DeviceDefinitionViewModels.Add(deviceDefinitionViewModel);
                                    }
                                }
                                finally
                                {
                                    await Task.Run(() => { _modbusRtuConnection.CloseConnection(); });
                                }
                            }
                        }
                    }
                    finally
                    {

                    }
                }
        }

        public ObservableCollection<IDeviceDefinitionViewModel> DeviceDefinitionViewModels { get; }

        public ICommand InterrogateCommand { get; }

        public ICommand StopInterrogationCommand { get; }

        public ICommand AddDeviceCommand { get; }

   
        public byte SlaveId
        {
            get { return _slaveId; }
            set
            {
                _slaveId = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInterrogationNotInProcess { get; private set; }

        public bool IsDevicesNotFound { get; private set; }
    }
}
