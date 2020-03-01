using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Factories
{
    public class DeviceViewModelFactory : IDeviceViewModelFactory
    {
        private readonly Func<IDeviceViewModel> _deviceViewModelGettingFunc;
        private readonly ITypesContainer _container;
        private readonly IDeviceEventsDispatcher _deviceEventsDispatcher;

        public DeviceViewModelFactory(Func<IDeviceViewModel> deviceViewModelGettingFunc, ITypesContainer container, IDeviceEventsDispatcher deviceEventsDispatcher)
        {
            _deviceViewModelGettingFunc = deviceViewModelGettingFunc;
            _container = container;
            _deviceEventsDispatcher = deviceEventsDispatcher;
        }

        public IDeviceViewModel CreateDeviceViewModel(IDevice device)
        {
            IDeviceViewModel deviceViewModel = _deviceViewModelGettingFunc();
            if (device.DeviceMemory == null)
            {
	            device.DeviceMemory = _container.Resolve<IDeviceMemory>();
            }
            if (device.DeviceFragments != null)
            {
                foreach (IDeviceFragment deviceFragment in device.DeviceFragments)
                {
                    IFragmentViewModel fragmentViewModel =
                        _container.Resolve<IFragmentViewModel>(deviceFragment.StrongName +
                                                                    ApplicationGlobalNames.CommonInjectionStrings
                                                                        .VIEW_MODEL);
                    fragmentViewModel.Initialize(deviceFragment);
                    if (fragmentViewModel is IDeviceDataProvider deviceDataProvider)
                    {
                        deviceDataProvider.SetDeviceData(device.Name, _deviceEventsDispatcher,device.DeviceMemory);
                    }
                    deviceViewModel.FragmentViewModels.Add(fragmentViewModel);
                }
            }

            deviceViewModel.Model = device;
            return deviceViewModel;
        }
    }
}