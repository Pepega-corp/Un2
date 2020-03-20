using System;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Factories
{

    public static class RuntimeConfigurationItemViewModelFactoryExtension
    {
        public static RuntimeConfigurationItemViewModelFactory WithParent(
            this RuntimeConfigurationItemViewModelFactory factory, IConfigurationItemViewModel parent)
        {
            var newFactory = factory.Clone() as RuntimeConfigurationItemViewModelFactory;
            newFactory.Parent = parent;
            return newFactory;
        }
    }

    public class RuntimeConfigurationItemViewModelFactory : IRuntimeConfigurationItemViewModelFactory, ICloneable
    {
        private readonly ITypesContainer _container;
        private IDeviceEventsDispatcher _deviceEventsDispatcher;
        private readonly IDeviceMemory _deviceMemory;

        public RuntimeConfigurationItemViewModelFactory(ITypesContainer container, IDeviceMemory deviceMemory,
            IDeviceEventsDispatcher deviceEventsDispatcher)
        {
            _container = container;
            _deviceMemory = deviceMemory;
            _deviceEventsDispatcher = deviceEventsDispatcher;
        }

        public IConfigurationItemViewModel Parent { get; set; }

        private void InitializeBaseProperties(IConfigurationItemViewModel configurationViewModel,
            IConfigurationItem configurationItem)
        {
            configurationViewModel.Description = configurationItem.Description;
            configurationViewModel.Header = configurationItem.Name;
            if (Parent != null)
            {
                configurationViewModel.Parent = Parent;
                configurationViewModel.Level = Parent.Level + 1;
            }
        }

        private void InitializeProperty(IRuntimePropertyViewModel runtimePropertyViewModel, IProperty property)
        {
            runtimePropertyViewModel.IsMeasureUnitEnabled = property.IsMeasureUnitEnabled;
            runtimePropertyViewModel.MeasureUnit = property.MeasureUnit;
            runtimePropertyViewModel.RangeViewModel = _container.Resolve<IRangeViewModel>();
            runtimePropertyViewModel.IsRangeEnabled = property.IsRangeEnabled;
            if (property.IsRangeEnabled)
            {
                runtimePropertyViewModel.RangeViewModel.RangeFrom = property.Range.RangeFrom.ToString();
                runtimePropertyViewModel.RangeViewModel.RangeTo = property.Range.RangeTo.ToString();

            }

            InitializeBaseProperties(runtimePropertyViewModel, property);
        }

        public IRuntimeConfigurationItemViewModel VisitItemsGroup(IItemsGroup itemsGroup)
        {
            var res = _container.Resolve<IRuntimeItemGroupViewModel>();
            res.ChildStructItemViewModels.Clear();
            foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
            {
                res.ChildStructItemViewModels.Add(configurationItem.Accept(this.WithParent(res)));
            }

            res.IsMain = itemsGroup.IsMain ?? false;
            res.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;
            InitializeBaseProperties(res, itemsGroup);
            return res;
        }

        public IRuntimeConfigurationItemViewModel VisitProperty(IProperty property)
        {
            var res = _container.Resolve<IRuntimePropertyViewModel>();
            InitializeProperty(res, property);

            _deviceEventsDispatcher.AddDeviceAddressSubscription(property.Address, property.NumberOfPoints,
                new DeviceDataPropertyMemorySubscription(property, res, _container.Resolve<IValueViewModelFactory>(),
                    _deviceMemory));

            var localValue = _container.Resolve<IFormattingService>().FormatValue(property.UshortsFormatter,
                InitDefaultUshortsValue(property.NumberOfPoints));
            var editableValue = _container.Resolve<IValueViewModelFactory>()
                .CreateEditableValueViewModel(new FormattedValueInfo(localValue, property, property.UshortsFormatter,
                    property));
            var setUnchangedSuscription = new EditableValueSetUnchangedSubscription(editableValue, _deviceMemory,
                property.Address, property.NumberOfPoints);

            var editSubscription =
                new LocalDataEditedSubscription(editableValue, _deviceMemory, property, setUnchangedSuscription);

            res.LocalValue = editableValue;
            editableValue.InitDispatcher(_deviceEventsDispatcher);
            _deviceEventsDispatcher.AddSubscriptionById(editSubscription
                , res.LocalValue.Id);



            _deviceEventsDispatcher.AddDeviceAddressSubscription(property.Address, property.NumberOfPoints,
                setUnchangedSuscription);

            var localDataSubscription = new LocalMemorySubscription(res.LocalValue, property.Address,
                property.NumberOfPoints, property.UshortsFormatter, _deviceMemory, setUnchangedSuscription);
            _deviceEventsDispatcher.AddLocalAddressSubscription(property.Address, property.NumberOfPoints,
                localDataSubscription);
            return res;
        }

        private ushort[] InitDefaultUshortsValue(ushort numOfPoints)
        {
            var res = new ushort[numOfPoints];
            for (int i = 0; i < numOfPoints; i++)
            {
                res[i] = 0;
            }

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

        public void Initialize(IDeviceEventsDispatcher deviceEventsDispatcher)
        {
            _deviceEventsDispatcher = deviceEventsDispatcher;
        }

        public object Clone()
        {
            return new RuntimeConfigurationItemViewModelFactory(_container, _deviceMemory, _deviceEventsDispatcher);

        }
    }
}