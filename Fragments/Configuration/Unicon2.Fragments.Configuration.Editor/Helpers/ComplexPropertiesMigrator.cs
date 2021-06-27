using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class ComplexPropertiesMigrator
    {

        public static List<IConfigurationItemViewModel> GetAllComplexPropertiesInConfiguration(List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            var res=new List<IConfigurationItemViewModel>();
            FillComplexPropertiesList(rootConfigurationItemViewModels,res);
            return res;
        }

        private static void FillComplexPropertiesList(
            IEnumerable<IConfigurationItemViewModel> configurationItemViewModels,
            List<IConfigurationItemViewModel> accumulator)
        {
            foreach (var configurationItemViewModel in configurationItemViewModels)
            {
                if (configurationItemViewModel is IComplexPropertyEditorViewModel complexPropertyEditorViewModel)
                {
                    accumulator.Add(complexPropertyEditorViewModel);
                }

                if (configurationItemViewModel is IItemGroupViewModel groupViewModel)
                {
                    FillComplexPropertiesList(groupViewModel.ChildStructItemViewModels,accumulator);
                }
            }
        }


        public static Result MigrateComplexProperties(List<IConfigurationItemViewModel> preparedData)
        {
            try
            {
                var factory=ConfigurationItemEditorViewModelFactory.Create();
                foreach (var preparedItem in preparedData)
                {
                    if (preparedItem is IComplexPropertyEditorViewModel complexPropertyEditorViewModel)
                    {
                        if (complexPropertyEditorViewModel.IsGroupedProperty)
                        {
                           var group= factory.WithParent(
                                complexPropertyEditorViewModel.Parent as IEditorConfigurationItemViewModel).VisitItemsGroup(null);

                           var propList =
                               ConvertComlexPropertyIntoDefaultProperties(complexPropertyEditorViewModel, group);
                        }
                        else
                        {
                            var propList = ConvertComlexPropertyIntoDefaultProperties(complexPropertyEditorViewModel, complexPropertyEditorViewModel.Parent as IEditorConfigurationItemViewModel);
                        }
                    }
                }

                return Result.Create(true);
            }
            catch (Exception e)
            {
                return Result.Create(e);

            }
        }

        private static List<IPropertyEditorViewModel> ConvertComlexPropertyIntoDefaultProperties(IComplexPropertyEditorViewModel complexPropertyEditorViewModel, IEditorConfigurationItemViewModel parent)
        {
            List < IPropertyEditorViewModel > res=new List<IPropertyEditorViewModel>();
            foreach (var subPropertyEditorViewModel in complexPropertyEditorViewModel.SubPropertyEditorViewModels)
            {
                var factory = ConfigurationItemEditorViewModelFactory.Create();
             //   subPropertyEditorViewModel.
            }

            return res;
        }

    }
}
