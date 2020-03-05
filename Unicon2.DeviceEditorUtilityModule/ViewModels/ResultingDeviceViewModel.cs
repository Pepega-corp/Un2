using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResultingDeviceViewModel : ViewModelBase, IResultingDeviceViewModel
    {
        private IDevice _device;
        private ITypesContainer _container;
        private IDeviceSharedResources _deviceSharedResources;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private readonly IFragmentEditorViewModelFactory _fragmentEditorViewModelFactory;
        private readonly IConnectionStateViewModelFactory _connectionStateViewModelFactory;
        private string _deviceName;
        private ObservableCollection<IFragmentEditorViewModel> _fragmentEditorViewModels;
        private ObservableCollection<IFragmentEditorViewModel> _availableEditorFragments;

        public ResultingDeviceViewModel(IDevice device, ITypesContainer container, ILocalizerService localizerService,
            IDeviceSharedResources deviceSharedResources, IApplicationGlobalCommands applicationGlobalCommands,
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,
            IFragmentEditorViewModelFactory fragmentEditorViewModelFactory,
            IConnectionStateViewModelFactory connectionStateViewModelFactory)
        {
            this._device = device;
            this._container = container;
            this._deviceSharedResources = deviceSharedResources;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this._sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            this._fragmentEditorViewModelFactory = fragmentEditorViewModelFactory;
            this._connectionStateViewModelFactory = connectionStateViewModelFactory;
            this._availableEditorFragments = new ObservableCollection<IFragmentEditorViewModel>();
            this.AddFragmentCommand = new RelayCommand<IFragmentEditorViewModel>(this.OnExecuteAddFragment);

            IEnumerable<IFragmentEditorViewModel> fragments = this._container.ResolveAll<IFragmentEditorViewModel>();
            foreach (IFragmentEditorViewModel fragment in fragments)
            {
                (fragment as IResourceContaining)?.SetResources(deviceSharedResources);
                this.AvailableEditorFragments.Add(fragment);
            }

            this.DeviceName = localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.NEW_DEVICE_STRING);
            this.FragmentEditorViewModels = new ObservableCollection<IFragmentEditorViewModel>();
            sharedResourcesGlobalViewModel.InitializeFromResources(deviceSharedResources);
            this.NavigateToConnectionTestingCommand = new RelayCommand(this.OnNavigateToConnectionTestingExecute);
        }

        private void OnNavigateToConnectionTestingExecute()
        {
            this._applicationGlobalCommands.ShowWindowModal(
                () => new ConnectionTestingWindow(), this._connectionStateViewModelFactory.CreateConnectionStateViewModel(this._device.ConnectionState));
        }


        private void OnExecuteAddFragment(IFragmentEditorViewModel fragmentEditorViewModel)
        {
            if (fragmentEditorViewModel == null) return;
            if (!(fragmentEditorViewModel is INameable))
            {
                if (this.FragmentEditorViewModels.Any((model => model.NameForUiKey == fragmentEditorViewModel.NameForUiKey)))
                    return;
                this.FragmentEditorViewModels.Add(fragmentEditorViewModel);
            }
            else
            {
               // this.FragmentEditorViewModels.Add(this._container.Resolve<IFragmentEditorViewModel>(fragmentEditorViewModel.StrongName));
            }
        }


        public ICommand NavigateToConnectionTestingCommand { get; }

        public ObservableCollection<IFragmentEditorViewModel> AvailableEditorFragments
        {
            get { return this._availableEditorFragments; }
            set
            {
                this._availableEditorFragments = value;
                this.RaisePropertyChanged();
            }
        }

        public void OpenSharedResources()
        {
            this._sharedResourcesGlobalViewModel.OpenSharedResourcesForEditing();
        }



        public ObservableCollection<IFragmentEditorViewModel> FragmentEditorViewModels
        {
            get { return this._fragmentEditorViewModels; }
            set
            {
                this._fragmentEditorViewModels = value;
                this.RaisePropertyChanged();
            }
        }


        public void LoadDevice(string path)
        {
            this._device.DeserializeFromFile(path);
            this.FragmentEditorViewModels.Clear();
            foreach (IDeviceFragment fragment in this._device.DeviceFragments)
            {
                IFragmentEditorViewModel fragmentEditorViewModel = this._fragmentEditorViewModelFactory.CreateFragmentEditorViewModel(fragment);
                fragmentEditorViewModel.Initialize(fragment);
                if (fragmentEditorViewModel is IResourceContaining)
                {
                    (fragmentEditorViewModel as IResourceContaining).SetResources(this._device.DeviceSharedResources);
                }
                this.FragmentEditorViewModels.Add(fragmentEditorViewModel);
            }
            this._deviceSharedResources = this._device.DeviceSharedResources;
            this._sharedResourcesGlobalViewModel.InitializeFromResources(this._deviceSharedResources);
            this.DeviceName = this._device.Name;
        }

        public void SaveDevice(string path, bool isDefaultSaving = true)
        {
            this._device.Name = this.DeviceName;
            this._device.DeviceFragments = new List<IDeviceFragment>();
            this._device.DeviceSharedResources = this._deviceSharedResources;
            foreach (IFragmentEditorViewModel fragmentEditorViewModel in this.FragmentEditorViewModels)
            {
                (this._device.DeviceFragments as List<IDeviceFragment>).Add(fragmentEditorViewModel.BuildDeviceFragment());
            }
            this._device.SerializeInFile(path, isDefaultSaving);
        }

        public string DeviceName
        {
            get { return this._deviceName; }
            set
            {
                this._deviceName = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand AddFragmentCommand { get; }
    }
}