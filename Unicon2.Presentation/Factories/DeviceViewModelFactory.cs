using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Subscription;
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

        public IDeviceViewModel CreateDeviceViewModel(IDevice device, Func<IFragmentViewModel> getActiveFragment)
        {
            IDeviceViewModel deviceViewModel = _deviceViewModelGettingFunc();
            if (device.DeviceMemory == null)
            {
                device.DeviceMemory = _container.Resolve<IDeviceMemory>();
            }

            var deviceLevelPublisher = new DeviceLevelEventsPublisher(getActiveFragment);
            if (device.DeviceFragments != null)
            {
                foreach (IDeviceFragment deviceFragment in device.DeviceFragments)
                {
                    IFragmentViewModel fragmentViewModel =
                        _container.Resolve<IFragmentViewModel>(deviceFragment.StrongName +
                                                               ApplicationGlobalNames.CommonInjectionStrings
                                                                   .VIEW_MODEL);
                    if (fragmentViewModel is IDeviceContextConsumer deviceContextConsumer)
                    {
                        var fragmentLevelDispatcher = new FragmentLevelEventsDispatcher();
                        deviceLevelPublisher.AddFragmentDispatcher(fragmentViewModel, fragmentLevelDispatcher);
                        deviceContextConsumer.DeviceContext=new DeviceContext(device.DeviceMemory,
                            new FragmentEventsDispatcher(deviceLevelPublisher, fragmentLevelDispatcher), device.Name,
                            device);
                    }
                    fragmentViewModel.Initialize(deviceFragment);


                    deviceViewModel.FragmentViewModels.Add(fragmentViewModel);
                }
            }

            deviceViewModel.Model = device;
            return deviceViewModel;
        }


    }
}