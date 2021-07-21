using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
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
using Unicon2.Fragments.Configuration.Model.Dependencies.Results;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
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
    public class HidePropertyDependencyTests
    {
        private TypesContainer _typesContainer;

        public HidePropertyDependencyTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        [Test]
        public async Task ShouldSaveHideDependencyResultPasteRootProps()
        {
            IResultingDeviceViewModel initialDevice = Program.GetApp().Container.Resolve<IResultingDeviceViewModel>();
            initialDevice.LoadDevice("FileAssets/testFile.json");

            var configurationEditorViewModel =
                initialDevice.FragmentEditorViewModels.First() as ConfigurationEditorViewModel;

            configurationEditorViewModel.RootConfigurationItemViewModels.Clear();

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            var propWithDep=AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1,_typesContainer);
            var propResource=AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1,_typesContainer);

            var resourceService = _typesContainer.Resolve<ISharedResourcesGlobalViewModel>();

            resourceService.AddAsSharedResourceWithContainer(propResource,"resForHideProp",false);

            propResource.Address = 1000.ToString();
            propWithDep.Address = 1100.ToString();

            propResource.Header = "propResource";
            propWithDep.Header = "propWithDep";

            propWithDep.DependencyViewModels.Add(new ConditionResultDependencyViewModel(new List<IResultViewModel>(),new List<IConditionViewModel>() )
            {
                SelectedConditionViewModel = new CompareResourceConditionViewModel(resourceService,new List<string>())
                {
                    ReferencedResourcePropertyName = "resForHideProp",
                    SelectedCondition = "Less",
                    UshortValueToCompare = 1
                },
                SelectedResultViewModel = new HidePropertyResultViewModel()
            });

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);


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

            var propWithDepSaved=TestsUtils.FindItemByName(
                (device.DeviceFragments.First(fragment => fragment is DefaultDeviceConfiguration) as
                    DefaultDeviceConfiguration).RootConfigurationItemList, item => item.Name == "propWithDep");

            Assert.True((propWithDepSaved.Item as IProperty).Dependencies.Count==1);
            Assert.True(((propWithDepSaved.Item as IProperty).Dependencies.First() as ConditionResultDependency).Result is HidePropertyResult);
            await configurationFragmentViewModel.SetFragmentOpened(true);

            var readCommand = TestsUtils.GetFragmentCommand(configurationFragmentViewModel, "Device", "Read");
            readCommand.Execute(null);
            Assert.True(await TestsUtils.WaitUntil(() => readCommand.CanExecute(null), 30000));
            var propWithDepLoaded = TestsUtils.FindItemViewModelByName(
                configurationFragmentViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>().ToList(), item => item.Header == "propWithDep");
           
            
            var propResourceLoaded = TestsUtils.FindItemViewModelByName(
                configurationFragmentViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>().ToList(), item => item.Header == "propResource");

            Assert.True((propWithDepLoaded.Item as IRuntimePropertyViewModel).IsHidden);

            ((propResourceLoaded.Item as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                .BoolValueProperty = true;

            Assert.False((propWithDepLoaded.Item as IRuntimePropertyViewModel).IsHidden);

            ((propResourceLoaded.Item as ILocalAndDeviceValueContainingViewModel).LocalValue as IBoolValueViewModel)
                 .BoolValueProperty = false;
            Assert.True((propWithDepLoaded.Item as IRuntimePropertyViewModel).IsHidden);

        }

    }
}