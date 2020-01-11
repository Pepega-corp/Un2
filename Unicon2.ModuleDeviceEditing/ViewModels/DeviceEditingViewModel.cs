using FluentValidation.Results;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.ModuleDeviceEditing.Interfaces;
using Unicon2.ModuleDeviceEditing.ViewModels.Validators;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.Navigation;
using Unicon2.Unity.ViewModels;

namespace Unicon2.ModuleDeviceEditing.ViewModels
{
    /// <summary>
    /// вью-модель для редактирования подключения усторойства
    /// </summary>
    public class DeviceEditingViewModel : NavigationViewModelBase, IDeviceEditingViewModel, INotifyDataErrorInfo
    {
        private IViewModel _selectedDeviceConnection;
        private readonly Func<IDeviceDefinitionViewModel> _deviceDefinitionCreator;
        private IDevicesContainerService _devicesContainerService;
        private readonly ITypesContainer _container;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly ILocalizerService _localizerService;
        private ObservableCollection<IDeviceDefinitionViewModel> _deviceDefinitions;
        private IDeviceDefinitionViewModel _selectedDevice;
        private ObservableCollection<IViewModel> _deviceConnections;
        private bool _isFlyOutOpen;
        private ModesEnum _currentMode;
        private string _deviceSignature;
        private IDeviceConnection _previousDeviceConnection;
        private IDevice _editingDevice;
        private bool _canSubmitCommandExecute = true;

        public DeviceEditingViewModel(Func<IDeviceDefinitionViewModel> deviceDefinitionCreator,
            IDevicesContainerService devicesContainerService, ITypesContainer container,
            IDialogCoordinator dialogCoordinator, ILocalizerService localizerService)
        {
            this.DeviceDefinitions = new ObservableCollection<IDeviceDefinitionViewModel>();
            this.DeviceConnections = new ObservableCollection<IViewModel>();
            //подгрузка всех файлов с определениями (вынести в асинхронный метод)

            this._deviceDefinitionCreator = deviceDefinitionCreator;
            this._devicesContainerService = devicesContainerService;
            this._container = container;
            this._dialogCoordinator = dialogCoordinator;
            this._localizerService = localizerService;

            this.SubmitCommand = new RelayCommand(this.OnSubmitCommand, () => _canSubmitCommandExecute);
            this.OpenDeviceFromFileCommand = new RelayCommand(this.OnOpenDeviceFromFileExecute);

            //подгрузка всех зарегистрированных фабрик разных видов подключений
            IEnumerable<IDeviceConnectionFactory> deviceConnectionFactories = this._container.ResolveAll<IDeviceConnectionFactory>();
            foreach (IDeviceConnectionFactory deviceConnectionFactory in deviceConnectionFactories)
            {
                this.DeviceConnections.Add(deviceConnectionFactory.CreateDeviceConnectionViewModel());
            }
        }

        private void OnOpenDeviceFromFileExecute()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " XML файл (*.xml)|*.xml" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                IDeviceCreator deviceCreator = this._container.Resolve<IDeviceCreator>();
                deviceCreator.DeviceDescriptionFilePath = ofd.FileName;
                deviceCreator.DeviceName = ofd.SafeFileName.Replace(".xml", "");
                IDeviceDefinitionViewModel deviceDefinition = this._deviceDefinitionCreator();
                deviceDefinition.Model = deviceCreator;
                this.SelectedDevice = deviceDefinition;
            }
        }

        /// <summary>
        /// Инициализация при переходе на представление
        /// </summary>
        /// <param name="deviceDefinitionCreator"></param>
        internal void Initialize(Func<IDeviceDefinitionViewModel> deviceDefinitionCreator)
        {
            this.DeviceDefinitions.Clear();
            //  _devicesContainerService.LoadDevicesDefinitions();
            if (this._devicesContainerService.Creators == null) return;
            if (this._devicesContainerService.Creators.Count == 0) return;
            foreach (IDeviceCreator creator in this._devicesContainerService.Creators)
            {
                IDeviceDefinitionViewModel deviceDefinition = deviceDefinitionCreator();
                deviceDefinition.Model = creator;
                this.DeviceDefinitions.Add(deviceDefinition);
            }
        }

        /// <summary>
        /// все варианты устройств для создания
        /// </summary>
        public ObservableCollection<IDeviceDefinitionViewModel> DeviceDefinitions
        {
            get => this._deviceDefinitions;
            set
            {
                this._deviceDefinitions = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// выбранный тип подключения
        /// </summary>
        public IViewModel SelectedDeviceConnection
        {
            get => this._selectedDeviceConnection;
            set
            {
                this._selectedDeviceConnection = value;
                this.RaisePropertyChanged();
                this.FireErrorsChanged(nameof(this.SelectedDeviceConnection));
            }
        }
        /// <summary>
        /// Комманда подключения
        /// </summary>
        public RelayCommand SubmitCommand { get; set; }


        private async Task<bool> ConnectDevice(IDevice device, IDeviceConnection deviceConnection)
        {
            try
            {
                await this._devicesContainerService.ConnectDeviceAsync(device, deviceConnection);
            }
            catch (Exception e)
            {
                this._dialogCoordinator.ShowModalMessageExternal(this,
                    this._localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.PORT_ERROR_MESSAGE),
                    e.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Метод срабатывает на кнопку добавления
        /// </summary>
        private async void OnSubmitCommand()
        {
            _canSubmitCommandExecute = false;
            SubmitCommand.RaiseCanExecuteChanged();
            try
            {
                if (this.HasErrors) return;
                if (this.SelectedDeviceConnection == null) return;
                IDevice connectingDevice = null;

                //в режиме редактирования предыдущее подключение нужно удалить
                if (this.CurrentMode == ModesEnum.EditingMode)
                {
                    connectingDevice = this._editingDevice;
                    this._previousDeviceConnection?.Dispose();
                }

                //В режиме добавления выбранное устройство инициализиреутся
                if (this.CurrentMode == ModesEnum.AddingMode)
                {
                    this.FireErrorsChanged(nameof(this.SelectedDevice));
                    if (this.SelectedDevice == null) return;
                    connectingDevice = (this.SelectedDevice.Model as IDeviceCreator).Create();
                }

                if (connectingDevice == null) return;

                //модель выбранного подключения клонируется, что не создавать устройства с ссылкой на одно и то же подключкение
                //попытка подключения, при неудаче вывод сообщения и прекращение создания устройства
                if (!await this.ConnectDevice(connectingDevice,
                    (this.SelectedDeviceConnection.Model as IDeviceConnection)?.Clone() as IDeviceConnection)) return;


                connectingDevice.DeviceSignature = this.DeviceSignature;

                //закрытие представления
                this.IsFlyOutOpen = false;
            }
            finally
            {
                _canSubmitCommandExecute = true;
                SubmitCommand.RaiseCanExecuteChanged();
            }

        }

        /// <summary>
        /// выбранное устройство
        /// </summary>
        public IDeviceDefinitionViewModel SelectedDevice
        {
            get => this._selectedDevice;
            set
            {
                this._selectedDevice = value;
                this.RaisePropertyChanged();
                this.DeviceSignature = this._selectedDevice?.Name;
                this.FireErrorsChanged(nameof(this.SelectedDevice));
            }

        }
        /// <summary>
        /// текущий режим (редактирование или добавление)
        /// </summary>
        public ModesEnum CurrentMode
        {
            get { return this._currentMode; }
            set
            {
                this._currentMode = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// подпись устройства
        /// </summary>
        public string DeviceSignature
        {
            get { return this._deviceSignature; }
            set
            {
                this._deviceSignature = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Комманда открыть устройство из файла
        /// </summary>
        public ICommand OpenDeviceFromFileCommand { get; set; }

        public ICommand ApplyDefaultSettingsCommand { get; }

        /// <summary>
        /// свойство, показывающее открыто ли представление
        /// </summary>
        public bool IsFlyOutOpen
        {
            get => this._isFlyOutOpen;
            set
            {
                this._isFlyOutOpen = value;
                this.RaisePropertyChanged();
            }

        }

        /// <summary>
        /// коллекция доступных для выбора продключений
        /// </summary>
        public ObservableCollection<IViewModel> DeviceConnections
        {
            get { return this._deviceConnections; }
            set
            {
                this._deviceConnections = value;
                this.RaisePropertyChanged();
            }
        }


        /// <summary>
        /// метод, срабатывающий при навигации на представление
        /// </summary>
        /// <param name="navigationContext"></param>
        protected override void OnNavigatedTo(UniconNavigationContext navigationContext)
        {
            //открыть fluout
            this.IsFlyOutOpen = true;
            this.Initialize(this._deviceDefinitionCreator);
            if (navigationContext.NavigationParameters.GetParameterByName<IDevice>(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY) != null)
            {
                //извлечение из контекста устройства для редактирования
                IDevice device = navigationContext.NavigationParameters.GetParameterByName<IDevice>(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY);
                if (device.DeviceConnection != null)
                {
                    this.SelectedDeviceConnection =
                        this.DeviceConnections.First((model => ((IDeviceConnectionViewModel)model).ConnectionName == device.DeviceConnection.ConnectionName));
                    this.SelectedDeviceConnection.Model = device.DeviceConnection;
                    this._previousDeviceConnection = device.DeviceConnection;
                }
                this.SelectedDevice = this._deviceDefinitionCreator();
                this.SelectedDevice.Name = device.DeviceSignature;
                this._editingDevice = device;
                this.DeviceSignature = device.DeviceSignature;
                this.CurrentMode = ModesEnum.EditingMode;
                //закрываем соединение, чтобы освободить сокет, иначе он будет занят навечно(пока не закроется прилага)
                device.DeviceConnection.CloseConnection();
            }
            else
            {
                //если в параметрах навигации отсутствует устройство, то установка режима добавления устройства
                this.CurrentMode = ModesEnum.AddingMode;
            }
        }


        private readonly Dictionary<string, List<ValidationFailure>> _errorDictionary = new Dictionary<string, List<ValidationFailure>>();
        public bool HasErrors => this._errorDictionary.Count != 0;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = OnErrorsChanged;

        public void FireErrorsChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;
            if (this._errorDictionary.ContainsKey(propertyName))
            {
                return this._errorDictionary[propertyName];
            }
            return null;
        }

        public void SetValidationErrors(ValidationResult result)
        {
            this._errorDictionary.Clear();
            foreach (ValidationFailure error in result.Errors)
            {
                if (this._errorDictionary.ContainsKey(error.PropertyName))
                {
                    this._errorDictionary[error.PropertyName].Add(error);
                }
                else
                {
                    this._errorDictionary.Add(error.PropertyName, new List<ValidationFailure> { error });
                }
            }
        }

        /// <summary>
        /// Валидация вью-модели
        /// </summary>
        private void OnValidate()
        {
            //ValidationResult result = new DeviceEditingViewModelValidator(this._container.Resolve<ILocalizerService>()).Validate(this);
            //this.SetValidationErrors(result);
        }

        private static void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            (sender as DeviceEditingViewModel)?.OnValidate();
        }
    }
}