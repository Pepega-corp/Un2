using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.DeviceEditorUtilityModule.ViewModels.Validation;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Navigation;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class DeviceEditorViewModel : NavigationViewModelBase, IDeviceEditorViewModel
    {
        private const string DEFAULT_FOLDER = "Devices";
        private readonly ILocalizerService _localizerService;
        private readonly IDialogCoordinator _dialogCoordinator;
        private IResultingDeviceViewModel _resultingDeviceViewModel;
        private readonly IDevicesContainerService _devicesContainerService;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private bool _isOpen;

        private string _currentFolder;


        public DeviceEditorViewModel(ILocalizerService localizerService, IDialogCoordinator dialogCoordinator,
            IResultingDeviceViewModel resultingDeviceViewModel, IDevicesContainerService devicesContainerService,
            IApplicationGlobalCommands applicationGlobalCommands,
            IDeviceEditorViewModelValidator deviceEditorViewModelValidator)
        {
            _localizerService = localizerService;
            _dialogCoordinator = dialogCoordinator;
            _resultingDeviceViewModel = resultingDeviceViewModel;
            _devicesContainerService = devicesContainerService;
            _applicationGlobalCommands = applicationGlobalCommands;
            LoadExistingDevice = new RelayCommand(OnLoadExistingDevice);
            CreateDeviceCommand = new RelayCommand(OnCreateDeviceExecute);
            SaveInFileCommand = new RelayCommand(OnSaveInFileExecute);
            OpenSharedResourcesCommand = new RelayCommand(OnOpenSharedResourcesExecute);
            DeleteFragmentCommand = new RelayCommand<object>(OnDeleteFragmentExecute);
            OpenAddFragmentWindowCommand = new RelayCommand(OnOpenAddFragmentWindowCommand);
            _currentFolder =
                Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
            DeviceEditorValidationViewModel = new DeviceEditorValidationViewModel(() =>
                deviceEditorViewModelValidator.ValidateDeviceEditor(ResultingDeviceViewModel.FragmentEditorViewModels
                    .ToList()));
            _applicationGlobalCommands.SetGlobalDialogContext(this);
        }

        private void OnOpenAddFragmentWindowCommand()
        {
	        _applicationGlobalCommands.ShowWindowModal(()=>new AddFragmentView(), new AddFragmentViewModel(ResultingDeviceViewModel));
        }

        private void OnDeleteFragmentExecute(object obj)
        {
            if (!(obj is IFragmentEditorViewModel)) return;
            if (_dialogCoordinator.ShowModalMessageExternal(this,
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.DELETE),
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings
                        .DELETE_SELECTED_ITEM_QUESTION) + " (" +
                    _localizerService.GetLocalizedString((obj as IFragmentEditorViewModel).NameForUiKey) + ") ",
                    MessageDialogStyle.AffirmativeAndNegative) ==
                MessageDialogResult.Affirmative)
            {
                ResultingDeviceViewModel.FragmentEditorViewModels.Remove(obj as IFragmentEditorViewModel);
            }
        }


        private void OnOpenSharedResourcesExecute()
        {
            _resultingDeviceViewModel.OpenSharedResources();
        }

        private void OnSaveInFileExecute()
        {
            if (!ValidateDevice())
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " JSON файл (*.json)|*.json" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".json";
            sfd.FileName = ResultingDeviceViewModel.DeviceName;
            sfd.InitialDirectory = _currentFolder;
            if (sfd.ShowDialog() == true)
            {
                ResultingDeviceViewModel.SaveDevice(sfd.FileName, false);
                _currentFolder = Path.GetDirectoryName(sfd.FileName);
            }
        }

        private bool ValidateDevice()
        {
            DeviceEditorValidationViewModel.RefreshErrors.Execute(null);
            if (!DeviceEditorValidationViewModel.IsSuccess)
            {
                var res = _applicationGlobalCommands.AskUserGlobal(
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages
                        .VALIDATION_ERRORS_CONTINUE),
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.VALIDATION_ERRORS), this,
                    _localizerService.GetLocalizedString("Yes"), _localizerService.GetLocalizedString("No"));
                return res;
            }

            return true;
        }

        private void OnCreateDeviceExecute()
        {
            if (File.Exists(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH + "//" +
                            ResultingDeviceViewModel.DeviceName + ".json"))
            {
                if (!ValidateDevice())
                {
                    return;
                }
                if (_dialogCoordinator.ShowModalMessageExternal(this,
                        _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.SAVING),
                        _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings
                            .REPLACE_EXISTING_ITEM_QUESTION), MessageDialogStyle.AffirmativeAndNegative) ==
                    MessageDialogResult.Affirmative)
                {
                    _resultingDeviceViewModel.SaveDevice(ResultingDeviceViewModel.DeviceName);
                    _devicesContainerService.UpdateDeviceDefinition(_resultingDeviceViewModel.DeviceName);
                }
            }
            else
            {
                _resultingDeviceViewModel.SaveDevice(ResultingDeviceViewModel.DeviceName);

            }
        }

        private void OnLoadExistingDevice()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " JSON файл (*.json)|*.json" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            ofd.InitialDirectory = _currentFolder;
            if (ofd.ShowDialog() == true)
            {
                ResultingDeviceViewModel.LoadDevice(ofd.FileName);
                _currentFolder = Path.GetDirectoryName(ofd.FileName);
            }
            DeviceEditorValidationViewModel
                .RefreshErrors.Execute(null);
        }

        public ICommand LoadExistingDevice { get; }
        public ICommand CreateDeviceCommand { get; }
        public ICommand SaveInFileCommand { get; }
        public ICommand OpenAddFragmentWindowCommand { get; }

        public ICommand OpenSharedResourcesCommand { get; }

        public ICommand DeleteFragmentCommand { get; }

        public ICommand SelectConnectionTestPropertyCommand { get; }


        public IDeviceEditorValidationViewModel DeviceEditorValidationViewModel { get; }


        public IResultingDeviceViewModel ResultingDeviceViewModel
        {
            get { return _resultingDeviceViewModel; }
            set
            {
                _resultingDeviceViewModel = value;
                RaisePropertyChanged();
            }
        }

    }
}