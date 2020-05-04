using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
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
    
        //public IEditorConfigurationItemViewModel ResolveConfigurationItemEditorViewModel(IConfigurationItem configurationItem, IEditorConfigurationItemViewModel parent = null)
        //{

        //    IEditorConfigurationItemViewModel configurationItemViewModel =
        //        this._container.Resolve<IViewModel>(configurationItem.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IEditorConfigurationItemViewModel;
        //    configurationItemViewModel.Model = configurationItem;
        //    if (parent != null)
        //    {
        //        configurationItemViewModel.Parent = parent;
        //        configurationItemViewModel.Level = parent.Level + 1;
        //    }

        //    return configurationItemViewModel;
        //}

        //public IEditorConfigurationItemViewModel ResolveSubPropertyEditorViewModel(IConfigurationItem configurationItem,
        //    ObservableCollection<ISharedBitViewModel> mainBitViewModels, IEditorConfigurationItemViewModel parent = null)
        //{

        //    IEditorConfigurationItemViewModel configurationItemViewModel =
        //        this._container.Resolve<IViewModel>(configurationItem.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IEditorConfigurationItemViewModel;
        //    (configurationItemViewModel as ISubPropertyEditorViewModel).BitNumbersInWord = mainBitViewModels;
        //    configurationItemViewModel.Model = configurationItem;
        //    if (parent != null)
        //    {
        //        configurationItemViewModel.Parent = parent;
        //        configurationItemViewModel.Level = parent.Level + 1;
        //    }

        //    return configurationItemViewModel;
        //}
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

            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel =
	            StaticContainer.Container.Resolve<ISharedResourcesGlobalViewModel>();
            if (sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesWithContainersContainsModel(property))
            {
	            sharedResourcesGlobalViewModel.AddExistingResourceWithContainer(editorPropertyViewModel, property);

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

                res.IsMain = itemsGroup.IsMain ?? false;
                res.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
                InitializeBaseProperties(res, itemsGroup);
            }

            return res;
        }



        public IEditorConfigurationItemViewModel VisitProperty(IProperty property)
        {
            var res = _container.Resolve<IPropertyEditorViewModel>();
            if (property != null) InitializeProperty(res, property);
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

        public IEditorConfigurationItemViewModel VisitDependentProperty(IDependentProperty dependentProperty)
        {
	        var res = _container.Resolve<IDependentPropertyEditorViewModel>();
	        if (dependentProperty == null)
	        {
		        InitializeBaseProperties(res, dependentProperty);
		        return res;
	        }

	        InitializeProperty(res, dependentProperty);


	        foreach (IDependancyCondition condition in dependentProperty.DependancyConditions)
	        {
		        IConditionViewModel conditionViewModel = _container.Resolve<IConditionViewModel>();
		        conditionViewModel.SelectedCondition = condition.ConditionsEnum.ToString();
		        conditionViewModel.SelectedConditionResult = condition.ConditionResult.ToString();
		        conditionViewModel.UshortValueToCompare = condition.UshortValueToCompare;
		        conditionViewModel.ReferencedResourcePropertyName = condition.ReferencedPropertyResourceName;
				res.ConditionViewModels.Add(conditionViewModel);
	        }
			InitializeProperty(res,dependentProperty);
	        return res;

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
