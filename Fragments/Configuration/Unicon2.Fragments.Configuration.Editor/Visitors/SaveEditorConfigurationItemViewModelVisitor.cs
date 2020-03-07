using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Interfaces;
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
                range.RangeTo = double.Parse(editorViewModel.RangeViewModel.RangeTo);
                range.RangeFrom = double.Parse(editorViewModel.RangeViewModel.RangeFrom);
                property.Range = range;
            }

            property.UshortsFormatter = _container.Resolve<ISaveFormatterService>()
                .CreateUshortsFormatter(editorViewModel.RelatedUshortsFormatterViewModel);
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

            group.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
            group.IsMain = itemsGroup.IsMain;
            return InitDefaults(group, itemsGroup);
        }

        public IConfigurationItem VisitProperty(IPropertyEditorViewModel propertyViewModel)
        {
            var property = _container.Resolve<IProperty>();
            property.Address = ushort.Parse(propertyViewModel.Address??"0");
            property.NumberOfPoints = ushort.Parse(propertyViewModel.NumberOfPoints??"0");
            return InitializeProperty(propertyViewModel, property);
        }

        public IConfigurationItem VisitComplexProperty(IComplexPropertyEditorViewModel property)
        {
            throw new NotImplementedException();
        }

        public IConfigurationItem VisitMatrix(IEditorConfigurationItemViewModel appointableMatrixViewModel)
        {
            throw new NotImplementedException();
        }

        public IConfigurationItem VisitDependentProperty(IDependentPropertyEditorViewModel dependentPropertyViewModel)
        {
            throw new NotImplementedException();
        }

        public IConfigurationItem VisitSubProperty(IEditorConfigurationItemViewModel dependentPropertyViewModel)
        {
            throw new NotImplementedException();
        }
    }
}