using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Factories
{
    public class DeviceViewModelFactory : IDeviceViewModelFactory
    {
        private readonly Func<IDeviceViewModel> _deviceViewModelGettingFunc;
        private readonly ITypesContainer _container;

        public DeviceViewModelFactory(Func<IDeviceViewModel> deviceViewModelGettingFunc, ITypesContainer container)
        {
            _deviceViewModelGettingFunc = deviceViewModelGettingFunc;
            _container = container;
        }

        public IDeviceViewModel CreateDeviceViewModel(IDevice device)
        {
            IDeviceViewModel deviceViewModel = _deviceViewModelGettingFunc();
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
                        deviceDataProvider.SetDeviceData(device.Name);
                    }
                    deviceViewModel.FragmentViewModels.Add(fragmentViewModel);
                }
            }

            deviceViewModel.Model = device;
            return deviceViewModel;
        }
    }
}