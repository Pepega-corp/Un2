using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Shell;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        private ITypesContainer _typesContainer;

        public ConfigurationTests()
        {
            App app=new App();
          app.Initialize();
          _typesContainer = new TypesContainer(app.Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        private static void SetupContainer(ITypesContainer typesContainer)
        {
        }

        [Test]
        public void Test()
        {
            RuntimeConfigurationViewModel runtimeConfigurationViewModel =
                new RuntimeConfigurationViewModel(_typesContainer);
            var serializerService = _typesContainer.Resolve<ISerializerService>();
            var device = serializerService.DeserializeFromFile<IDevice>("testFile.json");

            var deviceViewModelFactory = _typesContainer.Resolve<IDeviceViewModelFactory>();
            var deviceMemory = new DeviceMemory();
            device.DataProvider = new MockConnection(_typesContainer);
            device.DeviceMemory = deviceMemory;
            RuntimeConfigurationViewModel configurationFragmentViewModel = null;
            var deviceViewModel =
                deviceViewModelFactory.CreateDeviceViewModel(device, () => configurationFragmentViewModel);
            configurationFragmentViewModel =
                deviceViewModel.FragmentViewModels.First(model => model.NameForUiKey == "Configuration") as
                    RuntimeConfigurationViewModel;
            configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels[0]
                .FragmentOptionCommandViewModels[0].OptionCommand.Execute(null);



        }


    }
}