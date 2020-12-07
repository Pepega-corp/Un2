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
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Journals.Model;
using Unicon2.Fragments.Journals.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Values;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Helpers.Query;
using Unicon2.Tests.Utils;

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
    }
}

