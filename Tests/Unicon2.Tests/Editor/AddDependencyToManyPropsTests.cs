using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Common;
using Unity;
using static Unicon2.Tests.Utils.EditorHelpers;

namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class AddDependencyToManyPropsTests
    {
        private TypesContainer _typesContainer;

        public AddDependencyToManyPropsTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        [Test]
        public async Task ShouldAllowAddManyDependencies()
        {
            var configurationEditorViewModel = _typesContainer.Resolve<IFragmentEditorViewModel>(
                ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as ConfigurationEditorViewModel;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                Name = "root"
            };

            var prop1=AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 1, _typesContainer);
            var prop2 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 2, _typesContainer);
            var prop3 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 3, _typesContainer);
            var prop4 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 4, _typesContainer);
            var prop5 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 5, _typesContainer);
            var prop6 = AddPropertyViewModel(rootGroup.ChildStructItemViewModels, 6, _typesContainer);

            configurationEditorViewModel.RootConfigurationItemViewModels.Add(rootGroup);

            var result = ConfigurationFragmentFactory.CreateConfiguration(configurationEditorViewModel);
            Assert.AreEqual(result.RootConfigurationItemList.Count, 1);

            var itemList = (result.RootConfigurationItemList[0] as DefaultItemsGroup).ConfigurationItemList;


            var selected = new ObservableCollection<object>();
            selected.Add(prop1);
            selected.Add(prop2);
            selected.Add(prop5);

            configurationEditorViewModel.OnSelectionChangedCommand.Execute(selected);

            Assert.True(configurationEditorViewModel.IsMultiEditMode);
            Assert.True(configurationEditorViewModel.AddDependencyToManyProps.CanExecute(null));


        }


    }
}