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
            DeviceDefinitions = new ObservableCollection<IDeviceDefinitionViewModel>();
            DeviceConnections = new ObservableCollection<IViewModel>();
            //подгрузка всех файлов с определениями (вынести в асинхронный метод)

            _deviceDefinitionCreator = deviceDefinitionCreator;
            _devicesContainerService = devicesContainerService;
            _container = container;
            _dialogCoordinator = dialogCoordinator;
            _localizerService = localizerService;

            SubmitCommand = new RelayCommand(OnSubmitCommand, () => _canSubmitCommandExecute);
            OpenDeviceFromFileCommand = new RelayCommand(OnOpenDeviceFromFileExecute);

            //подгрузка всех зарегистрированных фабрик разных видов подключений
            IEnumerable<IDeviceConnectionFactory> deviceConnectionFactories = _container.ResolveAll<IDeviceConnectionFactory>();
            foreach (IDeviceConnectionFactory deviceConnectionFactory in deviceConnectionFactories)
            {
                DeviceConnections.Add(deviceConnectionFactory.CreateDeviceConnectionViewModel());
            }
        }

        private void OnOpenDeviceFromFileExecute()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " JSON файл (*.json)|*.json" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                IDeviceCreator deviceCreator = _container.Resolve<IDeviceCreator>();
                deviceCreator.DeviceDescriptionFilePath = ofd.FileName;
                deviceCreator.DeviceName = ofd.SafeFileName.Replace(".json", "");
                IDeviceDefinitionViewModel deviceDefinition = _deviceDefinitionCreator();
                deviceDefinition.Model = deviceCreator;
                SelectedDevice = deviceDefinition;
            }
        }

        /// <summary>
        /// Инициализация при переходе на представление
        /// </summary>
        /// <param name="deviceDefinitionCreator"></param>
        internal void Initialize(Func<IDeviceDefinitionViewModel> deviceDefinitionCreator)
        {
            DeviceDefinitions.Clear();
            //  _devicesContainerService.LoadDevicesDefinitions();
            if (_devicesContainerService.Creators == null) return;
            if (_devicesContainerService.Creators.Count == 0) return;
            foreach (IDeviceCreator creator in _devicesContainerService.Creators)
            {
                IDeviceDefinitionViewModel deviceDefinition = deviceDefinitionCreator();
                deviceDefinition.Model = creator;
                DeviceDefinitions.Add(deviceDefinition);
            }
        }

        /// <summary>
        /// все варианты устройств для создания
        /// </summary>
        public ObservableCollection<IDeviceDefinitionViewModel> DeviceDefinitions
        {
            get => _deviceDefinitions;
            set
            {
                _deviceDefinitions = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// выбранный тип подключения
        /// </summary>
        public IViewModel SelectedDeviceConnection
        {
            get => _selectedDeviceConnection;
            set
            {
                _selectedDeviceConnection = value;
                RaisePropertyChanged();
                FireErrorsChanged(nameof(SelectedDeviceConnection));
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
                await _devicesContainerService.ConnectDeviceAsync(device, deviceConnection);
            }
            catch (Exception e)
            {
                _dialogCoordinator.ShowModalMessageExternal(this,
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.PORT_ERROR_MESSAGE),
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
                if (HasErrors) return;
                if (SelectedDeviceConnection == null) return;
                IDevice connectingDevice = null;

                //в режиме редактирования предыдущее подключение нужно удалить
                if (CurrentMode == ModesEnum.EditingMode)
                {
                    connectingDevice = _editingDevice;
                    _previousDeviceConnection?.Dispose();
                }

                //В режиме добавления выбранное устройство инициализиреутся
                if (CurrentMode == ModesEnum.AddingMode)
                {
                    FireErrorsChanged(nameof(SelectedDevice));
                    if (SelectedDevice == null) return;
                    connectingDevice = (SelectedDevice.Model as IDeviceCreator).Create();
                }

                if (connectingDevice == null) return;
                connectingDevice.DeviceSignature = DeviceSignature;
                //модель выбранного подключения клонируется, что не создавать устройства с ссылкой на одно и то же подключкение
                //попытка подключения, при неудаче вывод сообщения и прекращение создания устройства
                if (!await ConnectDevice(connectingDevice,
                    (SelectedDeviceConnection.Model as IDeviceConnection)?.Clone() as IDeviceConnection)) return;


                

                //закрытие представления
                IsFlyOutOpen = false;
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
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged();
                DeviceSignature = _selectedDevice?.Name;
                FireErrorsChanged(nameof(SelectedDevice));
            }

        }
        /// <summary>
        /// текущий режим (редактирование или добавление)
        /// </summary>
        public ModesEnum CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// подпись устройства
        /// </summary>
        public string DeviceSignature
        {
            get { return _deviceSignature; }
            set
            {
                _deviceSignature = value;
                RaisePropertyChanged();
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
            get => _isFlyOutOpen;
            set
            {
                _isFlyOutOpen = value;
                RaisePropertyChanged();
            }

        }

        /// <summary>
        /// коллекция доступных для выбора продключений
        /// </summary>
        public ObservableCollection<IViewModel> DeviceConnections
        {
            get { return _deviceConnections; }
            set
            {
                _deviceConnections = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// метод, срабатывающий при навигации на представление
        /// </summary>
        /// <param name="navigationContext"></param>
        protected override void OnNavigatedTo(UniconNavigationContext navigationContext)
        {
            //открыть fluout
            IsFlyOutOpen = true;
            Initialize(_deviceDefinitionCreator);
            if (navigationContext.NavigationParameters.GetParameterByName<IDevice>(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY) != null)
            {
                //извлечение из контекста устройства для редактирования
                IDevice device = navigationContext.NavigationParameters.GetParameterByName<IDevice>(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY);
                if (device.DeviceConnection != null)
                {
                    SelectedDeviceConnection =
                        DeviceConnections.First((model => ((IDeviceConnectionViewModel)model).ConnectionName == device.DeviceConnection.ConnectionName));
                    SelectedDeviceConnection.Model = device.DeviceConnection;
                    _previousDeviceConnection = device.DeviceConnection;
                }
                SelectedDevice = _deviceDefinitionCreator();
                SelectedDevice.Name = device.DeviceSignature;
                _editingDevice = device;
                DeviceSignature = device.DeviceSignature;
                CurrentMode = ModesEnum.EditingMode;
                //закрываем соединение, чтобы освободить сокет, иначе он будет занят навечно(пока не закроется прилага)
                device.DeviceConnection.CloseConnection();
            }
            else
            {
                //если в параметрах навигации отсутствует устройство, то установка режима добавления устройства
                CurrentMode = ModesEnum.AddingMode;
            }
        }


        private readonly Dictionary<string, List<ValidationFailure>> _errorDictionary = new Dictionary<string, List<ValidationFailure>>();
        public bool HasErrors => _errorDictionary.Count != 0;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = OnErrorsChanged;

        public void FireErrorsChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;
            if (_errorDictionary.ContainsKey(propertyName))
            {
                return _errorDictionary[propertyName];
            }
            return null;
        }

        public void SetValidationErrors(ValidationResult result)
        {
            _errorDictionary.Clear();
            foreach (ValidationFailure error in result.Errors)
            {
                if (_errorDictionary.ContainsKey(error.PropertyName))
                {
                    _errorDictionary[error.PropertyName].Add(error);
                }
                else
                {
                    _errorDictionary.Add(error.PropertyName, new List<ValidationFailure> { error });
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