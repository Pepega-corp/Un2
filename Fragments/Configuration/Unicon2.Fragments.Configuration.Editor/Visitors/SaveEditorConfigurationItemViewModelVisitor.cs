using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Visitors
{
    public class SaveEditorConfigurationItemViewModelVisitor : IConfigurationItemViewModelVisitor<IConfigurationItem>
    {
        private readonly ITypesContainer _container;

        public SaveEditorConfigurationItemViewModelVisitor(ITypesContainer container)
        {
            _container = container;
        }

        private IConfigurationItem InitializeProperty(IPropertyEditorViewModel editorViewModel, IProperty property)
        {
            property.IsMeasureUnitEnabled = editorViewModel.IsMeasureUnitEnabled;
            property.MeasureUnit = editorViewModel.MeasureUnit;
            if (editorViewModel.IsRangeEnabled)
            {
                IRange range = _container.Resolve<IRange>();
                if (editorViewModel.RangeViewModel.RangeTo != null)
	                range.RangeTo = double.Parse(editorViewModel.RangeViewModel.RangeTo);
                if (editorViewModel.RangeViewModel.RangeFrom != null)
	                range.RangeFrom = double.Parse(editorViewModel.RangeViewModel.RangeFrom);
                property.Range = range;
                property.IsRangeEnabled = editorViewModel.IsRangeEnabled;
            }

            if (editorViewModel.FormatterParametersViewModel != null)
            {
                property.UshortsFormatter=StaticContainer.Container.Resolve<ISaveFormatterService>()
                    .CreateUshortsParametersFormatter(editorViewModel.FormatterParametersViewModel);
            }

            var sharedResourcesGlobalViewModel= _container.Resolve<ISharedResourcesGlobalViewModel>();
            if (sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesContainsViewModel(editorViewModel))
            {
	            sharedResourcesGlobalViewModel.AddResourceFromViewModel(editorViewModel, property);
            }
            
            property.NumberOfWriteFunction = editorViewModel.NumberOfWriteFunction;

            if (editorViewModel.DependencyViewModels != null)
            {
	            List<IDependency> dependencies=new List<IDependency>();
	            dependencies = editorViewModel.DependencyViewModels
		            .Select(_container.Resolve<DependencyFillHelper>().CreateDependencyModel).ToList();
	            property.Dependencies = dependencies;
            }
            return InitDefaults(property, editorViewModel);
        }

        private IConfigurationItem InitDefaults(IConfigurationItem configurationItem,
            IEditorConfigurationItemViewModel editorConfigurationItemViewModel)
        {
            configurationItem.Description = editorConfigurationItemViewModel.Description;
            configurationItem.Name = editorConfigurationItemViewModel.Header;
            return configurationItem;
        }

        public IConfigurationItem VisitItemsGroup(IConfigurationGroupEditorViewModel itemsGroup)
        {
            var group = _container.Resolve<IItemsGroup>();
            foreach (var childStructItemViewModel in itemsGroup.ChildStructItemViewModels)
            {
                if (childStructItemViewModel is IEditorConfigurationItemViewModel editorConfigurationItemViewModel)
                {
                    group.ConfigurationItemList.Add(editorConfigurationItemViewModel.Accept(this));
                }
            }

            IConfigurationItemFactory factory = _container.Resolve<IConfigurationItemFactory>();

			if (itemsGroup.IsGroupWithReiteration)
            {
	            var groupWithReiterationInfo= factory.ResolveGroupWithReiterationInfo();
	            groupWithReiterationInfo.IsReiterationEnabled = true;
	            groupWithReiterationInfo.ReiterationStep = itemsGroup.ReiterationStep;
				itemsGroup.SubGroupNames.ForEach(wrapper =>
				{
					var subGroupInfo = factory.ResolveReiterationSubGroupInfo();
					subGroupInfo.Name = wrapper.StringValue;
					groupWithReiterationInfo.SubGroups.Add(subGroupInfo);
				} );
				group.GroupInfo = groupWithReiterationInfo;
            }
            group.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
            group.IsMain = itemsGroup.IsMain;
            return InitDefaults(group, itemsGroup);
        }

        public IConfigurationItem VisitProperty(IPropertyEditorViewModel propertyViewModel)
        {
            var property = _container.Resolve<IProperty>();
            property.Address = ushort.Parse(propertyViewModel.Address ?? "0");
            property.NumberOfPoints = ushort.Parse(propertyViewModel.NumberOfPoints ?? "0");
            return InitializeProperty(propertyViewModel, property);
        }

        public IConfigurationItem VisitComplexProperty(IComplexPropertyEditorViewModel propertyViewModel)
        {
			var complexProperty = _container.Resolve<IComplexProperty>();
			complexProperty.Address = ushort.Parse(propertyViewModel.Address ?? "0");
			complexProperty.NumberOfPoints = ushort.Parse(propertyViewModel.NumberOfPoints ?? "0");
			foreach (var childStructItemViewModel in propertyViewModel.SubPropertyEditorViewModels)
			{
				if (childStructItemViewModel is ISubPropertyEditorViewModel subPropertyEditorViewModel)
				{
					complexProperty.SubProperties.Add(subPropertyEditorViewModel.Accept(this) as ISubProperty);
				}
			}

			complexProperty.IsGroupedProperty = propertyViewModel.IsGroupedProperty;
			return InitializeProperty(propertyViewModel, complexProperty); 
        }

        public IConfigurationItem VisitMatrix(IEditorConfigurationItemViewModel appointableMatrixViewModel)
        {
            throw new NotImplementedException();
        }


        public IConfigurationItem VisitSubProperty(ISubPropertyEditorViewModel subPropertyEditorViewModel)
        {
	        var subProperty = _container.Resolve<ISubProperty>();
	        subProperty.BitNumbersInWord = subPropertyEditorViewModel.BitNumbersInWord.Where(model => model.Value&&model.Owner==subPropertyEditorViewModel)
		        .Select(model => model.NumberOfBit).ToList();
	        subProperty.Address = ushort.Parse(subPropertyEditorViewModel.Address ?? "0");
	        subProperty.NumberOfPoints = ushort.Parse(subPropertyEditorViewModel.NumberOfPoints ?? "0");
			return InitializeProperty(subPropertyEditorViewModel, subProperty);
		}
    }
}