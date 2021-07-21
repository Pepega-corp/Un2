using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Commands;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
    public class WriteConfigurationIfNothingChangesCase
    {
        [Test]
        public async Task WriteConfigurationIfNothingChanges()
        {

            Program.GetApp().Container.Resolve<IDevicesContainerService>().Refresh();

            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

            var connection =
                new MockConnection();
            await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, connection);


            var configuration = device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                IDeviceConfiguration;

            var shell = StaticContainer.Container.Resolve<ShellViewModel>();
            var deviceMemory = new DeviceMemory();


            device.DeviceMemory = deviceMemory;

            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;


            if (LocalValuesWriteValidator.ValidateLocalValuesToWrite(configurationFragmentViewModel
                .RootConfigurationItemViewModels))
            {
                await new MemoryWriterVisitor(configurationFragmentViewModel.DeviceContext, new List<ushort>(),
                    configuration, 0).ExecuteWrite();
            }

            Assert.False(connection.IsWriteSingleCoilAsyncTriggered);


            Program.RefreshProject();

        }

    }
}