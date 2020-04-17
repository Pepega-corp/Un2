using System;
using System.Collections.Generic;
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
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
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

        public static RuntimeConfigurationItemViewModelFactory WithOffset(
	        this RuntimeConfigurationItemViewModelFactory factory, int offset)
        {
	        var newFactory = factory.Clone() as RuntimeConfigurationItemViewModelFactory;
	        newFactory.AddressOffset = offset;
	        return newFactory;
        }
	}

    public class RuntimeConfigurationItemViewModelFactory : IRuntimeConfigurationItemViewModelFactory, ICloneable
    {
        private readonly ITypesContainer _container;
        private readonly DeviceContext _deviceContext;

        public RuntimeConfigurationItemViewModelFactory(ITypesContainer container, DeviceContext deviceContext)
        {
	        _container = container;
	        _deviceContext = deviceContext;
        }
		public int AddressOffset { get; set; }
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
            if (itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo &&
                groupWithReiterationInfo.IsReiterationEnabled)
            {
	            int offset = AddressOffset;

				foreach (var subGroupInfo in groupWithReiterationInfo.SubGroups)
	            {
		            var subGroup = _container.Resolve<IRuntimeItemGroupViewModel>();

		            subGroup.Description = itemsGroup.Description;
		            subGroup.Header = subGroupInfo.Name;
		            if (Parent != null)
		            {
			            subGroup.Parent = Parent;
			            subGroup.Level = Parent.Level + 1;
		            }

		            foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
		            {
			            subGroup.ChildStructItemViewModels.Add(configurationItem.Accept(this.WithParent(subGroup)
				            .WithOffset(offset)));
		            }

		            res.ChildStructItemViewModels.Add(subGroup);
		            offset += groupWithReiterationInfo.ReiterationStep;
	            }
            }
            else
			{
				foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
				{
					res.ChildStructItemViewModels.Add(configurationItem.Accept(this.WithParent(res)));
				}
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

            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription((ushort)(property.Address+AddressOffset), property.NumberOfPoints,
                new DeviceDataPropertyMemorySubscription(property, res, _container.Resolve<IValueViewModelFactory>(),
                    _deviceContext.DeviceMemory,(ushort)AddressOffset));

            var localValue = _container.Resolve<IFormattingService>().FormatValue(property.UshortsFormatter,
                InitDefaultUshortsValue(property.NumberOfPoints));
            var editableValue = _container.Resolve<IValueViewModelFactory>()
                .CreateEditableValueViewModel(new FormattedValueInfo(localValue, property, property.UshortsFormatter,
                    property));
            var setUnchangedSuscription = new EditableValueSetUnchangedSubscription(editableValue, _deviceContext.DeviceMemory,
				(ushort)(property.Address + AddressOffset), property.NumberOfPoints);

            var editSubscription =
                new LocalDataEditedSubscription(editableValue, _deviceContext.DeviceMemory, property, setUnchangedSuscription,AddressOffset);

            res.LocalValue = editableValue;
            editableValue.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
            _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
                , res.LocalValue.Id);



            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription((ushort)(property.Address + AddressOffset), property.NumberOfPoints,
                setUnchangedSuscription);

            var localDataSubscription = new LocalMemorySubscription(res.LocalValue, (ushort)(property.Address + AddressOffset),
                property.NumberOfPoints, property.UshortsFormatter, _deviceContext.DeviceMemory, setUnchangedSuscription);
            _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription((ushort)(property.Address + AddressOffset), property.NumberOfPoints,
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

            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription((ushort)(complexProperty.Address+AddressOffset), complexProperty.NumberOfPoints,
	            new DeviceDataComplexPropertySubscription(_container.Resolve<IValueViewModelFactory>(), _deviceContext.DeviceMemory, complexProperty, res, (ushort)AddressOffset));

			List<EditableValueSetUnchangedSubscription> setUnchangedSuscriptions=new List<EditableValueSetUnchangedSubscription>();
			foreach (ISubProperty subProperty in complexProperty.SubProperties)
            {
	            IRuntimeSubPropertyViewModel subPropertyViewModel = subProperty.Accept(this) as IRuntimeSubPropertyViewModel;

                var localValue = _container.Resolve<IFormattingService>().FormatValue(subProperty.UshortsFormatter,
	                InitDefaultUshortsValue(subProperty.NumberOfPoints));


                var editableValue = _container.Resolve<IValueViewModelFactory>()
	                .CreateEditableValueViewModel(new FormattedValueInfo(localValue, subProperty, subProperty.UshortsFormatter,
		                subProperty));


                var setUnchangedSuscription = new EditableValueSetUnchangedSubscription(editableValue, _deviceContext.DeviceMemory,
					(ushort)(subProperty.Address + AddressOffset), subProperty.NumberOfPoints);
                setUnchangedSuscriptions.Add(setUnchangedSuscription);
				var editSubscription =
	                new LocalDataComplexPropertyEditedSubscription(res, _deviceContext.DeviceMemory, subProperty,complexProperty, setUnchangedSuscription,AddressOffset);


                subPropertyViewModel.LocalValue = editableValue;
                editableValue.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
                _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
	                , subPropertyViewModel.LocalValue.Id);


                _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription((ushort)(subProperty.Address + AddressOffset), subProperty.NumberOfPoints,
	                setUnchangedSuscription);


				res.ChildStructItemViewModels.Add(subPropertyViewModel);
                res.IsCheckable = true;
            }



			var localDataSubscription = new 
				LocalComplexPropertyMemorySubscription(res,complexProperty, _deviceContext.DeviceMemory, setUnchangedSuscriptions,AddressOffset);
			_deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription((ushort)(complexProperty.Address + AddressOffset), complexProperty.NumberOfPoints,
				localDataSubscription);

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

        public object Clone()
        {
            return new RuntimeConfigurationItemViewModelFactory(_container, _deviceContext)
            {
				Parent=Parent,
				AddressOffset = AddressOffset
            };
        }
    }
}