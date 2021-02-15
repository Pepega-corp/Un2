using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Journals.Model;
using Unicon2.Fragments.Journals.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Values;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Helpers.Query;
using Unicon2.Tests.Utils;
using Unicon2.Tests.Utils.Mocks;

namespace Unicon2.Tests.Journals
{
    [TestFixture]
    public class Mr301SystemJournalTests
    {
        [Test]
        public async Task LoadMr301SystemJournalTest()
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

            var commandViewModel = journalViewModel.FragmentOptionsViewModel.GetCommand("Device", "Load");
            commandViewModel.OptionCommand.Execute(null);
            await TestsUtils.WaitUntil(() => commandViewModel.OptionCommand.CanExecute(null));


            var str = StaticContainer.Container.Resolve<ISerializerService>().SerializeInString(
                journalViewModel.UniconJournal
            );


            var str1 = StaticContainer.Container.Resolve<ISerializerService>()
                .DeserializeFromFile<UniconJournal>("FileAssets/Журнал(Журнал системы) МР301_JA.ujr");
            Program.RefreshProject();
        }
        
        
        [Test]
        public async Task LoadJournalStructureMustBeChecked()
        { 
            Program.CleanProject();
           var globalCommandsMock = ApplicationGlobalCommandsMock
               .Create()
               .WithSelectFileToOpenResult("FileAssets/Журнал(Журнал системы) МР301_JA.ujr")
               .WithAskUserGlobalResult(false);
            StaticContainer.Container.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

            var shell = StaticContainer.Container.Resolve<ShellViewModel>();

            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new OfflineConnection());

            var journalViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "UniconJournal(Аварий)") as
                UniconJournalViewModel;

            var commandViewModel = journalViewModel.FragmentOptionsViewModel.GetCommand("Device", "Open");
            commandViewModel.OptionCommand.Execute(null);
            await TestsUtils.WaitUntil(() => commandViewModel.OptionCommand.CanExecute(null));
            Assert.True(globalCommandsMock.IsAskUserGlobalTriggered);
            
            Assert.False(journalViewModel.Table.Values.Any());
         
            Program.RefreshProject();
        }
        
        [Test]
        public async Task LoadJournalStructureMustBeCheckedReverse()
        { 
            Program.CleanProject();
            var globalCommandsMock = ApplicationGlobalCommandsMock
                .Create()
                .WithSelectFileToOpenResult("FileAssets/Журнал(Аварий) МР301_JA.ujr")
                .WithAskUserGlobalResult(false);
            StaticContainer.Container.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

            var shell = StaticContainer.Container.Resolve<ShellViewModel>();

            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new OfflineConnection());

            var journalViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "UniconJournal(Журнал системы)") as
                UniconJournalViewModel;

            var commandViewModel = journalViewModel.FragmentOptionsViewModel.GetCommand("Device", "Open");
            commandViewModel.OptionCommand.Execute(null);
            await TestsUtils.WaitUntil(() => commandViewModel.OptionCommand.CanExecute(null));
            Assert.True(globalCommandsMock.IsAskUserGlobalTriggered);
            
            Assert.False(journalViewModel.Table.Values.Any());
         
            Program.RefreshProject();
        }
        
        [Test]
        public async Task LoadJournalStructureMustBeCheckedAndItIsOkComplex()
        { 
            Program.CleanProject();
            var globalCommandsMock = ApplicationGlobalCommandsMock
                .Create()
                .WithSelectFileToOpenResult("FileAssets/Журнал(Аварий) МР301_JA.ujr")
                .WithAskUserGlobalResult(false);
            StaticContainer.Container.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

            var shell = StaticContainer.Container.Resolve<ShellViewModel>();

            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new OfflineConnection());

            var journalViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "UniconJournal(Аварий)") as
                UniconJournalViewModel;

            var commandViewModel = journalViewModel.FragmentOptionsViewModel.GetCommand("Device", "Open");
            
            commandViewModel.OptionCommand.Execute(null);
            await TestsUtils.WaitUntil(() => commandViewModel.OptionCommand.CanExecute(null));
            Assert.False(globalCommandsMock.IsAskUserGlobalTriggered);
            Assert.True(journalViewModel.Table.Values.Any());

            Program.RefreshProject();
        }
        
        [Test]
        public async Task LoadJournalStructureMustBeCheckedAndItIsOk()
        { 
            Program.CleanProject();
            var globalCommandsMock = ApplicationGlobalCommandsMock
                .Create()
                .WithSelectFileToOpenResult("FileAssets/Журнал(Журнал системы) МР301_JA.ujr")
                .WithAskUserGlobalResult(false);
            StaticContainer.Container.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

            var shell = StaticContainer.Container.Resolve<ShellViewModel>();

            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new OfflineConnection());

            var journalViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "UniconJournal(Журнал системы)") as
                UniconJournalViewModel;

            var commandViewModel = journalViewModel.FragmentOptionsViewModel.GetCommand("Device", "Open");
            
            commandViewModel.OptionCommand.Execute(null);
            await TestsUtils.WaitUntil(() => commandViewModel.OptionCommand.CanExecute(null));
            Assert.False(globalCommandsMock.IsAskUserGlobalTriggered);
            Assert.True(journalViewModel.Table.Values.Any());

            Program.RefreshProject();
        }
    }
}

