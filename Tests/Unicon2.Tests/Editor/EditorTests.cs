using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Formatting.Model;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.Model.Conditions;
using Unicon2.Fragments.Configuration.Model.Filter;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Common;
using Unity;

namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class EditorTests
    {
        private TypesContainer _typesContainer;
        private int _addressModifier = 1;
        private int _numOfPointsModifier = 2;
        private int _nameModifier = 3;
        private int _numOfFunctionModifier = 3;


        public EditorTests()
        {

            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        [Test]
        public async Task EmptyEditorSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;
            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 0);
        }

        [Test]
        public async Task EditorAllFormattersPropSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 2);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 3);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 4);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 5);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 6);

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 1);

            var itemList = (result.RootConfigurationItemList[0] as DefaultItemsGroup).ConfigurationItemList;

            CheckPropertyResultProperty(itemList, 1);
            CheckPropertyResultProperty(itemList, 2);
            CheckPropertyResultProperty(itemList, 3);
            CheckPropertyResultProperty(itemList, 4);
            CheckPropertyResultProperty(itemList, 5);
            CheckPropertyResultProperty(itemList, 6);

            Assert.AreEqual(itemList.Count, 6);

        }

        [Test]
        public async Task EditorAllFormattersPropFromSharedResourcesSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var deviceSharedResources = new DeviceSharedResources();
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel =
                _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

            sharedResourcesGlobalViewModel.InitializeFromResources(deviceSharedResources);

            CreateFormatterParametersForResourcesViewModel(1);
            CreateFormatterParametersForResourcesViewModel(2);
            CreateFormatterParametersForResourcesViewModel(3);
            CreateFormatterParametersForResourcesViewModel(4);
            CreateFormatterParametersForResourcesViewModel(5);
            CreateFormatterParametersForResourcesViewModel(6);

            
            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 1);
            AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 2);
            AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 3);
            AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 4);
            AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 5);
            AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 6);

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 1);

            var itemList = (result.RootConfigurationItemList[0] as DefaultItemsGroup).ConfigurationItemList;

            CheckPropertyResultProperty(itemList, 1);
            CheckPropertyResultProperty(itemList, 2);
            CheckPropertyResultProperty(itemList, 3);
            CheckPropertyResultProperty(itemList, 4);
            CheckPropertyResultProperty(itemList, 5);
            CheckPropertyResultProperty(itemList, 6);

            Assert.AreEqual(itemList.Count, 6);

        }
    

        private void CreateFormatterParametersForResourcesViewModel(int identity)
        {
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel =
                _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();
            var formatterViewModel = CreateFormatterViewModel(identity);

            sharedResourcesGlobalViewModel.AddAsSharedResource(new FormatterParametersViewModel()
            {
                IsFromSharedResources = false,
                Name = "formatter" + identity,
                RelatedUshortsFormatterViewModel = formatterViewModel
            },false);

            ISaveFormatterService saveFormatterService = _typesContainer.Resolve<ISaveFormatterService>();

            IUshortsFormatter resourceUshortsFormatter =
                saveFormatterService.CreateUshortsParametersFormatter(formatterViewModel);
            resourceUshortsFormatter.Name = "formatter" + identity;
            sharedResourcesGlobalViewModel.UpdateSharedResource(resourceUshortsFormatter);
        }


        [Test]
        public async Task EditorAllFormattersRootPropSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 1);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 2);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 3);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 4);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 5);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 6);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);


            CheckPropertyResultProperty(result.RootConfigurationItemList, 1);
            CheckPropertyResultProperty(result.RootConfigurationItemList, 2);
            CheckPropertyResultProperty(result.RootConfigurationItemList, 3);
            CheckPropertyResultProperty(result.RootConfigurationItemList, 4);
            CheckPropertyResultProperty(result.RootConfigurationItemList, 5);
            CheckPropertyResultProperty(result.RootConfigurationItemList, 6);



            Assert.AreEqual(result.RootConfigurationItemList.Count, 6);
        }


        [Test]
        public async Task EditorBaseValuesSave()
        {
            var globalCommandsMock = ApplicationGlobalCommandsMock
                .Create()
                .WithAskUserGlobalResult(true);
            StaticContainer.Container.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);
            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            initialDevice.LoadDevice("FileAssets/testFile.json");

            var configurationEditorViewModel = initialDevice.FragmentEditorViewModels.First() as ConfigurationEditorViewModel;

            var memoryForBaseValues =
                serializerService.DeserializeFromFile<Dictionary<ushort, ushort>>(
                    "FileAssets/Конфигурация testFile.cnf");

            var baseValuesMemory = Result<Dictionary<ushort, ushort>>
                .Create( memoryForBaseValues, true);
            configurationEditorViewModel.BaseValuesViewModel = new BaseValuesViewModel();


            configurationEditorViewModel.BaseValuesViewModel.BaseValuesViewModels.Add(new BaseValueViewModel()
            {
                Name = "baseVal1",
                BaseValuesMemory = baseValuesMemory
            });
            configurationEditorViewModel.BaseValuesViewModel.BaseValuesViewModels.Add(new BaseValueViewModel()
            {
                Name = "baseVal2",
                BaseValuesMemory = baseValuesMemory
            });
            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);


            Assert.AreEqual(result.BaseValues.BaseValues.Count, 2);
            Assert.True(result.BaseValues.BaseValues[0].Name == "baseVal1");
            Assert.True(result.BaseValues.BaseValues[1].Name == "baseVal2");

            Program.CleanProject();
            var device = initialDevice.GetDevice();

            Program.GetApp().Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);
            var shell = Program.GetApp().Container.Resolve<ShellViewModel>();
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;
            var command = configurationFragmentViewModel.FragmentOptionsViewModel.GetCommand("BaseValues", "baseVal1");

            var editableChosenFromListValueViewModel =
                ((configurationFragmentViewModel.AllRows[4].ChildStructItemViewModels[2] as
                    ILocalAndDeviceValueContainingViewModel).LocalValue as EditableChosenFromListValueViewModel);
            Assert.AreEqual(
                editableChosenFromListValueViewModel.SelectedItem,
                "Нет");
            command.OptionCommand.Execute(null);
            Assert.AreEqual(
                    editableChosenFromListValueViewModel.SelectedItem,
                "Д2   Инв");

            editableChosenFromListValueViewModel.SelectedItem =
                editableChosenFromListValueViewModel.AvailableItemsList[1];
            Assert.AreEqual(
                editableChosenFromListValueViewModel.SelectedItem,
                "Д1   Инв");
            command.OptionCommand.Execute(null);
            Assert.AreEqual(
                editableChosenFromListValueViewModel.SelectedItem,
                "Д2   Инв");
        }

        [Test]
        public async Task EditorImportPropertiesTypeASave()
        {
            var globalCommandsMock = ApplicationGlobalCommandsMock
                .Create()
                .WithAskUserGlobalResult(true).WithSelectFileToOpenResult("FileAssets/МР5ПО60_ВЛС.xlsx");
            StaticContainer.Container.RegisterInstance<IApplicationGlobalCommands>(globalCommandsMock);
            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            initialDevice.LoadDevice("FileAssets/testFile.json");

            var configurationEditorViewModel =
                initialDevice.FragmentEditorViewModels.First() as ConfigurationEditorViewModel;



            var targetGroup = configurationEditorViewModel.RootConfigurationItemViewModels.First();

            targetGroup.ChildStructItemViewModels.Clear();

            var helper = Program.GetApp().Container.Resolve<ImportPropertiesFromExcelTypeAHelper>();

            helper.ImportPropertiesToGroup(targetGroup as IConfigurationGroupEditorViewModel);



            Program.CleanProject();
            var device = initialDevice.GetDevice();

            Program.GetApp().Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);
            var shell = Program.GetApp().Container.Resolve<ShellViewModel>();
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;

            var group = configurationEditorViewModel.RootConfigurationItemViewModels.First();

            Assert.AreEqual(group.ChildStructItemViewModels.Count, 7);

            Assert.AreEqual(group.ChildStructItemViewModels[4].ChildStructItemViewModels[4].Header, "F<<< СРАБ");

            Assert.AreEqual(group.ChildStructItemViewModels[3].ChildStructItemViewModels
                .Count, 16);
            Assert.True(
                (((device.DeviceFragments.First() as IDeviceConfiguration).RootConfigurationItemList[0] as
                    IItemsGroup).ConfigurationItemList[4] as IComplexProperty).SubProperties[4].UshortsFormatter is IBoolFormatter);

        }




        [Test]
        public async Task GroupFilterSaveLoad()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            rootGroup.FilterViewModels.AddRange(new ObservableCollection<IFilterViewModel>()
            {
                new FilterViewModel(new ObservableCollection<IConditionViewModel>())
                {
                    Name = "F1"
                },
                new FilterViewModel(new ObservableCollection<IConditionViewModel>()
                {
                    new CompareConditionViewModel(new List<string>()
                    {
                        "c1"
                    })
                    {
                        SelectedCondition = ConditionsEnum.Equal.ToString(),
                        UshortValueToCompare = 1
                    }
                })
                {
                    Name = "F2"
                }
            });

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);

            var groupFilter = (result.RootConfigurationItemList[0] as IItemsGroup).GroupFilter as GroupFilterInfo;
            Assert.True(groupFilter.Filters.Count == 2);

            DefaultFilter defaultFilter1 = groupFilter.Filters[0] as DefaultFilter;

            DefaultFilter defaultFilter2 = groupFilter.Filters[1] as DefaultFilter;
            Assert.True(defaultFilter1.Conditions.Count == 0);
            Assert.True(defaultFilter1.Name == "F1");

            Assert.True(defaultFilter2.Conditions.Count == 1);
            Assert.True(defaultFilter2.Name == "F2");

            var condition = defaultFilter2.Conditions[0] as CompareCondition;

            Assert.True(condition.ConditionsEnum == ConditionsEnum.Equal);
            Assert.True(condition.UshortValueToCompare == 1);


            ConfigurationItemEditorViewModelFactory configurationItemEditorViewModelFactory =
                ConfigurationItemEditorViewModelFactory.Create();

            var loadedRootItem =
                configurationItemEditorViewModelFactory.VisitItemsGroup(
                    result.RootConfigurationItemList[0] as IItemsGroup);


            FilterViewModel filterViewModel1 =
                (loadedRootItem as IConfigurationGroupEditorViewModel).FilterViewModels[0] as FilterViewModel;

            FilterViewModel filterViewModel2 =
                (loadedRootItem as IConfigurationGroupEditorViewModel).FilterViewModels[1] as FilterViewModel;

            Assert.True(filterViewModel1.Name == "F1");
            Assert.True(filterViewModel2.Name == "F2");

            Assert.True(filterViewModel1.ConditionViewModels.Count == 0);
            Assert.True(filterViewModel2.ConditionViewModels.Count == 1);

            CompareConditionViewModel compareConditionViewModel =
                filterViewModel2.ConditionViewModels[0] as CompareConditionViewModel;

            Assert.True(compareConditionViewModel.SelectedCondition == ConditionsEnum.Equal.ToString());
            Assert.True(compareConditionViewModel.UshortValueToCompare == 1);
        }

        [Test]
        public async Task RemoveSubproperty()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };


            var complexPropertyEditorViewModel =
                ConfigurationItemEditorViewModelFactory.Create().VisitComplexProperty(null) as
                    IComplexPropertyEditorViewModel;

            var subprop1 = ConfigurationItemEditorViewModelFactory.Create().WithParent(complexPropertyEditorViewModel)
                .VisitSubProperty(null) as ISubPropertyEditorViewModel;
            var subprop2 = ConfigurationItemEditorViewModelFactory.Create().WithParent(complexPropertyEditorViewModel)
                .VisitSubProperty(null) as ISubPropertyEditorViewModel;


            complexPropertyEditorViewModel.ChildStructItemViewModels.Add(subprop1);
            complexPropertyEditorViewModel.ChildStructItemViewModels.Add(subprop2);
            complexPropertyEditorViewModel.SubPropertyEditorViewModels.Add(subprop1);
            complexPropertyEditorViewModel.SubPropertyEditorViewModels.Add(subprop2);

            subprop1.BitNumbersInWord =
                complexPropertyEditorViewModel.MainBitNumbersInWordCollection;
            subprop2.BitNumbersInWord =
                complexPropertyEditorViewModel.MainBitNumbersInWordCollection;

            complexPropertyEditorViewModel.MainBitNumbersInWordCollection[0].ChangeValueByOwnerCommand
                .Execute(subprop1);
            complexPropertyEditorViewModel.MainBitNumbersInWordCollection[1].ChangeValueByOwnerCommand
                .Execute(subprop1);
            complexPropertyEditorViewModel.MainBitNumbersInWordCollection[2].ChangeValueByOwnerCommand
                .Execute(subprop2);
            complexPropertyEditorViewModel.MainBitNumbersInWordCollection[3].ChangeValueByOwnerCommand
                .Execute(subprop2);

            rootGroup.ChildStructItemViewModels.Add(complexPropertyEditorViewModel);

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);

            var resSubProp1 =
                ((result.RootConfigurationItemList[0] as IItemsGroup).ConfigurationItemList[0] as IComplexProperty)
                .SubProperties[0];
            var resSubProp2 =
                ((result.RootConfigurationItemList[0] as IItemsGroup).ConfigurationItemList[0] as IComplexProperty)
                .SubProperties[1];

            Assert.True(resSubProp1.BitNumbersInWord.Count == 2);
            Assert.True(resSubProp2.BitNumbersInWord.Count == 2);

            Assert.True(resSubProp1.BitNumbersInWord.Contains(15));
            Assert.True(resSubProp1.BitNumbersInWord.Contains(14));
            Assert.True(resSubProp2.BitNumbersInWord.Contains(13));
            Assert.True(resSubProp2.BitNumbersInWord.Contains(12));


            complexPropertyEditorViewModel.RemoveChildItem(subprop2);

            Assert.True(complexPropertyEditorViewModel.MainBitNumbersInWordCollection.All(model =>model.Owner!=subprop2 ));
            Assert.True(complexPropertyEditorViewModel.MainBitNumbersInWordCollection.First(model => model.NumberOfBit==13).Value==false);
            Assert.True(complexPropertyEditorViewModel.MainBitNumbersInWordCollection.First(model => model.NumberOfBit == 12).Value == false);

            Assert.True(complexPropertyEditorViewModel.MainBitNumbersInWordCollection.First(model => model.NumberOfBit == 13).Owner == null);
            Assert.True(complexPropertyEditorViewModel.MainBitNumbersInWordCollection.First(model => model.NumberOfBit == 12).Owner ==null);

        }


        private IUshortsFormatterViewModel CreateFormatterViewModel(int identity)
        {
            switch (identity)
            {
                case 1:
                    return new BoolFormatterViewModel();
                case 2:
                    return new AsciiStringFormatterViewModel();
                case 3:
                    return new DictionaryMatchingFormatterViewModel()
                    {
                        DefaultMessage = "jopa",
                        UseDefaultMessage = true,
                        IsKeysAreNumbersOfBits = true,
                        KeyValuesDictionary = new ObservableCollection<BindableKeyValuePair<ushort, string>>()
                        {
                            new BindableKeyValuePair<ushort, string>()
                            {
                                Key = 0, Value = "jopa0"
                            },
                            new BindableKeyValuePair<ushort, string>()
                            {
                                Key = 1, Value = "jopa1"
                            },
                            new BindableKeyValuePair<ushort, string>()
                            {
                                Key = 2, Value = "jopa2"
                            },
                        }
                    };
                case 4:
                    return new DirectFormatterViewModel();
                case 5:
                    return new StringFormatter1251ViewModel();
                case 6:
                    var formuleFormatter =
                        _typesContainer.Resolve<IUshortsFormatterViewModel>(StringKeys.FORMULA_FORMATTER +
                                                                            ApplicationGlobalNames
                                                                                .CommonInjectionStrings
                                                                                .VIEW_MODEL) as
                            IFormulaFormatterViewModel;
                    formuleFormatter.FormulaString = "x*2+1";
                    return formuleFormatter;
            }

            return null;
        }


        private void CheckFormatterResult(int identity, IUshortsFormatter formatter)
        {
            switch (identity)
            {
                case 1:
                    Assert.True(formatter is BoolFormatter);
                    break;
                case 2:
                    Assert.True(formatter is AsciiStringFormatter);
                    break;
                case 3:
                    Assert.True(formatter is DictionaryMatchingFormatter);
                    var dictMatchFormatter = (DictionaryMatchingFormatter) formatter;
                    Assert.True(dictMatchFormatter.UseDefaultMessage);
                    Assert.True(dictMatchFormatter.IsKeysAreNumbersOfBits);
                    Assert.AreEqual(dictMatchFormatter.DefaultMessage, "jopa");
                    Assert.AreEqual(dictMatchFormatter.StringDictionary.Count, 3);
                    Assert.AreEqual(dictMatchFormatter.StringDictionary[0], "jopa0");
                    Assert.AreEqual(dictMatchFormatter.StringDictionary[1], "jopa1");
                    Assert.AreEqual(dictMatchFormatter.StringDictionary[2], "jopa2");
                    break;
                case 4:
                    Assert.True(formatter is DirectUshortFormatter);
                    break;
                case 5:
                    Assert.True(formatter is StringFormatter1251);
                    break;
                case 6:
                    Assert.True(formatter is FormulaFormatter);
                    var formulaFormatter = (FormulaFormatter) formatter;
                    Assert.AreEqual(formulaFormatter.FormulaString, "x*2+1");

                    break;
            }

        }


        private void CheckPropertyResultProperty(List<IConfigurationItem> configurationItems, int identity)
        {
            var property = configurationItems[identity - 1] as IProperty;

            Assert.AreEqual(property.Address, (identity + _addressModifier));
            Assert.AreEqual(property.NumberOfPoints, (identity + _numOfPointsModifier));
            Assert.AreEqual(property.Name, (identity + _nameModifier).ToString());
            Assert.AreEqual(property.NumberOfWriteFunction, (identity + _numOfFunctionModifier));
            CheckFormatterResult(identity, property.UshortsFormatter);
        }

        private void InitFormatterViewModel(IPropertyEditorViewModel propertyEditorViewModel,
            IUshortsFormatterViewModel formatterViewModel)
        {
            propertyEditorViewModel.FormatterParametersViewModel = new FormatterParametersViewModel();
            propertyEditorViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                formatterViewModel;
        }

        private IPropertyEditorViewModel AddPropertyViewModel(
            ObservableCollection<IConfigurationItemViewModel> collection, int identity)
        {
            IPropertyEditorViewModel rootProperty =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
            rootProperty.Address = (identity + _addressModifier).ToString();
            rootProperty.NumberOfPoints = (identity + _numOfPointsModifier).ToString();
            rootProperty.Name = (identity + _nameModifier).ToString();
            rootProperty.NumberOfWriteFunction = (ushort) (identity + _numOfFunctionModifier);
            collection.Add(rootProperty);
            InitFormatterViewModel(rootProperty, CreateFormatterViewModel(identity));
            return rootProperty;
        }

        private IPropertyEditorViewModel AddPropertyWithFormatterFromResourceViewModel(
            ObservableCollection<IConfigurationItemViewModel> collection, int identity)
        {
            IPropertyEditorViewModel property =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
            property.Address = (identity + _addressModifier).ToString();
            property.NumberOfPoints = (identity + _numOfPointsModifier).ToString();
            property.Name = (identity + _nameModifier).ToString();
            property.NumberOfWriteFunction = (ushort) (identity + _numOfFunctionModifier);
            collection.Add(property);
            property.FormatterParametersViewModel = new FormatterParametersViewModel()
            {
                Name = "formatter" + identity,
                IsFromSharedResources = true,
            };
            return property;
        }
    }
}