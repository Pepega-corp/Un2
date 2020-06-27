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
        private IComPortInterrogationViewModel _comPortInterrogationViewModelImplementation;
        private bool _isInterrogationStopped;

        public ComPortInterrogationViewModel(Func<IDeviceDefinitionViewModel> deviceDefinitionCreator, IDevicesContainerService devicesContainerService,
            IModbusRtuConnection modbusRtuConnection, IComConnectionManager comConnectionManager, IConnectionService connectionService)
        {
            _deviceDefinitionCreator = deviceDefinitionCreator;
            _devicesContainerService = devicesContainerService;
            _modbusRtuConnection = modbusRtuConnection;
            _comConnectionManager = comConnectionManager;
            this._connectionService = connectionService;
            InterrogateCommand = new RelayCommand(OnInterrogateExecute);
            DeviceDefinitionViewModels = new ObservableCollection<IDeviceDefinitionViewModel>();
            SlaveId = 1;
            IsInterrogationNotInProcess = true;
            AddDeviceCommand = new RelayCommand<object>(OnAddDeviceExecute, (b) => IsInterrogationNotInProcess);
            StopInterrogationCommand = new RelayCommand(OnStopInterrogationExecute, (() => !_isInterrogationStopped));
        }

        private async void OnAddDeviceExecute(object obj)
        {
            IDeviceCreator deviceCreator = (obj as IDeviceDefinitionViewModel)?.Model as IDeviceCreator;
            if (deviceCreator == null) return;
            if (deviceCreator.AvailableConnection == null) return;
            _comConnectionManager.SetComPortConfigurationByName((deviceCreator.AvailableConnection as IModbusRtuConnection).ComPortConfiguration, (deviceCreator.AvailableConnection as IModbusRtuConnection).PortName);

            await _devicesContainerService.ConnectDeviceAsync(deviceCreator.Create(), deviceCreator.AvailableConnection);
            DeviceDefinitionViewModels.Remove(obj as IDeviceDefinitionViewModel);
            IsDevicesNotFound = false;

        }

        private void OnStopInterrogationExecute()
        {
            _isInterrogationStopped = true;
            (StopInterrogationCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private async void OnInterrogateExecute()
        {
            IsDevicesNotFound = false;
            RaisePropertyChanged(nameof(IsDevicesNotFound));
            IsInterrogationNotInProcess = false;
            (AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsInterrogationNotInProcess));
            DeviceDefinitionViewModels.Clear();
            if (Is115200Checked)
            {
                await CheckDevices(115200);
            }
            if (Is1200Checked)
            {
                await CheckDevices(1200);
            }
            if (Is19200Checked)
            {
                await CheckDevices(19200);
            }
            if (Is230400Checked)
            {
                await CheckDevices(230400);
            }
            if (Is2400Checked)
            {
                await CheckDevices(2400);
            }
            if (Is38400Checked)
            {
                await CheckDevices(38400);
            }
            if (Is460800Checked)
            {
                await CheckDevices(460800);
            }
            if (Is4800Checked)
            {
                await CheckDevices(4800);
            }
            if (Is57600Checked)
            {
                await CheckDevices(57600);
            }
            if (Is921600Checked)
            {
                await CheckDevices(921600);
            }
            if (Is9600Checked)
            {
                await CheckDevices(9600);
            }
            IsInterrogationNotInProcess = true;
            _isInterrogationStopped = false;
            (StopInterrogationCommand as RelayCommand)?.RaiseCanExecuteChanged();

            (AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();



            IsDevicesNotFound = DeviceDefinitionViewModels.Count == 0;
            RaisePropertyChanged(nameof(IsDevicesNotFound));

            RaisePropertyChanged(nameof(IsInterrogationNotInProcess));
        }

        private async Task CheckDevices(int baudRate)
        {
            if (_isInterrogationStopped)
            {
                return;
            }

            foreach (IDeviceCreator creator in _devicesContainerService.Creators)
            {
                if (creator.ConnectionState.DefaultComPortConfiguration == null) return;
                _modbusRtuConnection.ComPortConfiguration = creator.ConnectionState.DefaultComPortConfiguration;
                _modbusRtuConnection.SlaveId = SlaveId;
                _modbusRtuConnection.ComPortConfiguration.BaudRate = baudRate;
                System.Collections.Generic.List<string> ports = _modbusRtuConnection.GetAvailablePorts();
                var device=creator.Create();
                foreach (string port in ports)
                {
                    _modbusRtuConnection.PortName = port;
                    _comConnectionManager.SetComPortConfigurationByName(_modbusRtuConnection.ComPortConfiguration, port);
                    if (await _modbusRtuConnection.TryOpenConnectionAsync(false, null))
                    {
                        try
                        {
                         
                            var res=await this._connectionService.CheckConnection(creator.ConnectionState,new DeviceContext(null,null,"test",new ConnectionContainer(this._modbusRtuConnection),device.DeviceSharedResources ));
                            bool isMatches = res.IsSuccess;
                            if (isMatches)
                            {
                                IDeviceDefinitionViewModel deviceDefinitionViewModel = _deviceDefinitionCreator();
                                creator.AvailableConnection = _modbusRtuConnection.Clone() as IDeviceConnection;
                                deviceDefinitionViewModel.Model = creator;
                                deviceDefinitionViewModel.ConnectionDescription = _modbusRtuConnection.PortName;
                                DeviceDefinitionViewModels.Add(deviceDefinitionViewModel);
                            }
                        }
                        finally
                        {
                            await Task.Run(() =>
                            {
                                _modbusRtuConnection.CloseConnection();
                            });

                        }
                    }
                }

            }

        }

        public ObservableCollection<IDeviceDefinitionViewModel> DeviceDefinitionViewModels { get; }

        public ICommand InterrogateCommand { get; }

        public ICommand StopInterrogationCommand { get; }

        public ICommand AddDeviceCommand { get; }

        public bool Is1200Checked
        {
            get { return _is1200Checked; }
            set
            {
                _is1200Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is2400Checked
        {
            get { return _is2400Checked; }
            set
            {
                _is2400Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is4800Checked
        {
            get { return _is4800Checked; }
            set
            {
                _is4800Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is9600Checked
        {
            get { return _is9600Checked; }
            set
            {
                _is9600Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is19200Checked
        {
            get { return _is19200Checked; }
            set
            {
                _is19200Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is38400Checked
        {
            get { return _is38400Checked; }
            set
            {
                _is38400Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is57600Checked
        {
            get { return _is57600Checked; }
            set
            {
                _is57600Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is115200Checked
        {
            get { return _is115200Checked; }
            set
            {
                _is115200Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is230400Checked
        {
            get { return _is230400Checked; }
            set
            {
                _is230400Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is460800Checked
        {
            get { return _is460800Checked; }
            set
            {
                _is460800Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Is921600Checked
        {
            get { return _is921600Checked; }
            set
            {
                _is921600Checked = value;
                RaisePropertyChanged();
            }
        }

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
