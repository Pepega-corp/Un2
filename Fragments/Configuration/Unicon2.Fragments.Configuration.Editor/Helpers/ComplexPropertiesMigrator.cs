using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class ComplexPropertiesMigrator
    {

        public static List<IConfigurationItemViewModel> GetAllComplexPropertiesInConfiguration(
            List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            var res = new List<IConfigurationItemViewModel>();
            FillComplexPropertiesList(rootConfigurationItemViewModels, res);
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
                    FillComplexPropertiesList(groupViewModel.ChildStructItemViewModels, accumulator);
                }
            }
        }


        public static Result MigrateComplexProperties(List<IConfigurationItemViewModel> preparedData)
        {
            var sharedResourcesService = StaticContainer.Container.Resolve<ISharedResourcesGlobalViewModel>();

            try
            {
                var factory = ConfigurationItemEditorViewModelFactory.Create();
                foreach (var preparedItem in preparedData)
                {
                    if (preparedItem is IComplexPropertyEditorViewModel complexPropertyEditorViewModel)
                    {
                        if (complexPropertyEditorViewModel.IsGroupedProperty)
                        {
                            var group = factory.WithParent(
                                    complexPropertyEditorViewModel.Parent as IEditorConfigurationItemViewModel)
                                .VisitItemsGroup(null);
                            (group as IItemGroupViewModel).IsMain =
                                (complexPropertyEditorViewModel.Parent as IItemGroupViewModel).IsMain;
                            (group as IItemGroupViewModel).IsTableViewAllowed =
                                (complexPropertyEditorViewModel.Parent as IItemGroupViewModel).IsTableViewAllowed;

                            var propList =
                                ConvertComlexPropertyIntoDefaultProperties(complexPropertyEditorViewModel, group);

                            propList.resources.ForEach(tuple =>
                            {
                                sharedResourcesService.AddAsSharedResourceWithContainer(tuple.resource,
                                    tuple.resourceName, false);
                            });
                            complexPropertyEditorViewModel.Parent.ChildStructItemViewModels.Add(group);
                            propList.convertedProperties.ForEach(model => { group.ChildStructItemViewModels.Add(model); });
                            group.IsCheckable = propList.convertedProperties.Any();
                            group.Name = complexPropertyEditorViewModel.Name; 
                            group.Header = complexPropertyEditorViewModel.Header;
                            group.Description = complexPropertyEditorViewModel.Description;

                        }
                        else
                        {
                            var propList = ConvertComlexPropertyIntoDefaultProperties(complexPropertyEditorViewModel,
                                complexPropertyEditorViewModel.Parent as IEditorConfigurationItemViewModel);

                            propList.resources.ForEach(tuple =>
                            {
                                sharedResourcesService.AddAsSharedResourceWithContainer(tuple.resource,
                                    tuple.resourceName, false);
                            });
                            propList.convertedProperties.ForEach(model => { complexPropertyEditorViewModel.Parent.ChildStructItemViewModels.Add(model); });
                        }
                        complexPropertyEditorViewModel.Parent.Checked?.Invoke(false);
                        complexPropertyEditorViewModel.DeleteElement();
                    }
                }

                return Result.Create(true);
            }
            catch (Exception e)
            {
                return Result.Create(e);

            }
        }

        private static (List<IPropertyEditorViewModel> convertedProperties,
            List<(string resourceName, IPropertyEditorViewModel resource)> resources)
            ConvertComlexPropertyIntoDefaultProperties(IComplexPropertyEditorViewModel complexPropertyEditorViewModel,
                IEditorConfigurationItemViewModel parent)
        {
            List<IPropertyEditorViewModel> res = new List<IPropertyEditorViewModel>();
            List<(string resourceName, IPropertyEditorViewModel resource)> resourcesResult =
                new List<(string resourceName, IPropertyEditorViewModel resource)>();
            foreach (var subPropertyEditorViewModel in complexPropertyEditorViewModel.SubPropertyEditorViewModels)
            {
                var factory = ConfigurationItemEditorViewModelFactory.Create().WithParent(parent);
                var bits = subPropertyEditorViewModel.BitNumbersInWord.Where(model =>
                    model.Value && model.Owner == subPropertyEditorViewModel).ToList();
               
                var property = factory.VisitProperty(null) as IPropertyEditorViewModel;
                property.Name = subPropertyEditorViewModel.Name;
                property.Header = subPropertyEditorViewModel.Header;
                property.Address = complexPropertyEditorViewModel.Address;
                property.Description = subPropertyEditorViewModel.Description;
                property.NumberOfWriteFunction = complexPropertyEditorViewModel.NumberOfWriteFunction;
                property.NumberOfPoints = complexPropertyEditorViewModel.NumberOfPoints;
                property.IsMeasureUnitEnabled = subPropertyEditorViewModel.IsMeasureUnitEnabled;
                property.MeasureUnit = subPropertyEditorViewModel.MeasureUnit;
                property.IsRangeEnabled = subPropertyEditorViewModel.IsRangeEnabled;
                property.DependencyViewModels.AddCollection(subPropertyEditorViewModel.DependencyViewModels
                    .Select(model => model.Clone()));
                property.RangeViewModel = subPropertyEditorViewModel.RangeViewModel.Clone() as IRangeViewModel;

                property.IsFromBits = true;
                property.FormatterParametersViewModel = subPropertyEditorViewModel.FormatterParametersViewModel.Clone();
                foreach (var bit in bits)
                {
                    property.BitNumbersInWord.First(model => model.BitNumber == bit.NumberOfBit).IsChecked =
                        true;
                }

                var sharedResourcesService = StaticContainer.Container.Resolve<ISharedResourcesGlobalViewModel>();
                
                var resourceInfo = sharedResourcesService.GetNameByResourceViewModel(subPropertyEditorViewModel);
                if (resourceInfo.IsSuccess)
                {
                    resourcesResult.Add((resourceInfo.Item, property));
                    sharedResourcesService.RemoveResourceByViewModel(subPropertyEditorViewModel);
                }

                
                res.Add(property);
            }

            return (res, resourcesResult);
        }

    }
}
