using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Factories
{
    public class ConfigurationItemEditorViewModelFactory : IConfigurationItemEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public ConfigurationItemEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IEditorConfigurationItemViewModel ResolveConfigurationItemEditorViewModel(IConfigurationItem configurationItem, IEditorConfigurationItemViewModel parent = null)
        {

            IEditorConfigurationItemViewModel configurationItemViewModel =
                this._container.Resolve<IViewModel>(configurationItem.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IEditorConfigurationItemViewModel;
            configurationItemViewModel.Model = configurationItem;
            if (parent != null)
            {
                configurationItemViewModel.Parent = parent;
                configurationItemViewModel.Level = parent.Level + 1;
            }

            return configurationItemViewModel;
        }

        public IEditorConfigurationItemViewModel ResolveSubPropertyEditorViewModel(IConfigurationItem configurationItem,
            ObservableCollection<ISharedBitViewModel> mainBitViewModels, IEditorConfigurationItemViewModel parent = null)
        {

            IEditorConfigurationItemViewModel configurationItemViewModel =
                this._container.Resolve<IViewModel>(configurationItem.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IEditorConfigurationItemViewModel;
            (configurationItemViewModel as ISubPropertyEditorViewModel).BitNumbersInWord = mainBitViewModels;
            configurationItemViewModel.Model = configurationItem;
            if (parent != null)
            {
                configurationItemViewModel.Parent = parent;
                configurationItemViewModel.Level = parent.Level + 1;
            }

            return configurationItemViewModel;
        }
        private void InitializeBaseProperties(IConfigurationItemViewModel configurationViewModel, IConfigurationItem configurationItem)
        {
            configurationViewModel.Description = configurationItem.Description;
            configurationViewModel.Header = configurationItem.Name;
        }
        private void InitializeProperty(IRuntimePropertyViewModel runtimePropertyViewModel, IProperty property)
        {
            runtimePropertyViewModel.IsMeasureUnitEnabled = property.IsMeasureUnitEnabled;
            runtimePropertyViewModel.MeasureUnit = property.MeasureUnit;
            runtimePropertyViewModel.RangeViewModel = this._container.Resolve<IRangeViewModel>();
            runtimePropertyViewModel.IsRangeEnabled = property.IsRangeEnabled;
            runtimePropertyViewModel.RangeViewModel.Model = property.Range;
            InitializeBaseProperties(runtimePropertyViewModel, property);
        }
        public IEditorConfigurationItemViewModel VisitItemsGroup(IItemsGroup itemsGroup)
        {
            var res = _container.Resolve<IConfigurationGroupEditorViewModel>();
            res.ChildStructItemViewModels.Clear();
            foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
            {
                res.ChildStructItemViewModels.Add(configurationItem.Accept(this));
            }
            res.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
            InitializeBaseProperties(res, itemsGroup);
            return res;
        }

    

        public IEditorConfigurationItemViewModel VisitProperty(IProperty property)
        {
            container.Register < IPropertyEditorEditorViewModel, 
            container.Register < IDependentPropertyEditorViewModel,
            container.Register < IComplexPropertyEditorViewModel, ;
            container.Register < ISubPropertyEditorViewModel


        }

        public IEditorConfigurationItemViewModel VisitComplexProperty(IComplexProperty property)
        {
            throw new System.NotImplementedException();
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
