using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
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
        private bool _isOpen;

        private string _currentFolder;


        public DeviceEditorViewModel(ILocalizerService localizerService, IDialogCoordinator dialogCoordinator,
            IResultingDeviceViewModel resultingDeviceViewModel, IDevicesContainerService devicesContainerService)
        {
            _localizerService = localizerService;
            _dialogCoordinator = dialogCoordinator;
            _resultingDeviceViewModel = resultingDeviceViewModel;
            _devicesContainerService = devicesContainerService;
            LoadExistingDevice = new RelayCommand(OnLoadExistingDevice);
            CreateDeviceCommand = new RelayCommand(OnCreateDeviceExecute);
            SaveInFileCommand = new RelayCommand(OnSaveInFileExecute);
            OpenSharedResourcesCommand = new RelayCommand(OnOpenSharedResourcesExecute);
            DeleteFragmentCommand = new RelayCommand<object>(OnDeleteFragmentExecute);

            _currentFolder =
                Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " XML файл (*.xml)|*.xml" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".xml";
            sfd.FileName = ResultingDeviceViewModel.DeviceName;
            sfd.InitialDirectory = _currentFolder;
            if (sfd.ShowDialog() == true)
            {
                ResultingDeviceViewModel.SaveDevice(sfd.FileName, false);
                _currentFolder = Path.GetDirectoryName(sfd.FileName);
            }
        }

        private void OnCreateDeviceExecute()
        {
            if (File.Exists(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH + "//" +
                            ResultingDeviceViewModel.DeviceName + ".json"))
            {
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
            ofd.Filter = " XML файл (*.xml)|*.xml" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            ofd.InitialDirectory = _currentFolder;
            if (ofd.ShowDialog() == true)
            {
                ResultingDeviceViewModel.LoadDevice(ofd.FileName);
                _currentFolder = Path.GetDirectoryName(ofd.FileName);
            }
        }


        public ICommand LoadExistingDevice { get; }
        public ICommand CreateDeviceCommand { get; }
        public ICommand SaveInFileCommand { get; }

        public ICommand OpenSharedResourcesCommand { get; }

        public ICommand DeleteFragmentCommand { get; }

        public ICommand SelectConnectionTestPropertyCommand { get; }


        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                RaisePropertyChanged();
            }
        }

        public IResultingDeviceViewModel ResultingDeviceViewModel
        {
            get { return _resultingDeviceViewModel; }
            set
            {
                _resultingDeviceViewModel = value;
                RaisePropertyChanged();
            }
        }

        protected override void OnNavigatedTo(UniconNavigationContext navigationContext)
        {
            IsOpen = true;
        }
    }
}