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

        public DeviceViewModel(IDevicesContainerService devicesContainerService, IConnectionStateViewModel connectionStateViewModel, IDeviceLoggerViewModel deviceLoggerViewModel)
        {
            this._devicesContainerService = devicesContainerService;
            this._fragmentViewModels = new ObservableCollection<IFragmentViewModel>();
            this.NavigateToDeviceEditingCommand = new RelayCommand(this.OnNavigateToDeviceEditing);
            this.DeleteSelectedDeviceCommand = new RelayCommand(this.OnDeleteSelectedDevice);
            this.ConnectionStateViewModel = connectionStateViewModel;
            this.DeviceLoggerViewModel = deviceLoggerViewModel;
        }

        private void OnDeleteSelectedDevice()
        {
            this._devicesContainerService.ConnectableItemChanged?.Invoke(new ConnectableItemChangingContext(this._device, ItemModifyingTypeEnum.Delete));
        }

        private void OnNavigateToDeviceEditing()
        {
            this._devicesContainerService.ConnectableItemChanged?.Invoke(new ConnectableItemChangingContext(this._device, ItemModifyingTypeEnum.Edit));
        }


        public string DeviceSignature
        {
            get { return this._device.DeviceSignature; }
            set
            {
                this._device.DeviceSignature = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand NavigateToDeviceEditingCommand { get; set; }

        public ICommand DeleteSelectedDeviceCommand { get; set; }
        public IConnectionStateViewModel ConnectionStateViewModel { get; set; }

        public IDeviceLoggerViewModel DeviceLoggerViewModel
        {
            get { return this._deviceLoggerViewModel; }
            set
            {
                this._deviceLoggerViewModel = value;
                this.RaisePropertyChanged();
            }
        }


        private void SetDeviceModel(IDevice device)
        {
            this._device = device;
            this.DeviceLoggerViewModel.Model = this._device.DeviceLogger;
            this.ConnectionStateViewModel.Model = this._device.ConnectionState;
        }



        public ObservableCollection<IFragmentViewModel> FragmentViewModels
        {
            get { return this._fragmentViewModels; }
            set
            {
                this._fragmentViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public string DeviceName
        {
            get { return this._device.Name; }
            set
            {
                this._device.Name = value;
                this.RaisePropertyChanged();
            }
        }

        #region Implementation of IStronglyNamed

        public string StrongName => nameof(DeviceViewModel);

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get => this._device;
            set => this.SetDeviceModel(value as IDevice);
        }

        #endregion
    }
}