using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.Model.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Shell;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        private ITypesContainer _typesContainer;
        private IDevice _device;
        private IDeviceConfiguration _configuration;
        private IDeviceViewModelFactory _deviceViewModelFactory;
        private IRuntimeConfigurationViewModel _configurationFragmentViewModel;

        public ConfigurationTests()
        {
            App app = new App();
            app.Initialize();
            _typesContainer = new TypesContainer(app.Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
            var serializerService = _typesContainer.Resolve<ISerializerService>();

            _device = serializerService.DeserializeFromFile<IDevice>("testFile.json");
            _configuration =

                _device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                    IDeviceConfiguration;


            _deviceViewModelFactory = _typesContainer.Resolve<IDeviceViewModelFactory>();
            var deviceMemory = new DeviceMemory();
            _device.DataProvider = new MockConnection(_typesContainer);
            _device.DeviceMemory = deviceMemory;
            _configurationFragmentViewModel = null;
            var deviceViewModel =
                _deviceViewModelFactory.CreateDeviceViewModel(_device, () => _configurationFragmentViewModel);
            _configurationFragmentViewModel =
                deviceViewModel.FragmentViewModels.First(model => model.NameForUiKey == "Configuration") as
                    RuntimeConfigurationViewModel;
        }



        [Test]
        public async Task BoolDefaultPropertyTest()
        {

            var boolTestDefaultProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestDefaultProperty")
                    .Item as IProperty;

            var defaultPropertyWithBoolFormatting = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultProperty")
                .Item as IRuntimePropertyViewModel;

            await new MemoryReaderVisitor(_configuration,
                _configurationFragmentViewModel.DeviceContext, 0).ExecuteRead();

            var memoryAccessor = new ConfigurationMemoryAccessor(_configuration,
                _configurationFragmentViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal);
            await memoryAccessor.Process();

            var deviceValue = defaultPropertyWithBoolFormatting.DeviceValue as IBoolValueViewModel;
            var localValue = defaultPropertyWithBoolFormatting.LocalValue as EditableBoolValueViewModel;

            Assert.False(deviceValue.BoolValueProperty);
            Assert.False(localValue.BoolValueProperty);

            Assert.True(localValue.IsEditEnabled);
            localValue.BoolValueProperty = true;
            Assert.True(localValue.IsFormattedValueChanged);
            if (LocalValuesWriteValidator.ValidateLocalValuesToWrite(_configurationFragmentViewModel
                .RootConfigurationItemViewModels))
            {
                await new MemoryWriterVisitor(_configurationFragmentViewModel.DeviceContext, new List<ushort>(),
                    _configuration, 0).ExecuteWrite();
            }


            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[boolTestDefaultProperty.Address].Equals(1));

            Assert.False(localValue.IsFormattedValueChanged);

        }

        [Test]
        public async Task BoolSubPropertyTest()
        {

            var boolTestSubProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty")
                    .Item as ISubProperty;

            var defaultPropertyWithBoolFormatting = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestSubProperty")
                .Item as IRuntimePropertyViewModel;

            await new MemoryReaderVisitor(_configuration,
                _configurationFragmentViewModel.DeviceContext, 0).ExecuteRead();

            var memoryAccessor = new ConfigurationMemoryAccessor(_configuration,
                _configurationFragmentViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal);
            await memoryAccessor.Process();

            var deviceValue = defaultPropertyWithBoolFormatting.DeviceValue as IBoolValueViewModel;
            var localValue = defaultPropertyWithBoolFormatting.LocalValue as EditableBoolValueViewModel;

            Assert.False(deviceValue.BoolValueProperty);
            Assert.False(localValue.BoolValueProperty);

            Assert.True(localValue.IsEditEnabled);
            localValue.BoolValueProperty = true;
            Assert.True(localValue.IsFormattedValueChanged);
            if (LocalValuesWriteValidator.ValidateLocalValuesToWrite(_configurationFragmentViewModel
                .RootConfigurationItemViewModels))
            {
                await new MemoryWriterVisitor(_configurationFragmentViewModel.DeviceContext, new List<ushort>(),
                    _configuration, 0).ExecuteWrite();
            }

            var boolsInDevice = _configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[boolTestSubProperty.Address].GetBoolArrayFromUshort();
            Assert.True(boolsInDevice[boolTestSubProperty.BitNumbersInWord.First()]);

            Assert.False(localValue.IsFormattedValueChanged);

        }

        [Test]
        public async Task MultipleBoolSubPropertyTest()
        {

            var boolTestSubProperty =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty")
                    .Item as ISubProperty;

            var boolTestSubProperty1 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty1")
                    .Item as ISubProperty;

            var boolTestSubProperty2 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty2")
                    .Item as ISubProperty;

            var boolTestSubProperty3 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty3")
                    .Item as ISubProperty;

            var boolTestSubProperty4 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty4")
                    .Item as ISubProperty;

            var boolTestSubProperty5 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty5")
                    .Item as ISubProperty;

            var boolTestSubProperty6 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "boolTestSubProperty6")
                    .Item as ISubProperty;


            var properties = new List<(ISubProperty, bool, IRuntimePropertyViewModel)>()
            {
                (boolTestSubProperty, false, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty.Name)
                    .Item as IRuntimePropertyViewModel),
                (boolTestSubProperty1, true, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty1.Name)
                    .Item as IRuntimePropertyViewModel),
                (boolTestSubProperty2, false, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty2.Name)
                    .Item as IRuntimePropertyViewModel),
                (boolTestSubProperty3, true, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty3.Name)
                    .Item as IRuntimePropertyViewModel),
                (boolTestSubProperty4, true, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty4.Name)
                    .Item as IRuntimePropertyViewModel),
                (boolTestSubProperty5, false, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty5.Name)
                    .Item as IRuntimePropertyViewModel),
                (boolTestSubProperty6, true, _configurationFragmentViewModel.RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model => model.Header == boolTestSubProperty6.Name)
                    .Item as IRuntimePropertyViewModel),
            };

            await new MemoryReaderVisitor(_configuration,
                _configurationFragmentViewModel.DeviceContext, 0).ExecuteRead();

            var memoryAccessor = new ConfigurationMemoryAccessor(_configuration,
                _configurationFragmentViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal);
            await memoryAccessor.Process();

            foreach (var propertyWithValueTuple in properties)
            {
                var deviceValue = propertyWithValueTuple.Item3.DeviceValue as IBoolValueViewModel;
                var localValue = propertyWithValueTuple.Item3.LocalValue as EditableBoolValueViewModel;

                Assert.False(deviceValue.BoolValueProperty);
                Assert.False(localValue.BoolValueProperty);

                Assert.True(localValue.IsEditEnabled);
                localValue.BoolValueProperty = propertyWithValueTuple.Item2;
                Assert.AreEqual(localValue.IsFormattedValueChanged, propertyWithValueTuple.Item2);

            }


            if (LocalValuesWriteValidator.ValidateLocalValuesToWrite(_configurationFragmentViewModel
                .RootConfigurationItemViewModels))
            {
                await new MemoryWriterVisitor(_configurationFragmentViewModel.DeviceContext, new List<ushort>(),
                    _configuration, 0).ExecuteWrite();
            }

            foreach (var propertyWithValueTuple in properties)
            {
                var localValue = propertyWithValueTuple.Item3.LocalValue as EditableBoolValueViewModel;

                var boolsInDevice = _configurationFragmentViewModel.DeviceContext.DeviceMemory
                    .DeviceMemoryValues[propertyWithValueTuple.Item1.Address].GetBoolArrayFromUshort();
                Assert.AreEqual(boolsInDevice[propertyWithValueTuple.Item1.BitNumbersInWord.First()],
                    propertyWithValueTuple.Item2);

                Assert.False(localValue.IsFormattedValueChanged);
            }


        }

        [Test]
        public async Task DependencyDefaultToDefaultPropertyCheckTest()
        {


            var boolTestPropertyDependencySource =
                _configuration.RootConfigurationItemList
                    .FindItemByName(item => item.Name == "boolTestDefaultPropertyOutputDependency")
                    .Item as IProperty;

            var boolTestPropertyDependencyConsumer =
                _configuration.RootConfigurationItemList
                    .FindItemByName(item => item.Name == "boolTestDefaultPropertyDependencyConsumer")
                    .Item as IProperty;

            var boolTestPropertyDependencySourceViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultPropertyOutputDependency")
                .Item as IRuntimePropertyViewModel;

            var boolTestPropertyDependencyConsumerViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultPropertyDependencyConsumer")
                .Item as IRuntimePropertyViewModel;


            await new MemoryReaderVisitor(_configuration,
                _configurationFragmentViewModel.DeviceContext, 0).ExecuteRead();

            var memoryAccessor = new ConfigurationMemoryAccessor(_configuration,
                _configurationFragmentViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal);
            await memoryAccessor.Process();


            Assert.True(boolTestPropertyDependencyConsumer.Dependencies.Any(delegate(IDependency dependency)
            {
                if (dependency is IConditionResultDependency conditionResultDependency)
                {
                    if (conditionResultDependency.Condition is CompareResourceCondition compareResourceCondition)
                    {
                        return compareResourceCondition.ConditionsEnum == ConditionsEnum.Equal &&
                               compareResourceCondition.UshortValueToCompare == 0;
                    }
                }
                return false;
            }));

            var localValueOfDependencySource = (boolTestPropertyDependencySourceViewModel.LocalValue as EditableNumericValueViewModel);
            var localValueOfDependencyConsumer = (boolTestPropertyDependencyConsumerViewModel.LocalValue as EditableBoolValueViewModel);


            Assert.True(localValueOfDependencySource.NumValue=="0");
            Assert.False(localValueOfDependencyConsumer.IsEditEnabled);

            localValueOfDependencySource.NumValue = "15";


        }
    }
}