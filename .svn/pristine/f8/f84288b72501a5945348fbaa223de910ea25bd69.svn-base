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
            this._localizerService = localizerService;
            this._dialogCoordinator = dialogCoordinator;
            this._resultingDeviceViewModel = resultingDeviceViewModel;
            this._devicesContainerService = devicesContainerService;
            this.LoadExistingDevice = new RelayCommand(this.OnLoadExistingDevice);
            this.CreateDeviceCommand = new RelayCommand(this.OnCreateDeviceExecute);
            this.SaveInFileCommand = new RelayCommand(this.OnSaveInFileExecute);
            this.OpenSharedResourcesCommand = new RelayCommand(this.OnOpenSharedResourcesExecute);
            this.DeleteFragmentCommand = new RelayCommand<object>(this.OnDeleteFragmentExecute);

            this._currentFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
        }

        private void OnDeleteFragmentExecute(object obj)
        {
            if (!(obj is IFragmentEditorViewModel)) return;
            if (this._dialogCoordinator.ShowModalMessageExternal(this,
                    this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.DELETE),
                    this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings
                        .DELETE_SELECTED_ITEM_QUESTION) + " (" + this._localizerService.GetLocalizedString((obj as IFragmentEditorViewModel).NameForUiKey) + ") ", MessageDialogStyle.AffirmativeAndNegative) ==
                MessageDialogResult.Affirmative)
            {
                this.ResultingDeviceViewModel.FragmentEditorViewModels.Remove(obj as IFragmentEditorViewModel);
            }
        }


        private void OnOpenSharedResourcesExecute()
        {
            this._resultingDeviceViewModel.OpenSharedResources();
        }

        private void OnSaveInFileExecute()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " XML файл (*.xml)|*.xml" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".xml";
            sfd.FileName = this.ResultingDeviceViewModel.DeviceName;
            sfd.InitialDirectory = this._currentFolder;
            if (sfd.ShowDialog() == true)
            {
                this.ResultingDeviceViewModel.SaveDevice(sfd.FileName, false);
                this._currentFolder = Path.GetDirectoryName(sfd.FileName);
            }
        }

        private void OnCreateDeviceExecute()
        {
            if (File.Exists(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH + "//" +
                            this.ResultingDeviceViewModel.DeviceName + ".xml"))
            {
                if (this._dialogCoordinator.ShowModalMessageExternal(this,
                        this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.SAVING),
                        this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings
                            .REPLACE_EXISTING_ITEM_QUESTION), MessageDialogStyle.AffirmativeAndNegative) ==
                    MessageDialogResult.Affirmative)
                {
                    this._resultingDeviceViewModel.SaveDevice(this.ResultingDeviceViewModel.DeviceName);
                    this._devicesContainerService.UpdateDeviceDefinition(this._resultingDeviceViewModel.DeviceName);
                }
            }
            else
            {
                this._resultingDeviceViewModel.SaveDevice(this.ResultingDeviceViewModel.DeviceName);

            }
        }

        private void OnLoadExistingDevice()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " XML файл (*.xml)|*.xml" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            ofd.InitialDirectory = this._currentFolder;
            if (ofd.ShowDialog() == true)
            {
                this.ResultingDeviceViewModel.LoadDevice(ofd.FileName);
                this._currentFolder =  Path.GetDirectoryName(ofd.FileName);
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
            get { return this._isOpen; }
            set
            {
                this._isOpen = value;
                this.RaisePropertyChanged();
            }
        }

        public IResultingDeviceViewModel ResultingDeviceViewModel
        {
            get { return this._resultingDeviceViewModel; }
            set
            {
                this._resultingDeviceViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        protected override void OnNavigatedTo(UniconNavigationContext navigationContext)
        {
            this.IsOpen = true;
        }
    }
}