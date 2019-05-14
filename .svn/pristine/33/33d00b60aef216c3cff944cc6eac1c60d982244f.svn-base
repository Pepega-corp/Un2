using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.ComPortInterrogation;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
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
            IModbusRtuConnection modbusRtuConnection, IComConnectionManager comConnectionManager)
        {
            this._deviceDefinitionCreator = deviceDefinitionCreator;
            this._devicesContainerService = devicesContainerService;
            this._modbusRtuConnection = modbusRtuConnection;
            this._comConnectionManager = comConnectionManager;
            this.InterrogateCommand = new RelayCommand(this.OnInterrogateExecute);
            this.DeviceDefinitionViewModels = new ObservableCollection<IDeviceDefinitionViewModel>();
            this.SlaveId = 1;
            this.IsInterrogationNotInProcess = true;
            this.AddDeviceCommand = new RelayCommand<object>(this.OnAddDeviceExecute, (b) => this.IsInterrogationNotInProcess);
            this.StopInterrogationCommand = new RelayCommand(this.OnStopInterrogationExecute, (() => !this._isInterrogationStopped));
        }

        private async void OnAddDeviceExecute(object obj)
        {
            IDeviceCreator deviceCreator = (obj as IDeviceDefinitionViewModel)?.Model as IDeviceCreator;
            if (deviceCreator == null) return;
            if (deviceCreator.AvailableConnection == null) return;
            this._comConnectionManager.SetComPortConfigurationByName((deviceCreator.AvailableConnection as IModbusRtuConnection).ComPortConfiguration, (deviceCreator.AvailableConnection as IModbusRtuConnection).PortName);

            await this._devicesContainerService.ConnectDeviceAsync(deviceCreator.Create(), deviceCreator.AvailableConnection);
            this.DeviceDefinitionViewModels.Remove(obj as IDeviceDefinitionViewModel);
            this.IsDevicesNotFound = false;

        }

        private void OnStopInterrogationExecute()
        {
            this._isInterrogationStopped = true;
            (this.StopInterrogationCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private async void OnInterrogateExecute()
        {
            this.IsDevicesNotFound = false;
            this.RaisePropertyChanged(nameof(this.IsDevicesNotFound));
            this.IsInterrogationNotInProcess = false;
            (this.AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
            this.RaisePropertyChanged(nameof(this.IsInterrogationNotInProcess));
            this.DeviceDefinitionViewModels.Clear();
            if (this.Is115200Checked)
            {
                await this.CheckDevices(115200);
            }
            if (this.Is1200Checked)
            {
                await this.CheckDevices(1200);
            }
            if (this.Is19200Checked)
            {
                await this.CheckDevices(19200);
            }
            if (this.Is230400Checked)
            {
                await this.CheckDevices(230400);
            }
            if (this.Is2400Checked)
            {
                await this.CheckDevices(2400);
            }
            if (this.Is38400Checked)
            {
                await this.CheckDevices(38400);
            }
            if (this.Is460800Checked)
            {
                await this.CheckDevices(460800);
            }
            if (this.Is4800Checked)
            {
                await this.CheckDevices(4800);
            }
            if (this.Is57600Checked)
            {
                await this.CheckDevices(57600);
            }
            if (this.Is921600Checked)
            {
                await this.CheckDevices(921600);
            }
            if (this.Is9600Checked)
            {
                await this.CheckDevices(9600);
            }
            this.IsInterrogationNotInProcess = true;
            this._isInterrogationStopped = false;
            (this.StopInterrogationCommand as RelayCommand)?.RaiseCanExecuteChanged();

            (this.AddDeviceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();



            this.IsDevicesNotFound = this.DeviceDefinitionViewModels.Count == 0;
            this.RaisePropertyChanged(nameof(this.IsDevicesNotFound));

            this.RaisePropertyChanged(nameof(this.IsInterrogationNotInProcess));
        }

        private async Task CheckDevices(int baudRate)
        {
            if (this._isInterrogationStopped)
            {
                return;
            }

            foreach (IDeviceCreator creator in this._devicesContainerService.Creators)
            {
                if (creator.ConnectionState.DefaultComPortConfiguration == null) return;
                this._modbusRtuConnection.ComPortConfiguration = creator.ConnectionState.DefaultComPortConfiguration;
                this._modbusRtuConnection.SlaveId = this.SlaveId;
                this._modbusRtuConnection.ComPortConfiguration.BaudRate = baudRate;
                System.Collections.Generic.List<string> ports = this._modbusRtuConnection.GetAvailablePorts();

                foreach (string port in ports)
                {
                    this._modbusRtuConnection.PortName = port;
                    this._comConnectionManager.SetComPortConfigurationByName(this._modbusRtuConnection.ComPortConfiguration, port);
                    if (await this._modbusRtuConnection.TryOpenConnectionAsync(false, null))
                    {
                        try
                        {
                            creator.ConnectionState.Initialize(this._modbusRtuConnection, null);
                            await creator.ConnectionState.CheckConnection();
                            bool isMatches = creator.ConnectionState.GetIsExpectedValueMatchesDevice();
                            if (isMatches)
                            {
                                IDeviceDefinitionViewModel deviceDefinitionViewModel = this._deviceDefinitionCreator();
                                creator.AvailableConnection = this._modbusRtuConnection.Clone() as IDeviceConnection;
                                deviceDefinitionViewModel.Model = creator;
                                deviceDefinitionViewModel.ConnectionDescription = this._modbusRtuConnection.PortName;
                                this.DeviceDefinitionViewModels.Add(deviceDefinitionViewModel);
                            }
                        }
                        finally
                        {
                            await Task.Run(() =>
                            {
                                this._modbusRtuConnection.CloseConnection();
                            });

                        }
                    }
                }

            }

        }

        #region Implementation of IComPortInterrogationViewModel

        public ObservableCollection<IDeviceDefinitionViewModel> DeviceDefinitionViewModels { get; }

        public ICommand InterrogateCommand { get; }

        public ICommand StopInterrogationCommand { get; }

        public ICommand AddDeviceCommand { get; }

        public bool Is1200Checked
        {
            get { return this._is1200Checked; }
            set
            {
                this._is1200Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is2400Checked
        {
            get { return this._is2400Checked; }
            set
            {
                this._is2400Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is4800Checked
        {
            get { return this._is4800Checked; }
            set
            {
                this._is4800Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is9600Checked
        {
            get { return this._is9600Checked; }
            set
            {
                this._is9600Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is19200Checked
        {
            get { return this._is19200Checked; }
            set
            {
                this._is19200Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is38400Checked
        {
            get { return this._is38400Checked; }
            set
            {
                this._is38400Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is57600Checked
        {
            get { return this._is57600Checked; }
            set
            {
                this._is57600Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is115200Checked
        {
            get { return this._is115200Checked; }
            set
            {
                this._is115200Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is230400Checked
        {
            get { return this._is230400Checked; }
            set
            {
                this._is230400Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is460800Checked
        {
            get { return this._is460800Checked; }
            set
            {
                this._is460800Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Is921600Checked
        {
            get { return this._is921600Checked; }
            set
            {
                this._is921600Checked = value;
                this.RaisePropertyChanged();
            }
        }

        public byte SlaveId
        {
            get { return this._slaveId; }
            set
            {
                this._slaveId = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsInterrogationNotInProcess { get; private set; }

        public bool IsDevicesNotFound { get; private set; }

        #endregion
    }
}
