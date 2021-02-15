using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
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

        public IDeviceViewModel CreateDeviceViewModel(IDevice device)
        {
            IDeviceViewModel deviceViewModel = _deviceViewModelGettingFunc();
            if (device.DeviceMemory == null)
            {
                device.DeviceMemory = _container.Resolve<IDeviceMemory>();
            }

            var deviceLevelPublisher = new FragmentLevelEventsDispatcher();
            DeviceContext context = new DeviceContext(device.DeviceMemory,
                deviceLevelPublisher, device.DeviceSignature,
                device, device.DeviceSharedResources);
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
                        deviceContextConsumer.DeviceContext = context;
                    }

                    fragmentViewModel.Initialize(deviceFragment);


                    deviceViewModel.FragmentViewModels.Add(fragmentViewModel);
                }
            }

            deviceViewModel.TransactionCompleteSubscription = new TransactionCompleteSubscription(context,
                device.ConnectionState, deviceViewModel.ConnectionStateViewModel, _container, () =>
                {
                    if (_container.Resolve<IApplicationGlobalCommands>().AskUserGlobal(
                        _container.Resolve<ILocalizerService>()
                            .GetLocalizedString(ApplicationGlobalNames.StatusMessages.CONNECTION_LOST_GO_OFFLINE),
                        deviceViewModel.DeviceSignature))
                    {
                        _container.Resolve<IDevicesContainerService>().ConnectDeviceAsync(device,
                            _container.Resolve<IDeviceConnectionFactory>(ApplicationGlobalNames
                                .OFFLINE_CONNECTION_FACTORY_NAME).CreateDeviceConnection());
                        (deviceViewModel.TransactionCompleteSubscription as TransactionCompleteSubscription)?.ResetOnConnectionRetryCounter(false);
                    }
                });
            deviceViewModel.TransactionCompleteSubscription.Execute();

            deviceViewModel.Model = device;

            return deviceViewModel;
        }


    }
}