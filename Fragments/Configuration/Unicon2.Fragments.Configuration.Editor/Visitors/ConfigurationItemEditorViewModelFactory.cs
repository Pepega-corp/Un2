using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Visitors
{
    public static class ConfigurationItemEditorViewModelFactoryExtension
    {
        public static ConfigurationItemEditorViewModelFactory WithParent(
            this ConfigurationItemEditorViewModelFactory factory, IEditorConfigurationItemViewModel parent)
        {
			var newFactory = factory.Clone() as ConfigurationItemEditorViewModelFactory;
			newFactory.Parent = parent;
			return newFactory;
		}
    }

    public class ConfigurationItemEditorViewModelFactory : IConfigurationItemEditorViewModelFactory, ICloneable
    {
        private readonly ITypesContainer _container;
        internal IEditorConfigurationItemViewModel Parent { get; set; }
        private ConfigurationItemEditorViewModelFactory()
        {
            _container = StaticContainer.Container;
        }

        public static ConfigurationItemEditorViewModelFactory Create()
        {
            return new ConfigurationItemEditorViewModelFactory();
        }
    
        private void InitializeBaseProperties(IConfigurationItemViewModel configurationViewModel, IConfigurationItem configurationItem)
        {
            configurationViewModel.Description = configurationItem?.Description;
            configurationViewModel.Header = configurationItem?.Name;
            if (Parent != null)
            {
                configurationViewModel.Parent = Parent;
                configurationViewModel.Level = Parent.Level + 1;
            }
        }

        private void InitializeProperty(IPropertyEditorViewModel editorPropertyViewModel, IProperty property)
        {
            editorPropertyViewModel.IsMeasureUnitEnabled = property.IsMeasureUnitEnabled;
            editorPropertyViewModel.MeasureUnit = property.MeasureUnit;
            editorPropertyViewModel.IsRangeEnabled = property.IsRangeEnabled;
            if (property.IsRangeEnabled)
            {
                IRangeViewModel rangeViewModel = _container.Resolve<IRangeViewModel>();
                rangeViewModel.RangeFrom = property.Range.RangeFrom.ToString();
                rangeViewModel.RangeTo = property.Range.RangeTo.ToString();
                editorPropertyViewModel.RangeViewModel = rangeViewModel;
            }

            editorPropertyViewModel.Address = property.Address.ToString();
            editorPropertyViewModel.NumberOfPoints = property.NumberOfPoints.ToString();
            var formatterParametersViewModel = StaticContainer.Container.Resolve<IFormatterViewModelFactory>()
                .CreateFormatterViewModel(property.UshortsFormatter);
            editorPropertyViewModel.FormatterParametersViewModel = formatterParametersViewModel;

            editorPropertyViewModel.NumberOfWriteFunction = property.NumberOfWriteFunction;


            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel =
                StaticContainer.Container.Resolve<ISharedResourcesGlobalViewModel>();
            if (sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesWithContainersContainsModel(property))
            {
                sharedResourcesGlobalViewModel.AddExistingResourceWithContainer(editorPropertyViewModel, property);
            }

            if (property.Dependencies != null && property.Dependencies.Count > 0)
            {
                editorPropertyViewModel.DependencyViewModels.Clear();
                editorPropertyViewModel.DependencyViewModels.AddCollection(property.Dependencies
                    .Select(_container.Resolve<DependencyFillHelper>().CreateDependencyViewModel).ToList());
            }

            editorPropertyViewModel.IsFromBits = property.IsFromBits;
            if (editorPropertyViewModel.BitNumbersInWord != null)
            {
                property.BitNumbers.ForEach(bitNum =>
                    editorPropertyViewModel.BitNumbersInWord.First(model => model.BitNumber == bitNum).IsChecked =
                        true);
            }

            if (editorPropertyViewModel is ICanBeHidden canBeHidden)
            {
                canBeHidden.IsHidden = property.IsHidden;
            }

            InitializeBaseProperties(editorPropertyViewModel, property);
        }

        public IEditorConfigurationItemViewModel VisitItemsGroup(IItemsGroup itemsGroup)
        {
            var res = _container.Resolve<IConfigurationGroupEditorViewModel>();
            if (itemsGroup != null)
            {
                res.ChildStructItemViewModels.Clear();
                foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
                {
                    res.IsCheckable = true;
                    res.ChildStructItemViewModels.Add(configurationItem.Accept(this.WithParent(res)));
                }

                if (itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo)
                {
	                res.SetIsGroupWithReiteration(true);
	                res.ReiterationStep = groupWithReiterationInfo.ReiterationStep;
	                groupWithReiterationInfo.SubGroups.ForEach(info =>
		                res.SubGroupNames.Add(new StringWrapper(info.Name)));
                }

                if (itemsGroup.GroupFilter != null)
                {
                    var filterFillHelper = _container.Resolve<FilterFillHelper>();
                    res.FilterViewModels.AddCollection(itemsGroup.GroupFilter.Filters
                        .Select(filter => filterFillHelper.CreateFilterViewModel(filter)));
                }

                res.IsMain = itemsGroup.IsMain ?? false;
                res.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
                
            }
            InitializeBaseProperties(res, itemsGroup);
            return res;
        }



        public IEditorConfigurationItemViewModel VisitProperty(IProperty property)
        {
            var res = _container.Resolve<IPropertyEditorViewModel>();
            if (property != null)
            {
                InitializeProperty(res, property);
            }
            else
            {
                InitializeBaseProperties(res, property);
            }
            return res;
        }

        public IEditorConfigurationItemViewModel VisitComplexProperty(IComplexProperty property)
        {
            var res = _container.Resolve<IComplexPropertyEditorViewModel>();
            res.ChildStructItemViewModels.Clear();
            res.NumberOfPoints = "1";
            if (property == null)
            {
				InitializeBaseProperties(res,property);
	            return res;
            }

            res.IsGroupedProperty = property.IsGroupedProperty;
            foreach (ISubProperty subProperty in property.SubProperties)
            {
	            var subPropertyViewModel = subProperty.Accept(this.WithParent(res));
	            (subPropertyViewModel as ISubPropertyEditorViewModel).BitNumbersInWord =
		            res.MainBitNumbersInWordCollection;

	            foreach (var bitNumber in subProperty.BitNumbersInWord)
	            {
		            var sharedBit = (subPropertyViewModel as ISubPropertyEditorViewModel).BitNumbersInWord
			            .First((viewModel => viewModel.NumberOfBit == bitNumber));
		            sharedBit.Refresh();
		            sharedBit.ChangeValueByOwnerCommand
			            ?.Execute(subPropertyViewModel);
	            }
				res.SubPropertyEditorViewModels.Add(subPropertyViewModel as ISubPropertyEditorViewModel);
				res.ChildStructItemViewModels.Add(subPropertyViewModel as ISubPropertyEditorViewModel);
				res.IsCheckable = true;
            }
			InitializeProperty(res, property);
            return res;
        }

        public IEditorConfigurationItemViewModel VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IEditorConfigurationItemViewModel VisitSubProperty(ISubProperty subProperty)
        {
	        var res = _container.Resolve<ISubPropertyEditorViewModel>();
	        if (subProperty == null)
	        {
		        InitializeBaseProperties(res, subProperty);
				return res;
			} 
	        InitializeProperty(res,subProperty);
			return res;
			//res.BitNumbersInWord = subProperty.BitNumbersInWord
		}

        public object Clone()
        {
	        return new ConfigurationItemEditorViewModelFactory();

        }
    }
}
