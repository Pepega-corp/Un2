using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Fragments.Configuration.Model.Conditions;
using Unicon2.Fragments.Configuration.Model.Filter;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Presentation.ViewModels.Fragment;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
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
        public async Task CreateSaveLoadAndCheckCodeFormatter()
        {
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            initialDevice.LoadDevice("FileAssets/testFile.json");

            var configurationEditorViewModel = initialDevice.FragmentEditorViewModels.First() as ConfigurationEditorViewModel;

            var baseAddress = 1000;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };
            
            var testCases=new List<CodeFormatterTestCase>();
            testCases.Add(new CodeFormatterTestCase("SetResultValue(Add(2,GetDeviceValue(0)))=>Select(number)",
                "SetDeviceValue(Subtract(GetInputValue(),2),0)",new []{(ushort)1},3,6,new []{(ushort)4}));

            foreach (var testCase in testCases)
            {
                IPropertyEditorViewModel rootProperty =
                    ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null) as IPropertyEditorViewModel;
                rootProperty.Address = (1000 + testCases.IndexOf(testCase)).ToString();
                rootProperty.NumberOfPoints = 1.ToString();
                rootProperty.Name = "codeFormatterProp" + testCases.IndexOf(testCase).ToString();
                rootProperty.NumberOfWriteFunction = (ushort) 16;
                rootGroup.ChildStructItemViewModels.Add(rootProperty);


                rootProperty.FormatterParametersViewModel = new FormatterParametersViewModel();
                rootProperty.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                    new CodeFormatterViewModel(StaticContainer.Container.Resolve<ICodeFormatterService>())
                    {
                        FormatCodeString = testCase.CodeStringFormat,
                        FormatBackCodeString = testCase.CodeStringFormatBack
                    };
            }


            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
           
            
            Program.CleanProject();
            var device = initialDevice.GetDevice();

            Program.GetApp().Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);
            var shell = Program.GetApp().Container.Resolve<ShellViewModel>();
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

          var configuration = device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                    IDeviceConfiguration;
            
            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;
            var command = configurationFragmentViewModel.FragmentOptionsViewModel.GetCommand("Device", "Read");
            await configurationFragmentViewModel.SetFragmentOpened(true);

            var mockConnection = new MockConnection();

            foreach (var testCase in testCases)
            {
                mockConnection.MemorySlotDictionary.AddElement((ushort) (1000 + testCases.IndexOf(testCase)),
                    testCase.InitialDeviceValue[0]);
            }



            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, mockConnection);
            shell.ActiveFragmentViewModel = new FragmentPaneViewModel()
            {
                FragmentViewModel = configurationFragmentViewModel
            };
            command.OptionCommand.Execute(null);
            
            
            Assert.True(await TestsUtils.WaitUntil(() => command.OptionCommand.CanExecute(null), 30000));
            await TransferFromDeviceToLocal(configuration, configurationFragmentViewModel);

            foreach (var testCase in testCases)
            {
                var formatterWithCodePropertyViewModel = configurationFragmentViewModel
                    .RootConfigurationItemViewModels
                    .Cast<IConfigurationItemViewModel>().ToList()
                    .FindItemViewModelByName(model =>
                        model.Header == "codeFormatterProp" + testCases.IndexOf(testCase).ToString())
                    .Item as IRuntimePropertyViewModel;

                Assert.True((formatterWithCodePropertyViewModel.DeviceValue as INumericValueViewModel).NumValue == testCase.InitialFormattedValue.ToString());
                Assert.True((formatterWithCodePropertyViewModel.LocalValue as INumericValueViewModel).NumValue == testCase.InitialFormattedValue.ToString());

            }

            
            
            

        }
        
        private async Task TransferFromDeviceToLocal(IDeviceConfiguration configuration, RuntimeConfigurationViewModel configurationFragmentViewModel)
        {
            var memoryAccessor = new ConfigurationMemoryAccessor(configuration,
                configurationFragmentViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal,true);
            await memoryAccessor.Process();
        }

    }
}
