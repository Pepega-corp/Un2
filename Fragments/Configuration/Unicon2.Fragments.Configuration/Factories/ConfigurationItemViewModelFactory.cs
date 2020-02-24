using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Factories
{
    public class RuntimeConfigurationItemViewModelFactory : IRuntimeConfigurationItemViewModelFactory
    {
        private readonly ITypesContainer _container;
        private IMemoryBusDispatcher _memoryBusDispatcher;
        private readonly IConfigurationMemory _configurationMemory;

        public RuntimeConfigurationItemViewModelFactory(ITypesContainer container, IMemoryBusDispatcher memoryBusDispatcher, IConfigurationMemory configurationMemory)
        {
            this._container = container;
            _memoryBusDispatcher = memoryBusDispatcher;
            _configurationMemory = configurationMemory;
        }

        private void InitializeBaseProperties(IConfigurationItemViewModel configurationViewModel,
            IConfigurationItem configurationItem)
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

        public IRuntimeConfigurationItemViewModel VisitItemsGroup(IItemsGroup itemsGroup)
        {
            var res = _container.Resolve<IRuntimeItemGroupViewModel>();
            res.ChildStructItemViewModels.Clear();
            foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
            {
                res.ChildStructItemViewModels.Add(configurationItem.Accept(this));
            }

            res.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
            InitializeBaseProperties(res, itemsGroup);
            return res;
        }

        public IRuntimeConfigurationItemViewModel VisitProperty(IProperty property)
        {
            var res = _container.Resolve<IRuntimePropertyViewModel>();
            InitializeProperty(res, property);
            _memoryBusDispatcher.AddDeviceDataSubscription(property.Address, property.NumberOfPoints,
                new DeviceDataPropertyMemorySubscription(property, res, _container.Resolve<IValueViewModelFactory>(),
                    _configurationMemory));
            return res;
        }

        public IRuntimeConfigurationItemViewModel VisitComplexProperty(IComplexProperty complexProperty)
        {
            var res = _container.Resolve<IRuntimeComplexPropertyViewModel>();
            foreach (ISubProperty subProperty in complexProperty.SubProperties)
            {
                IRuntimeConfigurationItemViewModel subPropertyViewModel = subProperty.Accept(this);
                res.ChildStructItemViewModels.Add(subPropertyViewModel);
                res.IsCheckable = true;
            }

            res.IsGroupedProperty = complexProperty.IsGroupedProperty;
            InitializeProperty(res, complexProperty);
            return res;
        }

        public IRuntimeConfigurationItemViewModel VisitMatrix(IAppointableMatrix appointableMatrix)
        {
            var res = _container.Resolve<IRuntimeAppointableMatrixViewModel>();
            InitializeProperty(res, appointableMatrix);
            return res;
        }

        public IRuntimeConfigurationItemViewModel VisitDependentProperty(IDependentProperty dependentProperty)
        {
            var res = _container.Resolve<IRuntimeDependentPropertyViewModel>();
            InitializeProperty(res, dependentProperty);
            return res;
        }

        public IRuntimeConfigurationItemViewModel VisitSubProperty(ISubProperty subProperty)
        {
            var res = _container.Resolve<IRuntimeSubPropertyViewModel>();
            InitializeProperty(res, subProperty);
            return res;
        }

        public void Initialize(IMemoryBusDispatcher memoryBusDispatcher)
        {
            _memoryBusDispatcher = memoryBusDispatcher;
        }
    }
}