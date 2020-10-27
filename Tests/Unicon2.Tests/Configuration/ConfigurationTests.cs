using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Formatting.Infrastructure.Model;
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
using Unicon2.Presentation.Values;
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

            await ReadAndTransfer();

            var deviceValue = defaultPropertyWithBoolFormatting.DeviceValue as IBoolValueViewModel;
            var localValue = defaultPropertyWithBoolFormatting.LocalValue as EditableBoolValueViewModel;

            Assert.False(deviceValue.BoolValueProperty);
            Assert.False(localValue.BoolValueProperty);

            Assert.True(localValue.IsEditEnabled);
            localValue.BoolValueProperty = true;
            Assert.True(localValue.IsFormattedValueChanged);
            
            await Write();


            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[boolTestDefaultProperty.Address].Equals(1));

            Assert.False(localValue.IsFormattedValueChanged);

        }

        [Test]
        public async Task DefaultPropertyAllFormattersTest()
        {

            var defaultPropertyStringFormatter1251 =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "defaultPropertyStringFormatter1251")
                    .Item as IProperty;
            var defaultPropertyStringFormatter =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "defaultPropertyAsciiStringFormatter")
                    .Item as IProperty;
            var defaultPropertyNumericFormatter =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "defaultPropertyNumericFormatter")
                    .Item as IProperty;
            var defaultPropertySelectFormatter =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "defaultPropertySelectFormatter")
                    .Item as IProperty;
            var defaultPropertyFormulaFormatter =
                _configuration.RootConfigurationItemList.FindItemByName(item => item.Name == "defaultPropertyFormulaFormatter")
                    .Item as IProperty;
            
            
            var defaultPropertyStringFormatter1251ViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "defaultPropertyStringFormatter1251")
                .Item as IRuntimePropertyViewModel;
            var defaultPropertyStringFormatterViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "defaultPropertyAsciiStringFormatter")
                .Item as IRuntimePropertyViewModel;
            var defaultPropertyNumericFormatterViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "defaultPropertyNumericFormatter")
                .Item as IRuntimePropertyViewModel;
            var defaultPropertySelectFormatterViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "defaultPropertySelectFormatter")
                .Item as IRuntimePropertyViewModel;
            var defaultPropertyFormulaFormatterViewModel = _configurationFragmentViewModel.RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "defaultPropertyFormulaFormatter")
                .Item as IRuntimePropertyViewModel;
            
            await ReadAndTransfer();
            
            var defaultPropertyStringFormatter1251ViewModelDeviceValue = defaultPropertyStringFormatter1251ViewModel.DeviceValue as IStringValueViewModel;
            var defaultPropertyStringFormatter1251ViewModelLocalValue = defaultPropertyStringFormatter1251ViewModel.LocalValue as StringValueViewModel;
            
            var defaultPropertyStringFormatterViewModelDeviceValue = defaultPropertyStringFormatterViewModel.DeviceValue as IStringValueViewModel;
            var defaultPropertyStringFormatterViewModelLocalValue = defaultPropertyStringFormatterViewModel.LocalValue as StringValueViewModel;

            var defaultPropertyNumericFormatterViewModelDeviceValue = defaultPropertyNumericFormatterViewModel.DeviceValue as INumericValueViewModel;
            var defaultPropertyNumericFormatterViewModelLocalValue = defaultPropertyNumericFormatterViewModel.LocalValue as EditableNumericValueViewModel;
            
            var defaultPropertySelectFormatterViewModelDeviceValue = defaultPropertySelectFormatterViewModel.DeviceValue as IChosenFromListValueViewModel;
            var defaultPropertySelectFormatterViewModelLocalValue = defaultPropertySelectFormatterViewModel.LocalValue as EditableChosenFromListValueViewModel;
            
            var defaultPropertyFormulaFormatterViewModelDeviceValue = defaultPropertyFormulaFormatterViewModel.DeviceValue as INumericValueViewModel;
            var defaultPropertyFormulaFormatterViewModelLocalValue = defaultPropertyFormulaFormatterViewModel.LocalValue as EditableNumericValueViewModel;

            Assert.True(defaultPropertyStringFormatter1251ViewModelDeviceValue.StringValue.Replace("\0", "") == "");
            Assert.True(defaultPropertyStringFormatter1251ViewModelLocalValue.StringValue.Replace("\0", "") == "");

            Assert.True(defaultPropertyStringFormatterViewModelDeviceValue.StringValue.Replace("\0", "") == "");
            Assert.True(defaultPropertyStringFormatterViewModelLocalValue.StringValue.Replace("\0", "") == "");
            
            Assert.True(defaultPropertyNumericFormatterViewModelDeviceValue.NumValue=="0");
            Assert.True(defaultPropertyNumericFormatterViewModelLocalValue.NumValue=="0");
            
            Assert.True(defaultPropertySelectFormatterViewModelDeviceValue.SelectedItem=="1");
            Assert.True(defaultPropertySelectFormatterViewModelLocalValue.SelectedItem=="1");


            Assert.True((defaultPropertyFormulaFormatter.UshortsFormatter as IFormulaFormatter).FormulaString == "3*x+1");

            Assert.True(defaultPropertyFormulaFormatterViewModelDeviceValue.NumValue=="1");
            Assert.True(defaultPropertyFormulaFormatterViewModelLocalValue.NumValue=="1");
            

            Assert.False(defaultPropertyStringFormatter1251ViewModelLocalValue.IsEditEnabled);
            Assert.False(defaultPropertyStringFormatterViewModelLocalValue.IsEditEnabled);
            Assert.True(defaultPropertyNumericFormatterViewModelLocalValue.IsEditEnabled);
            Assert.True(defaultPropertySelectFormatterViewModelLocalValue.IsEditEnabled);
            Assert.True(defaultPropertyFormulaFormatterViewModelLocalValue.IsEditEnabled);

            defaultPropertyNumericFormatterViewModelLocalValue.NumValue = "225";
            defaultPropertySelectFormatterViewModelLocalValue.SelectedItem =
                defaultPropertySelectFormatterViewModelLocalValue.AvailableItemsList[1];
            defaultPropertyFormulaFormatterViewModelLocalValue.NumValue = "20";
            
            Assert.False(defaultPropertyStringFormatter1251ViewModelLocalValue.IsFormattedValueChanged);
            Assert.False(defaultPropertyStringFormatterViewModelLocalValue.IsFormattedValueChanged);
            Assert.True(defaultPropertyNumericFormatterViewModelLocalValue.IsFormattedValueChanged);
            Assert.True(defaultPropertySelectFormatterViewModelLocalValue.IsFormattedValueChanged);
            Assert.True(defaultPropertyFormulaFormatterViewModelLocalValue.IsFormattedValueChanged);

            await Write();
            
            Assert.False(defaultPropertyStringFormatter1251ViewModelLocalValue.IsFormattedValueChanged);
            Assert.False(defaultPropertyStringFormatterViewModelLocalValue.IsFormattedValueChanged);
            Assert.False(defaultPropertyNumericFormatterViewModelLocalValue.IsFormattedValueChanged);
            Assert.False(defaultPropertySelectFormatterViewModelLocalValue.IsFormattedValueChanged);
            Assert.False(defaultPropertyFormulaFormatterViewModelLocalValue.IsFormattedValueChanged);

             

            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[defaultPropertyStringFormatter1251.Address].Equals(0));
            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[defaultPropertyStringFormatter.Address].Equals(0));
            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[defaultPropertyNumericFormatter.Address].Equals(225));
            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[defaultPropertySelectFormatter.Address].Equals(1));
            Assert.True(_configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[defaultPropertyFormulaFormatter.Address].Equals(6));

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

            await ReadAndTransfer();

            var deviceValue = defaultPropertyWithBoolFormatting.DeviceValue as IBoolValueViewModel;
            var localValue = defaultPropertyWithBoolFormatting.LocalValue as EditableBoolValueViewModel;

            Assert.False(deviceValue.BoolValueProperty);
            Assert.False(localValue.BoolValueProperty);

            Assert.True(localValue.IsEditEnabled);
            localValue.BoolValueProperty = true;
            Assert.True(localValue.IsFormattedValueChanged);
            
            await Write();

            var boolsInDevice = _configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[boolTestSubProperty.Address].GetBoolArrayFromUshort();
            Assert.True(boolsInDevice[boolTestSubProperty.BitNumbersInWord.First()]);

            Assert.False(localValue.IsFormattedValueChanged);

            
            
            localValue.BoolValueProperty = false;
            Assert.True(localValue.IsFormattedValueChanged);
            
            await Write();

            boolsInDevice = _configurationFragmentViewModel.DeviceContext.DeviceMemory
                .DeviceMemoryValues[boolTestSubProperty.Address].GetBoolArrayFromUshort();
            Assert.False(boolsInDevice[boolTestSubProperty.BitNumbersInWord.First()]);
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

            await ReadAndTransfer();

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


            await Write();

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

            var boolTestPropertyDependencySourceViewModel = _configurationFragmentViewModel
                .RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultPropertyOutputDependency")
                .Item as IRuntimePropertyViewModel;

            var boolTestPropertyDependencyConsumerViewModel = _configurationFragmentViewModel
                .RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestDefaultPropertyDependencyConsumer")
                .Item as IRuntimePropertyViewModel;


            await ReadAndTransfer();

            for (int i = 0; i < 2; i++)
            {
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

                Func<EditableNumericValueViewModel> localValueOfDependencySource = () =>
                    (boolTestPropertyDependencySourceViewModel.LocalValue as EditableNumericValueViewModel);
                Func<EditableBoolValueViewModel> localValueOfDependencyConsumer = () =>
                    (boolTestPropertyDependencyConsumerViewModel.LocalValue as EditableBoolValueViewModel);

                Assert.True(localValueOfDependencySource().NumValue == "0");
                Assert.False(localValueOfDependencyConsumer().IsEditEnabled);

                localValueOfDependencySource().NumValue = "15";

                Assert.True(localValueOfDependencySource().IsFormattedValueChanged);
                Assert.True(localValueOfDependencyConsumer().IsEditEnabled);
                localValueOfDependencyConsumer().BoolValueProperty = true;
                Assert.True(localValueOfDependencyConsumer().BoolValueProperty);
                Assert.True(localValueOfDependencyConsumer().IsFormattedValueChanged);


                await Write();


                Assert.AreEqual(_device.DeviceMemory.DeviceMemoryValues[boolTestPropertyDependencySource.Address],
                    15);
                Assert.AreEqual(_device.DeviceMemory.DeviceMemoryValues[boolTestPropertyDependencyConsumer.Address],
                    1);
                Assert.False(localValueOfDependencySource().IsFormattedValueChanged);
                Assert.False(localValueOfDependencyConsumer().IsFormattedValueChanged);




                localValueOfDependencySource().NumValue = "0";

                Assert.True(localValueOfDependencySource().IsFormattedValueChanged);
                Assert.False(localValueOfDependencyConsumer().IsEditEnabled);

                await Write();
                Assert.AreEqual(_device.DeviceMemory.DeviceMemoryValues[boolTestPropertyDependencySource.Address],
                    0);
                Assert.AreEqual(_device.DeviceMemory.DeviceMemoryValues[boolTestPropertyDependencyConsumer.Address],
                    1);
                Assert.False(localValueOfDependencySource().IsFormattedValueChanged);
                Assert.False(localValueOfDependencyConsumer().IsFormattedValueChanged);




                localValueOfDependencySource().NumValue = "15";

                Assert.True(localValueOfDependencySource().IsFormattedValueChanged);
                Assert.True(localValueOfDependencyConsumer().IsEditEnabled);
                localValueOfDependencyConsumer().BoolValueProperty = false;
                Assert.False(localValueOfDependencyConsumer().BoolValueProperty);
                Assert.True(localValueOfDependencyConsumer().IsFormattedValueChanged);


                await Write();


                Assert.AreEqual(_device.DeviceMemory.DeviceMemoryValues[boolTestPropertyDependencySource.Address],
                    15);
                Assert.AreEqual(_device.DeviceMemory.DeviceMemoryValues[boolTestPropertyDependencyConsumer.Address],
                    0);
                Assert.False(localValueOfDependencySource().IsFormattedValueChanged);
                Assert.False(localValueOfDependencyConsumer().IsFormattedValueChanged);
                
                
                localValueOfDependencySource().NumValue = "0";
                await Write();

            }

        }


        [Test]
        public async Task DependencySubpropertyToSubproperty()
        {

            var boolTestSubPropertyDependencySource =
                _configuration.RootConfigurationItemList
                    .FindItemByName(item => item.Name == "boolTestSubPropertyDependencySource")
                    .Item as IProperty;

            var boolTestSubPropertyDependencyConsumer =
                _configuration.RootConfigurationItemList
                    .FindItemByName(item => item.Name == "boolTestSubPropertyDependencyConsumer")
                    .Item as IProperty;

            var boolTestSubPropertyDependencySourceViewModel = _configurationFragmentViewModel
                .RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestSubPropertyDependencySource")
                .Item as IRuntimePropertyViewModel;

            var boolTestSubPropertyDependencyConsumerViewModel = _configurationFragmentViewModel
                .RootConfigurationItemViewModels
                .Cast<IConfigurationItemViewModel>().ToList()
                .FindItemViewModelByName(model => model.Header == "boolTestSubPropertyDependencyConsumer")
                .Item as IRuntimePropertyViewModel;
            await ReadAndTransfer();



        }




        private async Task ReadAndTransfer()
        {
            await Read();
            await TransferFromDeviceToLocal();
        }
        
        private async Task TransferFromDeviceToLocal()
        {
            var memoryAccessor = new ConfigurationMemoryAccessor(_configuration,
                _configurationFragmentViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal);
            await memoryAccessor.Process();
        }
        private async Task Read()
        {
            await new MemoryReaderVisitor(_configuration,
                _configurationFragmentViewModel.DeviceContext, 0).ExecuteRead();

            
        }
        private async Task Write()
        {
            if (LocalValuesWriteValidator.ValidateLocalValuesToWrite(_configurationFragmentViewModel
                .RootConfigurationItemViewModels))
            {
                await new MemoryWriterVisitor(_configurationFragmentViewModel.DeviceContext, new List<ushort>(),
                    _configuration, 0).ExecuteWrite();
            }
        }
        
        
    }
}