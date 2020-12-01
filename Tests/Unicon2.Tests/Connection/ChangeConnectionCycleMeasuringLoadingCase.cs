using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Fragments.Measuring.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Commands;

namespace Unicon2.Tests.Connection
{
    [TestFixture]
    public class ChangeConnectionCycleMeasuringLoadingCase
    {
          [Test]
        public async Task OfflineConnectionMeasuringButtonsAvailbilityWhenCycleExecuting()
        {
            Program.GetApp().Container.Resolve<IDevicesContainerService>().Refresh();

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new MockConnection());

            
            var measuringMonitorViewModel = StaticContainer.Container.Resolve<ShellViewModel>().ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "MeasuringMonitor") as
                MeasuringMonitorViewModel;
            var loadCommand = measuringMonitorViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Loading").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == "Load")
                .OptionCommand as RelayCommand;
            var loadCycleCommand = measuringMonitorViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Loading").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == "CycleLoading") as FragmentOptionToggleCommandViewModel;
          
            Assert.True(await TestsUtils.WaitUntil(
                () => loadCommand.CanExecute(null)));
            Assert.True(loadCycleCommand.IsEnabled);

            loadCycleCommand.IsChecked = true;


            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new OfflineConnection());
            Assert.True(await TestsUtils.WaitUntil(
                () => !loadCycleCommand.IsChecked));

            Assert.False(loadCycleCommand.IsEnabled);
            Assert.False(loadCommand.CanExecute(null));


            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new MockConnection());


            Assert.True(loadCycleCommand.IsEnabled);
            Assert.True(loadCommand.CanExecute(null));
            Program.RefreshProject();
        }
    }
}