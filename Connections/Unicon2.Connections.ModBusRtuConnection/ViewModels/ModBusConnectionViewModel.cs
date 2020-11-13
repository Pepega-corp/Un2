using FluentValidation.Results;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.ComPortInterrogation;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories;
using Unicon2.Connections.ModBusRtuConnection.ViewModels.Validation;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;


namespace Unicon2.Connections.ModBusRtuConnection.ViewModels
{
    public class ModBusConnectionViewModel : ValidatableBindableBase, IModBusConnectionViewModel, IDeviceConnectionViewModel
    {
        private IComConnectionManager _connectionManager;
        private readonly ITypesContainer _container;
        private readonly IComPortConfigurationViewModelFactory _comPortConfigurationViewModelFactory;
        private string _selectedPort;
        private IModbusRtuConnection _modbusRtuConnection;
        private bool _isInterrogationEnabled;

        public ModBusConnectionViewModel(IComConnectionManager connectionManager, ITypesContainer container,
            IComPortConfigurationViewModelFactory comPortConfigurationViewModelFactory, IComPortInterrogationViewModel comPortInterrogationViewModel)
        {
            _connectionManager = connectionManager;
            _container = container;
            _comPortConfigurationViewModelFactory = comPortConfigurationViewModelFactory;
            RefreshAvailablePorts = new RelayCommand(OnRefreshingAvailablePorts);
            AvailablePorts = new ObservableCollection<string>();
            OnRefreshingAvailablePorts();
            ComPortInterrogationViewModel = comPortInterrogationViewModel;
            _isInterrogationEnabled = true;
        }


        private void OnRefreshingAvailablePorts()
        {
            string selectedPort = SelectedPort;
            AvailablePorts.Clear();
            AvailablePorts.AddCollection(_connectionManager.GetSerialPortNames());
            SelectedPort = !AvailablePorts.Contains(selectedPort) ? null : selectedPort;
        }

        public string StrongName => "ModBus RTU";

        public IDeviceConnection Model
        {
            get
            {
                _modbusRtuConnection.ComPortConfiguration = SelectedComPortConfigurationViewModel.Model as IComPortConfiguration;
                if (SelectedPort != null)
                {
                    _connectionManager.SetComPortConfigurationByName(_modbusRtuConnection.ComPortConfiguration,
                        SelectedPort);
                }
                return _modbusRtuConnection;
            }
            set => SetConnectionModel(value as IModbusRtuConnection);
        }

        private void SetConnectionModel(IModbusRtuConnection connection)
        {
            _modbusRtuConnection = connection;
            SelectedComPortConfigurationViewModel = _comPortConfigurationViewModelFactory.CreateComPortConfigurationViewModel(_modbusRtuConnection.ComPortConfiguration);
            SetPort(_modbusRtuConnection.PortName);
            RaisePropertyChanged(nameof(SlaveId));
            RaisePropertyChanged(nameof(ConnectionName));
            RaisePropertyChanged(nameof(SelectedComPortConfigurationViewModel));

        }

        public ObservableCollection<string> AvailablePorts { get; set; }
        public IComPortConfigurationViewModel SelectedComPortConfigurationViewModel { get; private set; }

        public IComPortInterrogationViewModel ComPortInterrogationViewModel { get; }

        public ICommand RefreshAvailablePorts { get; set; }

        public bool IsInterrogationEnabled
        {
            get { return _isInterrogationEnabled; }
            set
            {
                _isInterrogationEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedPort
        {
            get => _selectedPort;
            set
            {
                SetPort(value);
                FireErrorsChanged();
            }
        }

        private void SetPort(string value)
        {
            if (_selectedPort == value) return;
            _selectedPort = value;
            RaisePropertyChanged();
            if (_selectedPort == null) return;
            IComPortConfiguration comPortConfiguration = _connectionManager.GetComPortConfiguration(_selectedPort);
            SelectedComPortConfigurationViewModel = _comPortConfigurationViewModelFactory.CreateComPortConfigurationViewModel(comPortConfiguration);
            _modbusRtuConnection.ComPortConfiguration = comPortConfiguration;
            _modbusRtuConnection.PortName = _selectedPort;
            RaisePropertyChanged(nameof(SelectedComPortConfigurationViewModel));
        }

        public byte SlaveId
        {
            get => _modbusRtuConnection.SlaveId;
            set
            {
                _modbusRtuConnection.SlaveId = SlaveId;
                RaisePropertyChanged();

            }
        }

        public string ConnectionName => _modbusRtuConnection.ConnectionName;


        protected override void OnValidate()
        {
            ValidationResult result = new ModBusConnectionViewModelValidator(_container.Resolve<ILocalizerService>()).Validate(this);
            SetValidationErrors(result);
        }

        object IViewModel.Model
        {
            get { return Model; }
            set { Model = value as IDeviceConnection; }
        }
    }
}