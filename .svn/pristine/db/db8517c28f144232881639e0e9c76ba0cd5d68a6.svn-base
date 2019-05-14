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
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels
{
    public class ModBusConnectionViewModel : ValidatableBindableBase, IModBusConnectionViewModel
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
            this._connectionManager = connectionManager;
            this._container = container;
            this._comPortConfigurationViewModelFactory = comPortConfigurationViewModelFactory;
            this.RefreshAvailablePorts = new RelayCommand(this.OnRefreshingAvailablePorts);
            this.AvailablePorts = new ObservableCollection<string>();
            this.OnRefreshingAvailablePorts();
            this.ComPortInterrogationViewModel = comPortInterrogationViewModel;
            this._isInterrogationEnabled = true;
        }


        private void OnRefreshingAvailablePorts()
        {
            string selectedPort = this.SelectedPort;
            this.AvailablePorts.Clear();
            this.AvailablePorts.AddCollection(this._connectionManager.GetSerialPortNames());
            this.SelectedPort = !this.AvailablePorts.Contains(selectedPort) ? null : selectedPort;
        }

        public string StrongName => "ModBus RTU";

        public IDeviceConnection Model
        {
            get
            {
                this._modbusRtuConnection.ComPortConfiguration = this.SelectedComPortConfigurationViewModel.Model as IComPortConfiguration;
                if (this.SelectedPort != null)
                {
                    this._connectionManager.SetComPortConfigurationByName(this._modbusRtuConnection.ComPortConfiguration,
                        this.SelectedPort);
                }
                return this._modbusRtuConnection;
            }
            set => this.SetConnectionModel(value as IModbusRtuConnection);
        }

        private void SetConnectionModel(IModbusRtuConnection connection)
        {
            this._modbusRtuConnection = connection;
            this.SelectedComPortConfigurationViewModel = this._comPortConfigurationViewModelFactory.CreateComPortConfigurationViewModel(this._modbusRtuConnection.ComPortConfiguration);
            this.SetPort(this._modbusRtuConnection.PortName);
            this.RaisePropertyChanged(nameof(this.SlaveId));
            this.RaisePropertyChanged(nameof(this.ConnectionName));
            this.RaisePropertyChanged(nameof(this.SelectedComPortConfigurationViewModel));

        }

        public ObservableCollection<string> AvailablePorts { get; set; }
        public IComPortConfigurationViewModel SelectedComPortConfigurationViewModel { get; private set; }

        public IComPortInterrogationViewModel ComPortInterrogationViewModel { get; }

        public ICommand RefreshAvailablePorts { get; set; }

        public bool IsInterrogationEnabled
        {
            get { return this._isInterrogationEnabled; }
            set
            {
                this._isInterrogationEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        public string SelectedPort
        {
            get => this._selectedPort;
            set => this.SetPort(value);
        }

        private void SetPort(string value)
        {
            if (this._selectedPort == value) return;
            this._selectedPort = value;
            this.RaisePropertyChanged();
            if (this._selectedPort == null) return;
            IComPortConfiguration comPortConfiguration = this._connectionManager.GetComPortConfiguration(this._selectedPort);
            this.SelectedComPortConfigurationViewModel = this._comPortConfigurationViewModelFactory.CreateComPortConfigurationViewModel(comPortConfiguration);
            this._modbusRtuConnection.ComPortConfiguration = comPortConfiguration;
            this._modbusRtuConnection.PortName = this._selectedPort;
            this.RaisePropertyChanged(nameof(this.SelectedComPortConfigurationViewModel));
        }

        public byte SlaveId
        {
            get => this._modbusRtuConnection.SlaveId;
            set
            {
                this._modbusRtuConnection.SlaveId = this.SlaveId;
                this.RaisePropertyChanged();
            }
        }

        #region Implementation of IDeviceConnectionViewModel

        public string ConnectionName => this._modbusRtuConnection.ConnectionName;



        #endregion

        #region Overrides of ValidatableBindableBase

        protected override void OnValidate()
        {
            ValidationResult result = new ModBusConnectionViewModelValidator(this._container.Resolve<ILocalizerService>()).Validate(this);
            this.SetValidationErrors(result);
        }

        #endregion

        #region Implementation of IViewModel

        object IViewModel.Model
        {
            get { return this.Model; }
            set { this.Model = value as IDeviceConnection; }
        }

        #endregion
    }
}