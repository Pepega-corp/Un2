using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Infrastructure;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unity;
using static Unicon2.Tests.Utils.EditorHelpers;

namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class CopyPasteTests
    {
        private TypesContainer _typesContainer;

        public CopyPasteTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
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

            var addedRow = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);

            configurationEditorViewModel.SelectedRows = new List<IEditorConfigurationItemViewModel>() {addedRow};


            sharedResourcesGlobalViewModel.AddAsSharedResourceWithContainer(addedRow, null, false);

            configurationEditorViewModel.CopyElementCommand.Execute(null);

            configurationEditorViewModel.SelectedRow = rootGroup;

            Assert.True(configurationEditorViewModel.PasteAsChildElementCommand.CanExecute(null));

            configurationEditorViewModel.PasteAsChildElementCommand.Execute(null);

            var copiedRow = rootGroup.ChildStructItemViewModels[1];

            
            configurationEditorViewModel.SelectedRow =  (IEditorConfigurationItemViewModel)copiedRow ;


            Assert.True(
                (configurationEditorViewModel.AddSelectedElementAsResourceCommand as RelayCommand).CanExecute(null));

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 1);

            var itemList = (result.RootConfigurationItemList[0] as DefaultItemsGroup).ConfigurationItemList;

            CheckPropertyResultProperty(itemList, 1);
            CheckPropertyResultProperty(itemList, 1, 1);

            Assert.AreEqual(itemList.Count, 2);
        }


        [Test]
        public void EditorCopyManyProps()
        {

            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;


            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };
            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var addedRow1 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);
            var addedRow2 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 2, _typesContainer);
            var addedRow3 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 3, _typesContainer);


            configurationEditorViewModel.SelectedRows = new List<IEditorConfigurationItemViewModel>() { addedRow1 , addedRow2 , addedRow3 };


            configurationEditorViewModel.CopyElementCommand.Execute(null);

            configurationEditorViewModel.SelectedRow = rootGroup;

            configurationEditorViewModel.PasteAsChildElementCommand.Execute(null);


            var copiedRow = rootGroup.ChildStructItemViewModels[1];

            Assert.True(rootGroup.ChildStructItemViewModels.Count==6);
        }

        [Test]
        public void CopyPasteDictionaryFormatter()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;


            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };
            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var addedRow = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 3, _typesContainer);


            configurationEditorViewModel.SelectedRows = new List<IEditorConfigurationItemViewModel>() {addedRow};


            configurationEditorViewModel.CopyElementCommand.Execute(null);

            configurationEditorViewModel.SelectedRow = rootGroup;

            configurationEditorViewModel.PasteAsChildElementCommand.Execute(null);


            var copiedRow = rootGroup.ChildStructItemViewModels[1];


            (addedRow.FormatterParametersViewModel.RelatedUshortsFormatterViewModel as
                DictionaryMatchingFormatterViewModel).KeyValuesDictionary[1].Value = "jopa11";

            Assert.True(((copiedRow as IPropertyEditorViewModel).FormatterParametersViewModel
                .RelatedUshortsFormatterViewModel as
                DictionaryMatchingFormatterViewModel).KeyValuesDictionary[1].Value == "jopa1");

        }
    }
}
