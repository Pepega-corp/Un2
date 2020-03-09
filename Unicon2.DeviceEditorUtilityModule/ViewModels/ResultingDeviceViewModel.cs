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
            _device = device;
            _container = container;
            _deviceSharedResources = deviceSharedResources;
            _applicationGlobalCommands = applicationGlobalCommands;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            _fragmentEditorViewModelFactory = fragmentEditorViewModelFactory;
            _connectionStateViewModelFactory = connectionStateViewModelFactory;
            _availableEditorFragments = new ObservableCollection<IFragmentEditorViewModel>();
            AddFragmentCommand = new RelayCommand<IFragmentEditorViewModel>(OnExecuteAddFragment);

            IEnumerable<IFragmentEditorViewModel> fragments = _container.ResolveAll<IFragmentEditorViewModel>();
            foreach (IFragmentEditorViewModel fragment in fragments)
            {
                AvailableEditorFragments.Add(fragment);
            }
            DeviceName = localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.NEW_DEVICE_STRING);
            FragmentEditorViewModels = new ObservableCollection<IFragmentEditorViewModel>();
            sharedResourcesGlobalViewModel.InitializeFromResources(deviceSharedResources);
            NavigateToConnectionTestingCommand = new RelayCommand(OnNavigateToConnectionTestingExecute);
        }

        private void OnNavigateToConnectionTestingExecute()
        {
            _applicationGlobalCommands.ShowWindowModal(
                () => new ConnectionTestingWindow(), _connectionStateViewModelFactory.CreateConnectionStateViewModel(_device.ConnectionState));
        }


        private void OnExecuteAddFragment(IFragmentEditorViewModel fragmentEditorViewModel)
        {
            if (fragmentEditorViewModel == null) return;
            if (!(fragmentEditorViewModel is INameable))
            {
                if (FragmentEditorViewModels.Any((model => model.NameForUiKey == fragmentEditorViewModel.NameForUiKey)))
                    return;
                FragmentEditorViewModels.Add(fragmentEditorViewModel);
            }
            else
            {
               // this.FragmentEditorViewModels.Add(this._container.Resolve<IFragmentEditorViewModel>(fragmentEditorViewModel.StrongName));
            }
        }


        public ICommand NavigateToConnectionTestingCommand { get; }

        public ObservableCollection<IFragmentEditorViewModel> AvailableEditorFragments
        {
            get { return _availableEditorFragments; }
            set
            {
                _availableEditorFragments = value;
                RaisePropertyChanged();
            }
        }

        public void OpenSharedResources()
        {
            _sharedResourcesGlobalViewModel.OpenSharedResourcesForEditing();
        }



        public ObservableCollection<IFragmentEditorViewModel> FragmentEditorViewModels
        {
            get { return _fragmentEditorViewModels; }
            set
            {
                _fragmentEditorViewModels = value;
                RaisePropertyChanged();
            }
        }


        public void LoadDevice(string path)
        {
            _device.DeserializeFromFile(path);
            FragmentEditorViewModels.Clear();
            foreach (IDeviceFragment fragment in _device.DeviceFragments)
            {
                IFragmentEditorViewModel fragmentEditorViewModel = _fragmentEditorViewModelFactory.CreateFragmentEditorViewModel(fragment);
                fragmentEditorViewModel.Initialize(fragment);
                _sharedResourcesGlobalViewModel.InitializeFromResources(_device.DeviceSharedResources);
                FragmentEditorViewModels.Add(fragmentEditorViewModel);
            }
            _deviceSharedResources = _device.DeviceSharedResources;
            _sharedResourcesGlobalViewModel.InitializeFromResources(_deviceSharedResources);
            DeviceName = _device.Name;
        }

        public void SaveDevice(string path, bool isDefaultSaving = true)
        {
            _device.Name = DeviceName;
            _device.DeviceFragments = new List<IDeviceFragment>();
            _device.DeviceSharedResources = _deviceSharedResources;
            foreach (IFragmentEditorViewModel fragmentEditorViewModel in FragmentEditorViewModels)
            {
                (_device.DeviceFragments as List<IDeviceFragment>).Add(fragmentEditorViewModel.BuildDeviceFragment());
            }
            _device.SerializeInFile(path, isDefaultSaving);
        }

        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AddFragmentCommand { get; }
    }
}