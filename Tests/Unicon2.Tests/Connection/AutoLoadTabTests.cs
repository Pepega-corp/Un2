using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Journals.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Model.DefaultDevice;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Values;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Helpers.Query;
using Unicon2.Tests.Utils;

namespace Unicon2.Tests.Connection
{
    [TestFixture]
    public class AutoLoadTabTests
    {

        private async Task AutoLoadConfigurationTab(bool setting)
        {
            var setup = Program.RefreshProject();
            var settings = StaticContainer.Container.Resolve<IApplicationSettingsService>();
            var boolTestDefaultProperty = setup.configuration.RootConfigurationItemList
                .FindItemByName(item => item.Name == "boolTestDefaultProperty")
                .Item as IProperty;


            settings.IsFragmentAutoLoadEnabled = setting;
            var offlineConnection = new OfflineConnection();
            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(setup.device, offlineConnection);
            Assert.True(await TestsUtils.WaitUntil(
                () => !setup.deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));
            await setup.configurationViewModel.SetFragmentOpened(true);
            await setup.configurationViewModel.SetFragmentOpened(false);

            await setup.configurationViewModel.SetFragmentOpened(true);

            var connection = new MockConnection();
            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(setup.device, connection);
            Assert.True(await TestsUtils.WaitUntil(
                () => setup.deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));
            await Task.Delay(5000);
            Assert.True(await TestsUtils.WaitUntil(
                () => setting ==
                      setup.device.DeviceMemory.DeviceMemoryValues.ContainsKey(boolTestDefaultProperty.Address)));
        }

        private async Task AutoLoadJournalTab(bool setting)
        {
            Program.CleanProject();

            var settings = StaticContainer.Container.Resolve<IApplicationSettingsService>();
            settings.IsFragmentAutoLoadEnabled = setting;

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);


            var shell = StaticContainer.Container.Resolve<ShellViewModel>();

            var queryDefinitions = await QueryUtils.ReadQueryMockDefinitionFromFile("FileAssets/logFileForMR301JS.txt");

            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new MockConnectionWithSetup(queryDefinitions));

            var journalViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "UniconJournal(Журнал системы)") as
                UniconJournalViewModel;

            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];



            Assert.True(await TestsUtils.WaitUntil(
                () => !deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));
            await journalViewModel.SetFragmentOpened(true);

            Assert.True(await TestsUtils.WaitUntil(
                () => journalViewModel.FragmentOptionsViewModel.GetCommand("Device", "Load").OptionCommand
                    .CanExecute(null), 30000));
            await Task.Delay(5000);

            Assert.True(await TestsUtils.WaitUntil(
                () => setting == journalViewModel.UniconJournal.JournalRecords.Any(), 30000));

        }


        private async Task CycleLoadingMeasuringTab(bool setting)
        {
            var setup = Program.RefreshProject();

            var settings = StaticContainer.Container.Resolve<IApplicationSettingsService>();
            settings.IsFragmentAutoLoadEnabled = setting;

            await StaticContainer
                .Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(setup.device, new MockConnection());
            
            Assert.True(await TestsUtils.WaitUntil(
                () => setup.deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));
            await setup.measuringMonitorViewModel.SetFragmentOpened(true);

            Assert.True(await TestsUtils.WaitUntil(
                () =>
                    setting == ((FragmentOptionToggleCommandViewModel) setup.measuringMonitorViewModel
                        .FragmentOptionsViewModel.GetCommand("Loading", "CycleLoading")).IsChecked, 30000));
            await Task.Delay(5000);
            await setup.measuringMonitorViewModel.SetFragmentOpened(false);

            Assert.True(await TestsUtils.WaitUntil(
                () =>
                    !((FragmentOptionToggleCommandViewModel) setup.measuringMonitorViewModel.FragmentOptionsViewModel
                        .GetCommand("Loading", "CycleLoading")).IsChecked, 30000));

        }

        [Test]
        public async Task AutoLoadConfigurationTabIfSettingFalse()
        {
            await AutoLoadConfigurationTab(false);
        }

        [Test]
        public async Task AutoLoadConfigurationTabIfSettingTrue()
        {
            await AutoLoadConfigurationTab(true);
        }
        
        [Test]
        public async Task AutoLoadJournalsTabIfSettingFalse()
        {
            await AutoLoadJournalTab(false);
        }

        [Test]
        public async Task AutoLoadJournalsTabIfSettingTrue()
        {
            await AutoLoadJournalTab(true);
        }
        
        
        [Test]
        public async Task CycleLoadingMeasuringTabIfSettingFalse()
        {
            await CycleLoadingMeasuringTab(false);
        }

        [Test]
        public async Task CycleLoadingMeasuringTabIfSettingTrue()
        {
            await CycleLoadingMeasuringTab(true);
        }
        
    }
}