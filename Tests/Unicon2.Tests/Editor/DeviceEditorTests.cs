using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Shell;
using Unicon2.Unity.Common;
using Unity;

namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class DeviceEditorTests
    {
        private TypesContainer _typesContainer;
        private int _addressModifier = 1;
        private int _numOfPointsModifier = 2;
        private int _nameModifier = 3;
        private int _numOfFunctionModifier = 3;


        public DeviceEditorTests()
        {
        
            _typesContainer = new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
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
        public async Task EditorAllFormattersRootPropSave()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootPropertyBool =
                AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 1);
            InitFormatterViewModel(rootPropertyBool, new BoolFormatterViewModel());

            var rootPropertyAscii =
                AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 2);
            InitFormatterViewModel(rootPropertyAscii, new AsciiStringFormatterViewModel());

            var rootPropertyDictMatch =
                AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 3);
            InitFormatterViewModel(rootPropertyDictMatch, new DictionaryMatchingFormatterViewModel());

            var rootPropertyDirect =
                AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 4);
            InitFormatterViewModel(rootPropertyDictMatch, new DirectFormatterViewModel());

            var rootProperty1251 =
                AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 5);
            InitFormatterViewModel(rootPropertyDictMatch, new StringFormatter1251ViewModel());

            var rootPropertyFormula =
                AddPropertyViewModel(configurationEditorViewModel.RootConfigurationItemViewModels, 6);
            InitFormatterViewModel(rootPropertyDictMatch, _typesContainer.Resolve<IUshortsFormatterViewModel>(StringKeys.FORMULA_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL));



            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);


            CheckPropertyResultProperty(result.RootConfigurationItemList, 1, typeof(BoolFormatter));
            CheckPropertyResultProperty(result.RootConfigurationItemList, 2, typeof(AsciiStringFormatter));
            CheckPropertyResultProperty(result.RootConfigurationItemList, 3, typeof(DictionaryMatchingFormatter));
            CheckPropertyResultProperty(result.RootConfigurationItemList, 4, typeof(DirectUshortFormatter));
            CheckPropertyResultProperty(result.RootConfigurationItemList, 5, typeof(StringFormatter1251));
            CheckPropertyResultProperty(result.RootConfigurationItemList, 6, typeof(FormulaFormatter));



            Assert.AreEqual(result.RootConfigurationItemList.Count, 6);
        }

        private void CheckPropertyResultProperty(List<IConfigurationItem> configurationItems, int identity,Type formatterType)
        {
            var property = configurationItems[identity-1] as IProperty;

            Assert.AreEqual(property.Address, (identity + _addressModifier));
            Assert.AreEqual(property.NumberOfPoints, (identity + _numOfPointsModifier));
            Assert.AreEqual(property.Name, (identity + _nameModifier).ToString());
            Assert.AreEqual(property.NumberOfWriteFunction, (identity + _numOfFunctionModifier));

        }

        private void InitFormatterViewModel(IPropertyEditorViewModel propertyEditorViewModel,
            IUshortsFormatterViewModel formatterViewModel)
        {
            propertyEditorViewModel.FormatterParametersViewModel = new FormatterParametersViewModel();
            propertyEditorViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                new BoolFormatterViewModel();
        }

        private IPropertyEditorViewModel AddPropertyViewModel(
            ObservableCollection<IConfigurationItemViewModel> collection, int identity)
        {
            IPropertyEditorViewModel rootProperty =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
            rootProperty.Address = (identity + _addressModifier).ToString();
            rootProperty.NumberOfPoints = (identity + _numOfPointsModifier).ToString();
            rootProperty.Name = (identity + _nameModifier).ToString();
            rootProperty.NumberOfWriteFunction = (ushort)(identity + _numOfFunctionModifier);
            collection.Add(rootProperty);
            return rootProperty;
        }
    }
}