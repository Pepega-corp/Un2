using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Journals.Model;
using Unicon2.Fragments.Journals.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.ViewModels.Device;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Helpers.Query;
using Unicon2.Tests.Utils;
using Unicon2.Tests.Utils.Mocks;

namespace Unicon2.Tests.Device
{
    [TestFixture]
    public class LoadAllTests
    {
        [Test]
        public async Task LoadAllFromDevice()
        {
            var setup = Program.RefreshProject();

            var boolTestDefaultProperty =
                setup.configuration.RootConfigurationItemList
                    .FindItemByName(item => item.Name == "boolTestDefaultProperty")
                    .Item as IProperty;


            var connection = new MockConnection();
            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(setup.device, connection);

            connection.MemorySlotDictionary.Add(boolTestDefaultProperty.Address, 1);

            var serializerService = new SerializerServiceMock();

            var loadAllViewModel =
                new LoadAllFromDeviceWindowViewModelFactory(StaticContainer.Container.Resolve<ILoadAllService>(),
                        serializerService)
                    .CreateLoadAllFromDeviceWindowViewModel(
                        setup.device,
                        setup.deviceViewModel);

            Assert.True(loadAllViewModel.LoadFragmentViewModels.Count == 1);


            loadAllViewModel.PathToFolderToSave = Directory.GetCurrentDirectory();

            loadAllViewModel.SaveToFolderCommand.Execute(null);
            await TestsUtils.WaitUntil(() => loadAllViewModel.SaveToFolderCommand.CanExecute(null));
            Assert.True(serializerService.SerializedObjects.Count == 0);

            loadAllViewModel.LoadFragmentViewModels[0].IsSelectedForLoading = true;

            
            
            
            loadAllViewModel.SaveToFolderCommand.Execute(null);
            await TestsUtils.WaitUntil(() => loadAllViewModel.SaveToFolderCommand.CanExecute(null));
            Assert.True(serializerService.SerializedObjects.Count == 1);

            var memorySaved = serializerService.SerializedObjects[0] as Dictionary<ushort, ushort>;
            Assert.True(memorySaved[boolTestDefaultProperty.Address] == 1);

            Assert.True(serializerService.SerializedObjects[0] is Dictionary<ushort, ushort>);



            connection.MemorySlotDictionary[boolTestDefaultProperty.Address] = 0;

            loadAllViewModel.SaveToFolderCommand.Execute(null);
            await TestsUtils.WaitUntil(() => loadAllViewModel.SaveToFolderCommand.CanExecute(null));
            Assert.True(serializerService.SerializedObjects.Count == 2);

            var memorySaved1 = serializerService.SerializedObjects[1] as Dictionary<ushort, ushort>;
            Assert.True(memorySaved1[boolTestDefaultProperty.Address] == 0);
            
            
            
            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(setup.device, new OfflineConnection());

            loadAllViewModel.SaveToFolderCommand.Execute(null);
            await TestsUtils.WaitUntil(() => loadAllViewModel.SaveToFolderCommand.CanExecute(null));
            
            Assert.True(serializerService.SerializedObjects.Count == 2);

        }

        [Test]
        public async Task LoadAllFromDeviceJournal()
        {
            Program.CleanProject();

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
            IDeviceViewModel deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];


           
            var serializerServiceMock = new SerializerServiceMock();

            var loadAllViewModel =
                new LoadAllFromDeviceWindowViewModelFactory(StaticContainer.Container.Resolve<ILoadAllService>(),
                        serializerServiceMock)
                    .CreateLoadAllFromDeviceWindowViewModel(
                        device,
                        deviceViewModel);

            Assert.True(loadAllViewModel.LoadFragmentViewModels.Count == 3);


            loadAllViewModel.PathToFolderToSave = Directory.GetCurrentDirectory();

            loadAllViewModel.SaveToFolderCommand.Execute(null);
            await TestsUtils.WaitUntil(() => loadAllViewModel.SaveToFolderCommand.CanExecute(null));
            Assert.True(serializerServiceMock.SerializedObjects.Count == 0);

            loadAllViewModel.LoadFragmentViewModels.First(model =>model.FragmentName =="UniconJournal(Журнал системы)").IsSelectedForLoading = true;

            loadAllViewModel.SaveToFolderCommand.Execute(null);
            await TestsUtils.WaitUntil(() => loadAllViewModel.SaveToFolderCommand.CanExecute(null));
            Assert.True(serializerServiceMock.SerializedObjects.Count == 1);

            Assert.True(((UniconJournal)serializerServiceMock.SerializedObjects[0]).JournalRecords.Count==511);
           
        }

    }
}