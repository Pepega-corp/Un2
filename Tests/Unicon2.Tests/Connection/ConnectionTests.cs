using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Formatting.Model;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Measuring.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Connection
{
    [TestFixture]
    public class ConnectionTests
    {
        private ITypesContainer _typesContainer;
        private IDevice _device;
        private IDeviceConfiguration _configuration;
        private IDeviceViewModelFactory _deviceViewModelFactory;
        private RuntimeConfigurationViewModel _configurationFragmentViewModel;
        private MeasuringMonitorViewModel _measuringMonitorViewModel;


        private ShellViewModel _shell;
        private IDeviceViewModel _deviceViewModel;
        private RelayCommand _readCommand;

        public ConnectionTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
            _device = Program.GetDevice();
            _configuration = _device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                IDeviceConfiguration;

            _shell = _typesContainer.Resolve<ShellViewModel>();
            _deviceViewModelFactory = _typesContainer.Resolve<IDeviceViewModelFactory>();
            var deviceMemory = new DeviceMemory();
            _typesContainer.Resolve<IDevicesContainerService>()
                .AddConnectableItem(_device);
            _device.DeviceMemory = deviceMemory;
            _deviceViewModel = _shell.ProjectBrowserViewModel.DeviceViewModels[0];
            _configurationFragmentViewModel = null;
            _configurationFragmentViewModel = _shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;
            _measuringMonitorViewModel=_shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "MeasuringMonitor") as
                MeasuringMonitorViewModel;

            _readCommand = _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY)
                .OptionCommand as RelayCommand;
        }

        [Test]
        public async Task OfflineConnectionConfigurationButtonsAvailbility()
        {

            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            int isChanagedTriggered = 0;
            int isChanagedTriggered1 = 0;
            int isChanagedTriggered2 = 0;

            var optionCommands = new List<RelayCommand>()
            {
                _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                    .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                    .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY)
                    .OptionCommand as RelayCommand,
                _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                    .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                    .First(model => model.TitleKey == ConfigurationKeys.WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY)
                    .OptionCommand as RelayCommand,
                _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                    .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                    .First(model => model.TitleKey == ConfigurationKeys.TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY)
                    .OptionCommand as RelayCommand,
            };
            Assert.False(optionCommands.Any(command => command.CanExecute(null)));

            optionCommands[0].CanExecuteChanged += (sender, args) => { isChanagedTriggered++; };
            optionCommands[1].CanExecuteChanged += (sender, args) => { isChanagedTriggered1++; };
            optionCommands[2].CanExecuteChanged += (sender, args) => { isChanagedTriggered2++; };

            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));
            await _configurationFragmentViewModel.SetFragmentOpened(true);
            Assert.True(await TestsUtils.WaitUntil(
                () => optionCommands.All(command => command.CanExecute(null)), 30000));
            Assert.True(isChanagedTriggered > 0);
            Assert.True(isChanagedTriggered1 > 0);
            Assert.True(isChanagedTriggered2 > 0);
            //open again and it must not start reading
            _configurationFragmentViewModel.SetFragmentOpened(true);

            Assert.True(optionCommands.All(command => command.CanExecute(null)));
        }

        [Test]
        public async Task OfflineConnectionMeasuringButtonsAvailbility()
        {

            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            int isChanagedTriggered1 = 0;


            var loadCommand = _measuringMonitorViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Loading").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == "Load")
                .OptionCommand as RelayCommand;
            var loadCycleCommand = _measuringMonitorViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Loading").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == "CycleLoading") as FragmentOptionToggleCommandViewModel;

            Assert.False(loadCycleCommand.IsEnabled);
            Assert.False(loadCommand.CanExecute(null));


            loadCommand.CanExecuteChanged += (sender, args) => { isChanagedTriggered1++; };

            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));
            Assert.True(await TestsUtils.WaitUntil(
                () =>
                {
                    return loadCycleCommand.IsEnabled && loadCommand.CanExecute(null) &&
                            isChanagedTriggered1 > 0;
                }, 30000));
        }

        [Test]
        public async Task OfflineConnectionValuesInit()
        {
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.Clear();
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues.Clear();

            var boolTestDefaultProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestDefaultProperty")
                    .Item as IProperty;

            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());
            await _configurationFragmentViewModel.SetFragmentOpened(true);
            Assert.True(await TestsUtils.WaitUntil(
                () => _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                    boolTestDefaultProperty.Address] == 0,30000));
          

            Assert.False(
                _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.ContainsKey(
                    boolTestDefaultProperty.Address));


            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                boolTestDefaultProperty.Address] = 1;

            await _configurationFragmentViewModel.SetFragmentOpened(true);
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));

            Assert.True(
                _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                    boolTestDefaultProperty.Address] == 1);

            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                boolTestDefaultProperty.Address] = 0;

            Assert.True(await TestsUtils.WaitUntil(
                () => _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.ContainsKey(
                    boolTestDefaultProperty.Address), 30000));
            
        }


        [Test]
        public async Task ChangeConnectionToOffline()
        {

            var boolTestDefaultProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestDefaultProperty")
                    .Item as IProperty;

            var defaultPropertyWithBoolFormatting = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultProperty")
                .Item as IRuntimePropertyViewModel;

            
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.Clear();
            await _configurationFragmentViewModel.SetFragmentOpened(true);
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));

            var commandRead = _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY)
                .OptionCommand as RelayCommand;

            Assert.False(commandRead.CanExecute(null));

            Assert.True(await TestsUtils.WaitUntil(
                () => commandRead.CanExecute(null), 30000));


            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            Assert.False(commandRead.CanExecute(null));


            Assert.True(await TestsUtils.WaitUntil(
                () => defaultPropertyWithBoolFormatting.DeviceValue == null, 30000));


        }
  
        [Test]
        public async Task ChangeConnectionToOfflineIfFragmentClosedConfiguration()
        {
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.Clear();
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues.Clear();
            
            var boolTestDefaultProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestDefaultProperty")
                    .Item as IProperty;

            var defaultPropertyWithBoolFormatting = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultProperty")
                .Item as IRuntimePropertyViewModel;

            await _configurationFragmentViewModel.SetFragmentOpened(true);
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));

            var commandRead = _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY)
                .OptionCommand as RelayCommand;

            Assert.False(commandRead.CanExecute(null));

            Assert.True(await TestsUtils.WaitUntil(
                () => commandRead.CanExecute(null), 30000));
            await _configurationFragmentViewModel.SetFragmentOpened(false);

            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                boolTestDefaultProperty.Address] = 1;
            
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            Assert.False(commandRead.CanExecute(null));

            Assert.False((defaultPropertyWithBoolFormatting.LocalValue as EditableBoolValueViewModel).BoolValueProperty);
            await _configurationFragmentViewModel.SetFragmentOpened(true);
            Assert.True((defaultPropertyWithBoolFormatting.LocalValue as EditableBoolValueViewModel).BoolValueProperty);
            
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[
                boolTestDefaultProperty.Address] = 0;
        }
        
        [Test]
        public async Task ChangeConnectionToOfflineConnectionStateCheck()
        {
            _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.Clear();
            await _configurationFragmentViewModel.SetFragmentOpened(false);

            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));
         
            Assert.True(await TestsUtils.WaitUntil(
                () => _deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));


            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            Assert.True(await TestsUtils.WaitUntil(
                () => !_deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));

            await _configurationFragmentViewModel.SetFragmentOpened(true);
            
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection(_typesContainer));
         
            Assert.True(await TestsUtils.WaitUntil(
                () => _deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));


            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new OfflineConnection());

            Assert.True(await TestsUtils.WaitUntil(
                () => !_deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));
        }

        [Test]
        public async Task ConnectionLost()
        {

            var applicationGlobalCommandsMock = ApplicationGlobalCommandsMock.Create().WithAskUserGlobalResult(true);
            _typesContainer.RegisterInstance<IApplicationGlobalCommands>(applicationGlobalCommandsMock);
            


            _configurationFragmentViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues.Clear();
            var connection = new MockConnection(_typesContainer);
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device,connection);
            
            connection.SetConnectionLost(true);

            Assert.True(await TestsUtils.WaitUntil(() => _readCommand.CanExecute(null), 30000));
            _readCommand.Execute(null);

            Assert.True(await TestsUtils.WaitUntil(
                () => !_deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));
            
            connection.SetConnectionLost(false);
            Assert.True(await TestsUtils.WaitUntil(() => _readCommand.CanExecute(null), 30000));
            _readCommand.Execute(null);

            Assert.True(await TestsUtils.WaitUntil(
                () => _deviceViewModel.ConnectionStateViewModel.IsDeviceConnected, 30000));


            Assert.True(applicationGlobalCommandsMock.IsAskUserGlobalTriggered);

        }
    }
}