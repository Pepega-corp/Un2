using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Conditions;
using Unicon2.Fragments.Configuration.Model.Filter;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Configuration
{
    public class CodeFormattingTests
    {
        private ITypesContainer _typesContainer;

        public CodeFormattingTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        private class CodeFormatterTestCase
        {
            public CodeFormatterTestCase(string codeStringFormat, string codeStringFormatBack,ushort[] initialDeviceValue, double initialFormattedValue, double modifiedFormattedValue, ushort[] modifiedDeviceValue)
            {
                InitialDeviceValue = initialDeviceValue;
                InitialFormattedValue = initialFormattedValue;
                ModifiedFormattedValue = modifiedFormattedValue;
                ModifiedDeviceValue = modifiedDeviceValue;
                CodeStringFormat = codeStringFormat;
                CodeStringFormatBack = codeStringFormatBack;
            }

            public string CodeStringFormat { get; }
            public string CodeStringFormatBack { get; }

            public ushort[] InitialDeviceValue { get; }
            public double InitialFormattedValue { get; }
            public double ModifiedFormattedValue { get; }
            public ushort[] ModifiedDeviceValue { get; }
        }
    

        [Test]
        public void CreateSaveLoadAndCheckCodeFormatter()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };
            
            var testCases=new List<CodeFormatterTestCase>();
            testCases.Add(new CodeFormatterTestCase("Add(2,GetDeviceValue(0))=>Select(double)",
                "SetDeviceValue(Subtract(GetInputValue(),2))",new []{(ushort)1},3,6,new []{(ushort)4}));
       
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
    }
}
