using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.DeviceEditorUtilityModule.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Common;
using Unity;

namespace Unicon2.Tests.Editor
{
    [TestFixture]

    public class EditorValidatorTests
    {
        private TypesContainer _typesContainer;


        public EditorValidatorTests()
        {

            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        [Test]
        public async Task ValidateEditorConfiguration()
        {
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();


            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            EditorHelpers.AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            
            initialDevice.FragmentEditorViewModels
                .Add(configurationEditorViewModel);

           var res= _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>() {configurationEditorViewModel});

            Assert.True(res.Count==0);

            var newDevice = initialDevice.GetDevice();


            IResultingDeviceViewModel newDeviceViewModel = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            newDeviceViewModel.LoadDevice(newDevice);

            var resNew = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>()
                {
                    newDeviceViewModel.FragmentEditorViewModels.First()
                });

            Assert.True(resNew.Count == 0);


        }


        [Test]
        public void ValidateEditorConfigurationWithMissingResource()
        {
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();


            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            EditorHelpers.AddPropertyWithFormatterFromResourceViewModel(rootGroup.ChildStructItemViewModels, 1);

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);


            initialDevice.FragmentEditorViewModels
                .Add(configurationEditorViewModel);

            var res = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>() { configurationEditorViewModel });

            Assert.True(res.Count == 1);

            var newDevice = initialDevice.GetDevice();


            IResultingDeviceViewModel newDeviceViewModel = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            newDeviceViewModel.LoadDevice(newDevice);

            var resNew = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>()
                {
                    newDeviceViewModel.FragmentEditorViewModels.First()
                });

            Assert.True(resNew.Count == 1);


        }

        [Test]
        public void ValidateEditorConfigurationWithError()
        {
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();


            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

           var property= EditorHelpers.AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);
           property.FormatterParametersViewModel.RelatedUshortsFormatterViewModel = null;

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);


            initialDevice.FragmentEditorViewModels
                .Add(configurationEditorViewModel);

            var res = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>() { configurationEditorViewModel });

            Assert.True(res.Count == 1);

            var newDevice = initialDevice.GetDevice();


            IResultingDeviceViewModel newDeviceViewModel = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            newDeviceViewModel.LoadDevice(newDevice);

            var resNew = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>()
                {
                    newDeviceViewModel.FragmentEditorViewModels.First()
                });

            Assert.True(resNew.Count == 1);

        }


        [Test]
        public void ValidateEditorConfigurationWithErrorComplexProperty()
        {
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();


            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            var property = _typesContainer.Resolve<IComplexPropertyEditorViewModel>();
            rootGroup.ChildStructItemViewModels
                .Add(property);
            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);


            initialDevice.FragmentEditorViewModels
                .Add(configurationEditorViewModel);

            var res = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>() { configurationEditorViewModel });

            Assert.True(res.Count == 0);

            var newDevice = initialDevice.GetDevice();


            IResultingDeviceViewModel newDeviceViewModel = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            newDeviceViewModel.LoadDevice(newDevice);

            var resNew = _typesContainer.Resolve<IDeviceEditorViewModelValidator>()
                .ValidateDeviceEditor(new List<IFragmentEditorViewModel>()
                {
                    newDeviceViewModel.FragmentEditorViewModels.First()
                });

            Assert.True(resNew.Count == 0);

        }
    }
}