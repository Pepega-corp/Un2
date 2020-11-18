using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
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

            runtimePropertyViewModel.Address = property.Address;
            if (runtimePropertyViewModel is IDeviceContextConsumer deviceContextConsumer)
            {
                deviceContextConsumer.DeviceContext = _deviceContext;
            }
            InitializeBaseProperties(runtimePropertyViewModel, property);
        }

        public FactoryResult<IRuntimeConfigurationItemViewModel> VisitItemsGroup(IItemsGroup itemsGroup)
        {
            var res = _container.Resolve<IRuntimeItemGroupViewModel>() as RuntimeItemGroupViewModel;
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
			            configurationItem.Accept(this.WithParent(subGroup)
				            .WithOffset(offset)).OnAddingNeeded(subGroup.ChildStructItemViewModels.Add);



		            }

		            res.ChildStructItemViewModels.Add(subGroup);
                    subGroup.Offset = offset;

                    offset += groupWithReiterationInfo.ReiterationStep;
                }
            }
            else
			{
				foreach (IConfigurationItem configurationItem in itemsGroup.ConfigurationItemList)
				{
					configurationItem.Accept(this.WithParent(res)).OnAddingNeeded(res.ChildStructItemViewModels.Add);
				}
			}
         

            res.IsMain = itemsGroup.IsMain ?? false;
            res.IsTableViewAllowed = itemsGroup.IsTableViewAllowed;

            if (itemsGroup.GroupFilter != null)
            {
                res.FilterViewModels = itemsGroup.GroupFilter.Filters.Select(filter =>
                    new RuntimeFilterViewModel(filter.Name, () => res.TryTransformToTable(),filter.Conditions)).ToList();
            }

            InitializeBaseProperties(res, itemsGroup);
			return FactoryResult<IRuntimeConfigurationItemViewModel>.Create(res);

		}

		public FactoryResult<IRuntimeConfigurationItemViewModel> VisitProperty(IProperty property)
        {
            var res = _container.Resolve<IRuntimePropertyViewModel>();
            InitializeProperty(res, property);
            var formattingService = _container.Resolve<IFormattingService>();

            
            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription((ushort)(property.Address+AddressOffset), property.NumberOfPoints,
                new DeviceDataPropertyMemorySubscription(property, res, _container.Resolve<IValueViewModelFactory>(),
                    _deviceContext,(ushort)AddressOffset));
            var localUshorts = InitDefaultUshortsValue(property.NumberOfPoints);
            var localValue = _container.Resolve<IFormattingService>().FormatValue(property.UshortsFormatter,
	            localUshorts);
            var editableValue = _container.Resolve<IValueViewModelFactory>()
                .CreateEditableValueViewModel(new FormattedValueInfo(localValue, property, property.UshortsFormatter,
                    property));
            var setUnchangedSuscription = new EditableValueSetUnchangedSubscription(res, _deviceContext.DeviceMemory,
				(ushort)(property.Address + AddressOffset), property.NumberOfPoints);

            var editSubscription =
                new LocalDataEditedSubscription(editableValue, _deviceContext, property,AddressOffset);

            res.LocalValue = editableValue;
            editableValue?.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
            if (res.LocalValue != null)
            {
	            _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
		            , res.LocalValue.Id);
            }

            if (property?.Dependencies?.Count > 0)
            {
	            AddSubscriptionForConditions(property, (ushort address, ushort numOfPoints) =>
		            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(address, numOfPoints,
			            new DeviceDataPropertyMemorySubscription(property, res,
				            _container.Resolve<IValueViewModelFactory>(),
				            _deviceContext, (ushort) AddressOffset)));

	            AddSubscriptionForConditions(property, (ushort address, ushort numOfPoints) =>
		            _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(address, numOfPoints,
			            new LocalMemorySubscription(
				             property.UshortsFormatter, _deviceContext,res,property,formattingService,AddressOffset,localUshorts)));


	            AddSubscriptionForConditions(property, (ushort address, ushort numOfPoints) =>
		            _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(address, numOfPoints,
			            new EditableValueSetUnchangedSubscription(res, _deviceContext.DeviceMemory,
				            (ushort)(property.Address + AddressOffset), property.NumberOfPoints)));

	            AddSubscriptionForConditions(property, (ushort address, ushort numOfPoints) =>
		            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(address, numOfPoints,
			            new EditableValueSetUnchangedSubscription(res, _deviceContext.DeviceMemory,
				            (ushort)(property.Address + AddressOffset), property.NumberOfPoints)));
            }
            else
            {
	            
	            _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(
		            (ushort) (property.Address + AddressOffset), property.NumberOfPoints,
		            setUnchangedSuscription);
	            _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(
		            (ushort) (property.Address + AddressOffset), property.NumberOfPoints,
		            setUnchangedSuscription);

	            var localDataSubscription = new LocalMemorySubscription(property.UshortsFormatter, _deviceContext,res,property,formattingService,AddressOffset,localUshorts);
	            _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(
		            (ushort) (property.Address + AddressOffset), property.NumberOfPoints,
		            localDataSubscription);
            }
            
            
            return FactoryResult<IRuntimeConfigurationItemViewModel>.Create(res);

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

        public FactoryResult<IRuntimeConfigurationItemViewModel> VisitComplexProperty(IComplexProperty complexProperty)
        {
            var res = _container.Resolve<IRuntimeComplexPropertyViewModel>();
            var formattingService = _container.Resolve<IFormattingService>();

            foreach (ISubProperty subProperty in complexProperty.SubProperties)
            {

                IRuntimeSubPropertyViewModel subPropertyViewModel =
                    subProperty.Accept(this).Item as IRuntimeSubPropertyViewModel;
                _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(
                    (ushort) (subProperty.Address + AddressOffset), subProperty.NumberOfPoints,
                    new DeviceDataSubPropertySubscription(subProperty, subPropertyViewModel, (ushort) AddressOffset,
                        _container.Resolve<IValueViewModelFactory>(), _deviceContext));




                var localValue = _container.Resolve<IFormattingService>().FormatValue(subProperty.UshortsFormatter,
                    InitDefaultUshortsValue(subProperty.NumberOfPoints));


                var editableValue = _container.Resolve<IValueViewModelFactory>()
                    .CreateEditableValueViewModel(new FormattedValueInfo(localValue, subProperty,
                        subProperty.UshortsFormatter,
                        subProperty));


                var setUnchangedSuscription = new SubPropertySetUnchangedSubscription(subProperty.BitNumbersInWord,
                    subPropertyViewModel,
                    _deviceContext.DeviceMemory,
                    (ushort) (subProperty.Address + AddressOffset), subProperty.NumberOfPoints);
                var editSubscription =
                    new LocalDataComplexPropertyEditedSubscription(res, _deviceContext,
                        complexProperty, AddressOffset);


                subPropertyViewModel.LocalValue = editableValue;
                editableValue?.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
                _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
                    , subPropertyViewModel.LocalValue.Id);


          

                res.ChildStructItemViewModels.Add(subPropertyViewModel);
                res.IsCheckable = true;
                if (!complexProperty.IsGroupedProperty)
                {

                    Parent.ChildStructItemViewModels.Add(subPropertyViewModel);
                    Parent.IsCheckable = true;
                }



                if (subProperty?.Dependencies?.Count > 0)
                {
	                AddSubscriptionForConditions(subProperty, (ushort address, ushort numOfPoints) =>
		                _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(address, numOfPoints,
			                new DeviceDataSubPropertySubscription(subProperty, subPropertyViewModel,
				                (ushort) AddressOffset,
				                _container.Resolve<IValueViewModelFactory>(), _deviceContext)));

	                AddSubscriptionForConditions(subProperty, (ushort address, ushort numOfPoints) =>
		                _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(address, numOfPoints,
			                new
				                LocalSubPropertySubscription(_deviceContext, subPropertyViewModel, subProperty,
					                formattingService,
					                AddressOffset, res, complexProperty)));


	                AddSubscriptionForConditions(subProperty, (ushort address, ushort numOfPoints) =>
		                _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(address, numOfPoints,
			                new EditableValueSetUnchangedSubscription(res, _deviceContext.DeviceMemory,
				                (ushort) (subProperty.Address + AddressOffset), subProperty.NumberOfPoints)));

	                AddSubscriptionForConditions(subProperty, (ushort address, ushort numOfPoints) =>
		                _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(address, numOfPoints,
			                new EditableValueSetUnchangedSubscription(res, _deviceContext.DeviceMemory,
				                (ushort) (subProperty.Address + AddressOffset), subProperty.NumberOfPoints)));

	                AddSubscriptionForConditions(subProperty, (address, numOfPoints) =>
		                _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(
			                (ushort) (address + AddressOffset), numOfPoints,
			                setUnchangedSuscription));

	                AddSubscriptionForConditions(subProperty, (address, numOfPoints) =>
		                _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(
			                (ushort) (address + AddressOffset), numOfPoints,
			                setUnchangedSuscription));
	                
                }
                else
                {

                    _deviceContext.DeviceEventsDispatcher.AddDeviceAddressSubscription(
                        (ushort)(subProperty.Address + AddressOffset), subProperty.NumberOfPoints,
                        setUnchangedSuscription);

                    _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(
                        (ushort)(subProperty.Address + AddressOffset), subProperty.NumberOfPoints,
                        setUnchangedSuscription);

                    var localDataSubscription = new
                        LocalSubPropertySubscription(_deviceContext, subPropertyViewModel, subProperty, formattingService,
                            AddressOffset,res,complexProperty);
                    _deviceContext.DeviceEventsDispatcher.AddLocalAddressSubscription(
                        (ushort)(complexProperty.Address + AddressOffset), complexProperty.NumberOfPoints,
                        localDataSubscription);
                }





            }









            res.IsGroupedProperty = complexProperty.IsGroupedProperty;
            InitializeProperty(res, complexProperty);
            return FactoryResult<IRuntimeConfigurationItemViewModel>.Create(res,complexProperty.IsGroupedProperty);
        }

        public FactoryResult<IRuntimeConfigurationItemViewModel> VisitMatrix(IAppointableMatrix appointableMatrix)
        {
            var res = _container.Resolve<IRuntimeAppointableMatrixViewModel>();
            InitializeProperty(res, appointableMatrix);
            return FactoryResult<IRuntimeConfigurationItemViewModel>.Create(res);
        }

   

        private void AddSubscriptionForConditions(IProperty property,
			Action<ushort, ushort> memoryOperationAction)
		{
			if (property?.Dependencies?.Count > 0)
			{
				foreach (var dependency in property.Dependencies)
				{
					if (dependency is IConditionResultDependency conditionResultDependency &&
					    conditionResultDependency.Condition is ICompareResourceCondition compareResourceCondition)
					{
						var relatedProperty =
							_deviceContext.DeviceSharedResources.SharedResourcesInContainers.First(container =>
									container.ResourceName == compareResourceCondition.ReferencedPropertyResourceName)
								.Resource as IProperty;
						memoryOperationAction.Invoke(relatedProperty.Address,
							relatedProperty.NumberOfPoints);
					}
				}
			}
            memoryOperationAction.Invoke((ushort)(property.Address + AddressOffset),
                property.NumberOfPoints);
        }

		public FactoryResult<IRuntimeConfigurationItemViewModel> VisitSubProperty(ISubProperty subProperty)
        {
            var res = _container.Resolve<IRuntimeSubPropertyViewModel>();
            res.BitNumbersInWord = subProperty.BitNumbersInWord;
            InitializeProperty(res, subProperty);
            return FactoryResult<IRuntimeConfigurationItemViewModel>.Create(res);

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