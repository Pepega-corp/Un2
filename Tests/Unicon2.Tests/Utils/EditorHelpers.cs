using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Editor.ViewModels.InnerMembers;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers;
using Unicon2.Formatting.Model;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Tests.Utils
{
    public class EditorHelpers
    {
        private static int _addressModifier = 1;
        private static int _numOfPointsModifier = 2;
        private static int _nameModifier = 3;
        private static int _numOfFunctionModifier = 3;



        public static void InitFormatterViewModel(IPropertyEditorViewModel propertyEditorViewModel,
            IUshortsFormatterViewModel formatterViewModel)
        {
            propertyEditorViewModel.FormatterParametersViewModel = new FormatterParametersViewModel();
            propertyEditorViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                formatterViewModel;
        }

        public static IPropertyEditorViewModel AddPropertyViewModel(
            IList<IConfigurationItemViewModel> collection, int identity, ITypesContainer typesContainer)
        {
            IPropertyEditorViewModel rootProperty =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
            rootProperty.Address = (identity + _addressModifier).ToString();
            rootProperty.NumberOfPoints = (identity + _numOfPointsModifier).ToString();
            rootProperty.Name = (identity + _nameModifier).ToString();
            rootProperty.NumberOfWriteFunction = (ushort)(identity + _numOfFunctionModifier);
            collection?.Add(rootProperty);

            InitFormatterViewModel(rootProperty, CreateFormatterViewModel(identity,typesContainer));
            return rootProperty;
        }
        public static IPropertyEditorViewModel AddPropertyWithBitsViewModel(
            IList<IConfigurationItemViewModel> collection, int identity, ITypesContainer typesContainer)
        {
            IPropertyEditorViewModel rootProperty =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
            rootProperty.Address = (identity + _addressModifier).ToString();
            rootProperty.NumberOfPoints = (identity + _numOfPointsModifier).ToString();
            rootProperty.Name = (identity + _nameModifier).ToString();
            rootProperty.NumberOfWriteFunction = (ushort)(identity + _numOfFunctionModifier);
            collection?.Add(rootProperty);

            rootProperty.IsFromBits = true;
            rootProperty.BitNumbersInWord.FirstOrDefault(model => model.BitNumber == identity).IsChecked = true;

            InitFormatterViewModel(rootProperty, CreateFormatterViewModel(identity, typesContainer));
            return rootProperty;
        }


        public static IPropertyEditorViewModel AddPropertyWithFormatterFromResourceViewModel(
            ObservableCollection<IConfigurationItemViewModel> collection, int identity)
        {
            IPropertyEditorViewModel property =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
            property.Address = (identity + _addressModifier).ToString();
            property.NumberOfPoints = (identity + _numOfPointsModifier).ToString();
            property.Name = (identity + _nameModifier).ToString();
            property.NumberOfWriteFunction = (ushort)(identity + _numOfFunctionModifier);
            collection.Add(property);
            property.FormatterParametersViewModel = new FormatterParametersViewModel()
            {
                Name = "formatter" + identity,
                IsFromSharedResources = true,
            };
            return property;
        }


        public static void CheckPropertyFromBitsResultProperty(List<IConfigurationItem> configurationItems, int identity, int numberInList = -1)
        {
            var num = 0;
            if (numberInList != -1)
            {
                num = numberInList;
            }
            else
            {
                num = identity;
            }
            var property = configurationItems[num - 1] as IProperty;

            Assert.AreEqual(property.Address, (identity + _addressModifier));
            Assert.AreEqual(property.NumberOfPoints, (identity + _numOfPointsModifier));
            Assert.AreEqual(property.Name, (identity + _nameModifier).ToString());
            Assert.AreEqual(property.NumberOfWriteFunction, (identity + _numOfFunctionModifier));

            Assert.AreEqual(property.IsFromBits, true);
            Assert.AreEqual(property.BitNumbers[0], identity);
            Assert.AreEqual(property.BitNumbers.Count, 1);


            CheckFormatterResult(identity, property.UshortsFormatter);
        }

        public static void CheckPropertyResultProperty(List<IConfigurationItem> configurationItems, int identity, int numberInList=-1)
        {
            var num = 0;
            if (numberInList != -1)
            {
                num = numberInList;
            }
            else
            {
                num = identity;
            }
            var property = configurationItems[num - 1] as IProperty;

            Assert.AreEqual(property.Address, (identity + _addressModifier));
            Assert.AreEqual(property.NumberOfPoints, (identity + _numOfPointsModifier));
            Assert.AreEqual(property.Name, (identity + _nameModifier).ToString());
            Assert.AreEqual(property.NumberOfWriteFunction, (identity + _numOfFunctionModifier));
            CheckFormatterResult(identity, property.UshortsFormatter);
        }

        public static IUshortsFormatterViewModel CreateFormatterViewModel(int identity, ITypesContainer typesContainer)
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
                        typesContainer.Resolve<IUshortsFormatterViewModel>(StringKeys.FORMULA_FORMATTER +
                                                                           ApplicationGlobalNames
                                                                               .CommonInjectionStrings
                                                                               .VIEW_MODEL) as
                            IFormulaFormatterViewModel;
                    formuleFormatter.FormulaString = "x*2+" + identity;
                    formuleFormatter.ArgumentViewModels.Add(
                        new ArgumentViewModel()
                        {
                            ArgumentName = "1",
                            ResourceNameString = "testRes" + identity
                        });

                    return formuleFormatter;
            }

            return null;
        }


        public static void CheckFormatterResult(int identity, IUshortsFormatter formatter)
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
                    Assert.AreEqual(formulaFormatter.FormulaString, $"x*2+{identity}");
                    Assert.AreEqual(formulaFormatter.UshortFormattableResources.First(), "testRes" + identity);

                    break;
            }

        }
    }
}
