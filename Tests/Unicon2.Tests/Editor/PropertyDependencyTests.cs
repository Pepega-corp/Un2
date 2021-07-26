using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.Model.Dependencies;
using Unicon2.Fragments.Configuration.Model.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Model.Dependencies.Results;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Common;
using Unity;
using static Unicon2.Tests.Utils.EditorHelpers;


namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class PropertyDependencyTests
    {
        private TypesContainer _typesContainer;

        public PropertyDependencyTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        private async Task TestCore(
            Action<(IPropertyEditorViewModel propWithDep, IPropertyEditorViewModel propRes)> setupEditorProps,
            Action<(IConfigurationItem propWithDepSaved, IConfigurationItem propResSaved)> setupSavedAssertions,
            Action<(IConfigurationItemViewModel propWithDepRuntime, IConfigurationItemViewModel propResRuntime)>
                setupActRuntime)
        {

            // get initial file
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            initialDevice.LoadDevice("FileAssets/testFile.json");

            var configurationEditorViewModel =
                initialDevice.FragmentEditorViewModels.First() as ConfigurationEditorViewModel;

            configurationEditorViewModel.RootConfigurationItemViewModels.Clear();


            // setup editor

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            var propWithDep = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);
            var propResource = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);

            var resourceService = _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

            resourceService.AddAsSharedResourceWithContainer(propResource, "resForProp", false);

            setupEditorProps((propWithDep, propResource));


            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            // load saved editor in runtime

            Program.CleanProject();
            var shell = _typesContainer.Resolve<ShellViewModel>();

            var device = initialDevice.GetDevice();

            _typesContainer.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new MockConnection());
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;

            var propWithDepSaved = TestsUtils.FindItemByName(
                (device.DeviceFragments.First(fragment => fragment is DefaultDeviceConfiguration) as
                    DefaultDeviceConfiguration).RootConfigurationItemList, item => item.Name == "propWithDep");

            var propResSaved = TestsUtils.FindItemByName(
                (device.DeviceFragments.First(fragment => fragment is DefaultDeviceConfiguration) as
                    DefaultDeviceConfiguration).RootConfigurationItemList, item => item.Name == "propRes");


            setupSavedAssertions((propWithDepSaved.Item, propResSaved.Item));


            await configurationFragmentViewModel.SetFragmentOpened(true);

            var readCommand = TestsUtils.GetFragmentCommand(configurationFragmentViewModel, "Device", "Read");
            readCommand.Execute(null);
            Assert.True(await TestsUtils.WaitUntil(() => readCommand.CanExecute(null), 30000));
            var propWithDepLoaded = TestsUtils.FindItemViewModelByName(
                configurationFragmentViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>()
                    .ToList(), item => item.Header == "propWithDep");


            var propResourceLoaded = TestsUtils.FindItemViewModelByName(
                configurationFragmentViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>()
                    .ToList(), item => item.Header == "propResource");


            setupActRuntime((propWithDepLoaded.Item, propResourceLoaded.Item));
        }

        [Test]
        public async Task HideDependencyTest()
        {
            Action<(IPropertyEditorViewModel propWithDep, IPropertyEditorViewModel propRes)> setupEditor = tuple =>
            {
                tuple.propRes.Address = 1000.ToString();
                tuple.propWithDep.Address = 1100.ToString();

                tuple.propRes.Header = "propResource";
                tuple.propWithDep.Header = "propWithDep";
                var resourceService = _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

                tuple.propWithDep.DependencyViewModels.Add(
                    new ConditionResultDependencyViewModel(new List<IResultViewModel>(),
                        new List<IConditionViewModel>())
                    {
                        SelectedConditionViewModel =
                            new CompareResourceConditionViewModel(resourceService, new List<string>())
                            {
                                ReferencedResourcePropertyName = "resForProp",
                                SelectedCondition = "Less",
                                UshortValueToCompare = 1
                            },
                        SelectedResultViewModel = new HidePropertyResultViewModel()
                    });
            };


            Action<(IConfigurationItem propWithDepSaved, IConfigurationItem propResSaved)> setupSavedAssertions = tuple =>
            {
                Assert.True((tuple.propWithDepSaved as IProperty).Dependencies.Count == 1);
                Assert.True(((tuple.propWithDepSaved as IProperty).Dependencies.First() as ConditionResultDependency).Result is HidePropertyResult);
            };


            Action<(IConfigurationItemViewModel propWithDepRuntime, IConfigurationItemViewModel propResRuntime)>
                setupActRuntime =
                    tuple =>
                    {
                        Assert.True((tuple.propWithDepRuntime as IRuntimePropertyViewModel).IsHidden);

                        ((tuple.propResRuntime as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                            .BoolValueProperty = true;

                        Assert.False((tuple.propWithDepRuntime as IRuntimePropertyViewModel).IsHidden);

                        ((tuple.propResRuntime as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                            .BoolValueProperty = false;
                        Assert.True((tuple.propWithDepRuntime as IRuntimePropertyViewModel).IsHidden);
                    };


           await TestCore(setupEditor, setupSavedAssertions, setupActRuntime);
        }

        [Test]
        public async Task RegexConditionDependencyTest()
        {
            Action<(IPropertyEditorViewModel propWithDep, IPropertyEditorViewModel propRes)> setupEditor = tuple =>
            {
                tuple.propRes.Address = 1000.ToString();
                tuple.propWithDep.Address = 1100.ToString();

                tuple.propRes.NumberOfPoints = 2.ToString();

                tuple.propRes.Header = "propResource";
                tuple.propWithDep.Header = "propWithDep";

                tuple.propRes.FormatterParametersViewModel.RelatedUshortsFormatterViewModel=new AsciiStringFormatterViewModel();

                var resourceService = _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

                tuple.propWithDep.DependencyViewModels.Add(
                    new ConditionResultDependencyViewModel(new List<IResultViewModel>(),
                        new List<IConditionViewModel>())
                    {
                        SelectedConditionViewModel =
                            new RegexMatchConditionViewModel(resourceService)
                            {
                                ReferencedResourcePropertyName = "resForProp",
                                RegexPattern = "te\\St"
                            },
                        SelectedResultViewModel = new HidePropertyResultViewModel()
                    });
            };


            Action<(IConfigurationItem propWithDepSaved, IConfigurationItem propResSaved)> setupSavedAssertions = tuple =>
            {
                Assert.True((tuple.propWithDepSaved as IProperty).Dependencies.Count == 1);
                Assert.True(((tuple.propWithDepSaved as IProperty).Dependencies.First() as ConditionResultDependency).Result is HidePropertyResult);
                var condition= ((tuple.propWithDepSaved as IProperty).Dependencies.First() as ConditionResultDependency).Condition
                    as RegexMatchCondition;

                 Assert.True(condition.ReferencedPropertyResourceName== "resForProp");
                 Assert.True(condition.RegexPattern == "te\\St");
            };


            Action<(IConfigurationItemViewModel propWithDepRuntime, IConfigurationItemViewModel propResRuntime)>
                setupActRuntime =
                    tuple =>
                    {
                        Assert.False((tuple.propWithDepRuntime as IRuntimePropertyViewModel).IsHidden);

                        ((tuple.propResRuntime as ILocalAndDeviceValueContainingViewModel).LocalValue as IStringValueViewModel).StringValue="test";

                        Assert.True((tuple.propWithDepRuntime as IRuntimePropertyViewModel).IsHidden);
                    };


            await TestCore(setupEditor, setupSavedAssertions, setupActRuntime);
        }


        [Test]
        public async Task DependencyForGroupWithRepeatTest()
        {
            // get initial file
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            initialDevice.LoadDevice("FileAssets/testFile.json");

            var configurationEditorViewModel =
                initialDevice.FragmentEditorViewModels.First() as ConfigurationEditorViewModel;

            configurationEditorViewModel.RootConfigurationItemViewModels.Clear();


            // setup editor

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            rootGroup.IsGroupWithReiteration = true;
            rootGroup.ReiterationStep = 10;
            rootGroup.SubGroupNames.AddCollection(new List<StringWrapper>()
            {
                new StringWrapper("gr1"),
                new StringWrapper("gr2")
            });

            var propWithDep = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);
            var propRes = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);

            var resourceService = _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

            resourceService.AddAsSharedResourceWithContainer(propRes, "resForProp", false);

            propRes.Address = 1000.ToString();
            propWithDep.Address = 1100.ToString();

            propRes.Header = "propResource";
            propWithDep.Header = "propWithDep";

            propWithDep.DependencyViewModels.Add(
                new ConditionResultDependencyViewModel(new List<IResultViewModel>(),
                    new List<IConditionViewModel>())
                {
                    SelectedConditionViewModel =
                        new CompareResourceConditionViewModel(resourceService, new List<string>())
                        {
                            ReferencedResourcePropertyName = "resForProp",
                            SelectedCondition = "Less",
                            UshortValueToCompare = 1
                        },
                    SelectedResultViewModel = new HidePropertyResultViewModel()
                });


            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            // load saved editor in runtime

            Program.CleanProject();
            var shell = _typesContainer.Resolve<ShellViewModel>();

            var device = initialDevice.GetDevice();

            _typesContainer.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new MockConnection());
            var deviceViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0];

            var configurationFragmentViewModel = shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;


            await configurationFragmentViewModel.SetFragmentOpened(true);

            var readCommand = TestsUtils.GetFragmentCommand(configurationFragmentViewModel, "Device", "Read");
            readCommand.Execute(null);
            Assert.True(await TestsUtils.WaitUntil(() => readCommand.CanExecute(null), 30000));
            var propsWithDepRuntime = TestsUtils.FindAllItemViewModelsByName(
                configurationFragmentViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>()
                    .ToList(), item => item.Header == "propWithDep");


            var propsResRuntime = TestsUtils.FindAllItemViewModelsByName(
                configurationFragmentViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>()
                    .ToList(), item => item.Header == "propResource");


            Assert.True((propsWithDepRuntime.First() as IRuntimePropertyViewModel).IsHidden);

            ((propsResRuntime.First() as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                .BoolValueProperty = true;

            Assert.False((propsWithDepRuntime.First() as IRuntimePropertyViewModel).IsHidden);

            ((propsResRuntime.First() as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                .BoolValueProperty = false;
            Assert.True((propsWithDepRuntime.First() as IRuntimePropertyViewModel).IsHidden);

            Assert.True((propsWithDepRuntime[1] as IRuntimePropertyViewModel).IsHidden);

            ((propsResRuntime[1] as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                .BoolValueProperty = true;

            Assert.False((propsWithDepRuntime[1] as IRuntimePropertyViewModel).IsHidden);

            ((propsResRuntime[1] as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                .BoolValueProperty = false;
            Assert.True((propsWithDepRuntime[1] as IRuntimePropertyViewModel).IsHidden);
        }

    }
}