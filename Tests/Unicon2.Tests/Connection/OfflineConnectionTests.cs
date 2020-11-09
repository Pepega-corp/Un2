using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Connection
{
    [TestFixture]
    public class OfflineConnectionTests
    {
        private ITypesContainer _typesContainer;
        private IDevice _device;
        private IDeviceConfiguration _configuration;
        private IDeviceViewModelFactory _deviceViewModelFactory;
        private RuntimeConfigurationViewModel _configurationFragmentViewModel;

        public OfflineConnectionTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
            var serializerService = _typesContainer.Resolve<ISerializerService>();

            _device = serializerService.DeserializeFromFile<IDevice>("testFile.json");
            _configuration =

                _device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                    IDeviceConfiguration;


            _deviceViewModelFactory = _typesContainer.Resolve<IDeviceViewModelFactory>();
            var deviceMemory = new DeviceMemory();

            _device.DeviceMemory = deviceMemory;
            _configurationFragmentViewModel = null;
            var deviceViewModel =
                _deviceViewModelFactory.CreateDeviceViewModel(_device, () => _configurationFragmentViewModel);
            _configurationFragmentViewModel =
                deviceViewModel.FragmentViewModels.First(model => model.NameForUiKey == "Configuration") as
                    RuntimeConfigurationViewModel;
        }

        [Test]
        public async Task OfflineConnectionButtonsAvailbility()
        {
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            Assert.False(_configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY).OptionCommand.CanExecute(null));
            Assert.False(_configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ConfigurationKeys.WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY).OptionCommand.CanExecute(null));
            Assert.False(_configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ConfigurationKeys.TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY).OptionCommand.CanExecute(null));
            
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));
            
            Assert.True(_configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY).OptionCommand.CanExecute(null));
            Assert.True(_configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ConfigurationKeys.WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY).OptionCommand.CanExecute(null));
            Assert.True(_configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ConfigurationKeys.TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY).OptionCommand.CanExecute(null));
            
        }
        
                [Test]
        public async Task OfflineConnectionLocalValuesInit()
        {
            var boolTestDefaultProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestDefaultProperty")
                    .Item as IProperty;
            
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            Assert.True(
                _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                    boolTestDefaultProperty.Address] == 0);

            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                boolTestDefaultProperty.Address] = 1;
                
             await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));
            
             Assert.True(
                 _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                     boolTestDefaultProperty.Address] == 1);
             
             _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                 boolTestDefaultProperty.Address] = 0;
            
        }
    }
}