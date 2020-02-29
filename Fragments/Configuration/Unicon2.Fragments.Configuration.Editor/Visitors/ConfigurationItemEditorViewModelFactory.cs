using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Visitors
{
    public static class ConfigurationItemEditorViewModelFactoryExtension
    {
        public static ConfigurationItemEditorViewModelFactory SetParent(
            this ConfigurationItemEditorViewModelFactory factory, IEditorConfigurationItemViewModel parent)
        {
            return factory;
        }
    }

    public class ConfigurationItemEditorViewModelFactory : IConfigurationItemEditorViewModelFactory
    {
        private readonly ITypesContainer _container;
        internal IEditorConfigurationItemViewModel Parent { get; set; }
        private ConfigurationItemEditorViewModelFactory()
        {
            this._container = StaticContainer.Container;
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
            configurationViewModel.Description = configurationItem.Description;
            configurationViewModel.Header = configurationItem.Name;
            if (Parent != null)
            {
                configurationViewModel.Parent = Parent;
                configurationViewModel.Level = Parent.Level + 1;
            }
        }
        private void InitializeProperty(IPropertyEditorViewModel runtimePropertyViewModel, IProperty property)
        {
            runtimePropertyViewModel.IsMeasureUnitEnabled = property.IsMeasureUnitEnabled;
            runtimePropertyViewModel.MeasureUnit = property.MeasureUnit;
            runtimePropertyViewModel.IsRangeEnabled = property.IsRangeEnabled;
            if (property.IsRangeEnabled)
            {
                IRangeViewModel rangeViewModel = _container.Resolve<IRangeViewModel>();
                rangeViewModel.RangeFrom = property.Range.RangeFrom.ToString();
                rangeViewModel.RangeFrom = property.Range.RangeTo.ToString();

                runtimePropertyViewModel.RangeViewModel = rangeViewModel;
            }

            InitializeBaseProperties(runtimePropertyViewModel, property);
        }
        public IEditorConfigurationItemViewModel VisitItemsGroup(IItemsGroup itemsGroup)
        {
            var res = _container.Resolve<IConfigurationGroupEditorViewModel>();
            if (itemsGroup != null)
            {
                res.ChildStructItemViewModels.Clear();
                foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
                {
                    res.ChildStructItemViewModels.Add(configurationItem.Accept(this.SetParent(res)));
                }

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
            if (property == null) return res;
            foreach (ISubProperty configurationItem in property.SubProperties)
            {
                res.ChildStructItemViewModels.Add(configurationItem.Accept(this));
            }
            InitializeProperty(res, property);

            return res;

            //container.Register < IDependentPropertyEditorViewModel,
            //container.Register < ISubPropertyEditorViewModel
        }

        public IEditorConfigurationItemViewModel VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IEditorConfigurationItemViewModel VisitDependentProperty(IDependentProperty dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IEditorConfigurationItemViewModel VisitSubProperty(ISubProperty dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
