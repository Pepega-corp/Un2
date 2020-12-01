using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Model.DefaultDevice;
using Unicon2.Shell;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Tests.Connection
{
    [TestFixture]
    public class ProjectConnectionTests
    {
        public ProjectConnectionTests()
        {
            var typesContainer = Program.GetApp().Container.Resolve<ITypesContainer>();
            typesContainer.Register<IDialogCoordinator, DialogCoordinatorMock>(true);
        }


        [Test]
        public async Task OpenProjectTryReconnect()
        {
            var app = Program.GetApp();
            Program.GetDevice();
            var shell = app.Container.Resolve<ShellViewModel>();

            var projectService = app.Container.Resolve<IUniconProjectService>();
            await projectService.LoadProject("FileAssets/testProject.uniproj");
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];
            var deviceContainerService = app.Container.Resolve<IDevicesContainerService>();
            var device = deviceContainerService.ConnectableItems[0] as DefaultDevice;

            Assert.True(await TestsUtils.WaitUntil(() => deviceViewModel.ConnectionStateViewModel.IsDeviceConnected));
            Assert.True(device.DeviceConnection is MockConnection);
            
            projectService.CreateNewProject();

            Program.RefreshProject();
        }

        [Test]
        public async Task OpenProjectTryReconnectConnectionLost()
        {
            var app = Program.GetApp();

            var shell = app.Container.Resolve<ShellViewModel>();
            var typesContainer = app.Container.Resolve<ITypesContainer>();

            var applicationGlobalCommandsMock = ApplicationGlobalCommandsMock.Create().WithAskUserGlobalResult(true);
            typesContainer.RegisterInstance<IApplicationGlobalCommands>(applicationGlobalCommandsMock);

            MockConnection.IsConnectionLost = true;

            var projectService = app.Container.Resolve<IUniconProjectService>();
            await projectService.LoadProject("FileAssets/testProject.uniproj");
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

            Assert.True(await TestsUtils.WaitUntil(() => !deviceViewModel.ConnectionStateViewModel.IsDeviceConnected));
            Assert.False(applicationGlobalCommandsMock.IsAskUserGlobalTriggered);

            var deviceContainerService = app.Container.Resolve<IDevicesContainerService>();
            var device = deviceContainerService.ConnectableItems[0] as DefaultDevice;
            Assert.True(device.DeviceConnection is OfflineConnection);
            MockConnection.IsConnectionLost = false;

            projectService.CreateNewProject();
            Program.RefreshProject();
        }


 
    }

}