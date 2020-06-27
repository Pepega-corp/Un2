using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Presentation.Infrastructure.ViewModels.Connection;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Device
{
    public class DeviceViewModel : DisposableBindableBase, IDeviceViewModel
    {
        private readonly IDevicesContainerService _devicesContainerService;
        private IDevice _device;
        private ObservableCollection<IFragmentViewModel> _fragmentViewModels;
        private IDeviceLoggerViewModel _deviceLoggerViewModel;

        public DeviceViewModel(IDevicesContainerService devicesContainerService, IConnectionStateViewModel connectionStateViewModel,
            IDeviceLoggerViewModel deviceLoggerViewModel)
        {
            _devicesContainerService = devicesContainerService;
            _fragmentViewModels = new ObservableCollection<IFragmentViewModel>();
            NavigateToDeviceEditingCommand = new RelayCommand(OnNavigateToDeviceEditing);
            DeleteSelectedDeviceCommand = new RelayCommand(OnDeleteSelectedDevice);
            ConnectionStateViewModel = connectionStateViewModel;
            DeviceLoggerViewModel = deviceLoggerViewModel;
        }

        private void OnDeleteSelectedDevice()
        {
            _devicesContainerService.ConnectableItemChanged?.Invoke(new ConnectableItemChangingContext(_device, ItemModifyingTypeEnum.Delete));
        }

        private void OnNavigateToDeviceEditing()
        {
            _devicesContainerService.ConnectableItemChanged?.Invoke(new ConnectableItemChangingContext(_device, ItemModifyingTypeEnum.Edit));
        }


        public string DeviceSignature
        {
            get { return _device.DeviceSignature; }
            set
            {
                _device.DeviceSignature = value;
                RaisePropertyChanged();
            }
        }

        public ICommand NavigateToDeviceEditingCommand { get; set; }

        public ICommand DeleteSelectedDeviceCommand { get; set; }
        public IConnectionStateViewModel ConnectionStateViewModel { get; set; }

        public IDeviceLoggerViewModel DeviceLoggerViewModel
        {
            get { return _deviceLoggerViewModel; }
            set
            {
                _deviceLoggerViewModel = value;
                RaisePropertyChanged();
            }
        }


        private void SetDeviceModel(IDevice device)
        {
            _device = device;
            DeviceLoggerViewModel.Model = _device.DeviceLogger;
        }



        public ObservableCollection<IFragmentViewModel> FragmentViewModels
        {
            get { return _fragmentViewModels; }
            set
            {
                _fragmentViewModels = value;
                RaisePropertyChanged();
            }
        }

        public string DeviceName
        {
            get { return _device.Name; }
            set
            {
                _device.Name = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName => nameof(DeviceViewModel);

        public object Model
        {
            get => _device;
            set => SetDeviceModel(value as IDevice);
        }
    }
}