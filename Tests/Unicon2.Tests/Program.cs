using System.Linq;
using Prism.Ioc;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Measuring.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Presentation.ViewModels.Device;
using Unicon2.Shell;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Tests
{
    public static class Program
    {

        private static App _app;
        private static DefaultDevice _defaultDevice;

        static void Main(string[] args)
        {
        }

        public static App GetApp()
        {
            if (_app == null)
            {
                _app = new App();
                _app.InitializePublic();
                var shell = _app.Container.Resolve<ShellViewModel>();
            }
            return _app;
        }

        public static void CleanProject()
        {
            var globalCommandsMock = ApplicationGlobalCommandsMock
                .Create()
                .WithAskUserGlobalResult(false);
            StaticContainer.Container?.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);
            GetApp().Container.Resolve<IDevicesContainerService>().Refresh();
            GetApp().Container.Resolve<IDevicesContainerService>().ConnectableItemChanged?.Invoke(
                new ConnectableItemChangingContext(null, ItemModifyingTypeEnum.Refresh));
        }

        public static (DefaultDevice device, DeviceViewModel deviceViewModel, ShellViewModel shellViewModel,
            RuntimeConfigurationViewModel configurationViewModel, IDeviceConfiguration configuration,
            MeasuringMonitorViewModel
            measuringMonitorViewModel )
            RefreshProject()
        {
            GetApp().Container.Resolve<IDevicesContainerService>().Refresh();
            GetApp().Container.Resolve<IDevicesContainerService>().ConnectableItemChanged?.Invoke(
                new ConnectableItemChangingContext(null, ItemModifyingTypeEnum.Refresh));
            var device = GetDevice(true);
            device.DeviceMemory.DeviceMemoryValues.Clear();
            device.DeviceMemory.LocalMemoryValues.Clear();
            device.DeviceMemory.DeviceBitMemoryValues.Clear();
            device.DeviceMemory.LocalBitMemoryValues.Clear();
            GetApp().Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);
            var shell = GetApp().Container.Resolve<ShellViewModel>();
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;
            var measuringMonitorViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "MeasuringMonitor") as
                MeasuringMonitorViewModel;

            var configuration = device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                IDeviceConfiguration;

            return (device, deviceViewModel as DeviceViewModel, shell, configurationFragmentViewModel, configuration,
                measuringMonitorViewModel);
        }

        public static DefaultDevice GetDevice(bool createNew = false)
        {
            if (createNew)
            {
                var serializerService = GetApp().Container.Resolve<ISerializerService>();

                _defaultDevice = serializerService.DeserializeFromFile<IDevice>("FileAssets/testFile.json") as DefaultDevice;
                GetApp().Container.Resolve<IDevicesContainerService>()
                    .AddConnectableItem(_defaultDevice);
                return _defaultDevice;
            }
            if (_defaultDevice == null)
            {     

                var serializerService = GetApp().Container.Resolve<ISerializerService>();

                _defaultDevice = serializerService.DeserializeFromFile<IDevice>("FileAssets/testFile.json") as DefaultDevice;
                GetApp().Container.Resolve<IDevicesContainerService>()
                    .AddConnectableItem(_defaultDevice);
            }
            return _defaultDevice;
        }
        
    }
}