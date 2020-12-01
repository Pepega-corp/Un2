using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.ModuleDeviceEditing.ViewModels;
using Unicon2.Unity.Common;
using Unity;

namespace Unicon2.Tests.Device
{
    public class DeviceEditingTests
    {
        private TypesContainer _typesContainer;

        public DeviceEditingTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }


        [Test]
        public async Task Validate()
        {
            var deviceEditingViewModel = _typesContainer.Resolve<DeviceEditingViewModel>();
            Assert.False(deviceEditingViewModel.HasErrors);
            deviceEditingViewModel.SubmitCommand.Execute(null);
            Assert.True(deviceEditingViewModel.HasErrors);

        }
    }
}