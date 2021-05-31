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
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unity;
using static Unicon2.Tests.Utils.EditorHelpers;

namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class EditorTests
    {
        private TypesContainer _typesContainer;
  


        public EditorTests()
        {

            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        [Test]
        public void EmptyEditorSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;
            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 0);
        }

        [Test]
        public void EditorAllFormattersPropSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1,_typesContainer);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 2, _typesContainer);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 3, _typesContainer);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 4, _typesContainer);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 5, _typesContainer);
            AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 6, _typesContainer);

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
        public void EditorAllFormattersPropCopySave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

           var original1= AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);
           var original2 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 2, _typesContainer);
           var original3 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 3, _typesContainer);
           var original4 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 4, _typesContainer);
           var original5 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 5, _typesContainer);
           var original6 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 6, _typesContainer);

           rootGroup.ChildStructItemViewModels.Add(original1.Clone() as IConfigurationItemViewModel);
           rootGroup.ChildStructItemViewModels.Add(original2.Clone() as IConfigurationItemViewModel);
           rootGroup.ChildStructItemViewModels.Add(original3.Clone() as IConfigurationItemViewModel);
           rootGroup.ChildStructItemViewModels.Add(original4.Clone() as IConfigurationItemViewModel);
           rootGroup.ChildStructItemViewModels.Add(original5.Clone() as IConfigurationItemViewModel);
           rootGroup.ChildStructItemViewModels.Add(original6.Clone() as IConfigurationItemViewModel);


            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 1);

            var itemList = (result.RootConfigurationItemList[0] as DefaultItemsGroup).ConfigurationItemList;
            Assert.AreEqual(itemList.Count, 12);

            CheckPropertyResultProperty(itemList, 1);
            CheckPropertyResultProperty(itemList, 2);
            CheckPropertyResultProperty(itemList, 3);
            CheckPropertyResultProperty(itemList, 4);
            CheckPropertyResultProperty(itemList, 5);
            CheckPropertyResultProperty(itemList, 6);
            var copiesList = itemList.Skip(6).Take(6).ToList();

            CheckPropertyResultProperty(copiesList, 1);
            CheckPropertyResultProperty(copiesList, 2);
            CheckPropertyResultProperty(copiesList, 3);
            CheckPropertyResultProperty(copiesList, 4);
            CheckPropertyResultProperty(copiesList, 5);
            CheckPropertyResultProperty(copiesList, 6);
        }

        [Test]
        public void EditorCopyPropAsSharedResources()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var deviceSharedResources = new DeviceSharedResources();
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel =
                _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

            sharedResourcesGlobalViewModel.InitializeFromResources(deviceSharedResources);

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };
            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var addedRow = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1,_typesContainer);
            
           configurationEditorViewModel.SelectedRow = addedRow;

         
           sharedResourcesGlobalViewModel.AddAsSharedResourceWithContainer(addedRow, false);

            configurationEditorViewModel.CopyElementCommand.Execute(null);

           configurationEditorViewModel.SelectedRow = rootGroup;

            configurationEditorViewModel.PasteAsChildElementCommand.Execute(null);


            var copiedRow = rootGroup.ChildStructItemViewModels[1];

            configurationEditorViewModel.SelectedRow = (IEditorConfigurationItemViewModel) copiedRow;


            Assert.True((configurationEditorViewModel.AddSelectedElementAsResourceCommand as RelayCommand).CanExecute(null));

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 1);

            var itemList = (result.RootConfigurationItemList[0] as DefaultItemsGroup).ConfigurationItemList;

            CheckPropertyResultProperty(itemList, 1);
            CheckPropertyResultProperty(itemList, 1,1);

            Assert.AreEqual(itemList.Count, 2);

        }

        [Test]
        public void EditorAllFormattersPropFromSharedResourcesSave()
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
            var formatterViewModel = CreateFormatterViewModel(identity, _typesContainer);

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
        public void EditorAllFormattersRootPropSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 1, _typesContainer);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 2, _typesContainer);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 3, _typesContainer);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 4, _typesContainer);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 5, _typesContainer);
            AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 6, _typesContainer);

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
        public void EditorBaseValuesSave()
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
        public void EditorImportPropertiesTypeASave()
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
        public void GroupFilterSaveLoad()
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
        public void RemoveSubproperty()
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



    }
}