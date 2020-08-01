using System;
using System.Collections;
using System.Linq;
using System.Windows;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class DeviceDataDependentPropertyMemorySubscription : IDeviceDataMemorySubscription
	{
		private readonly IDependentProperty _dependentProperty;
		private readonly ILocalAndDeviceValueContainingViewModel _localAndDeviceValueContainingViewModel;
		private readonly ushort _offset;
		private readonly IValueViewModelFactory _valueViewModelFactory;
		private readonly DeviceContext _deviceContext;
		private IFormattingService _formattingService;

		public DeviceDataDependentPropertyMemorySubscription(IDependentProperty dependentProperty,
			ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,
			IValueViewModelFactory valueViewModelFactory, DeviceContext deviceContext, ushort offset, IFormattingService formattingService)
		{
			_dependentProperty = dependentProperty;
			_localAndDeviceValueContainingViewModel = localAndDeviceValueContainingViewModel;
			_valueViewModelFactory = valueViewModelFactory;
			_deviceContext = deviceContext;
			_offset = offset;
			_formattingService = formattingService;
		}

		public void Execute()
		{
			if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
				(ushort)(_dependentProperty.Address + _offset), _dependentProperty.NumberOfPoints, false))
			{
				return;
			}
			var formatterForDependentProperty = _dependentProperty.UshortsFormatter;
			foreach (var condition in _dependentProperty.DependancyConditions)
			{
				if (condition.ConditionResult == ConditionResultEnum.ApplyingFormatter)
				{
				    var checkResult = DependentSubscriptionHelpers.CheckCondition(condition, condition.UshortValueToCompare,
				        _deviceContext, _formattingService, false);
				    if (checkResult.IsSuccess)
				    {
				        if (checkResult.IsSuccess)
				        {
				            formatterForDependentProperty = condition.UshortsFormatter;
                        }

                    }
				    else
				    {
				        return;
				    }
				}
			}

			
			var value = _formattingService.FormatValue(formatterForDependentProperty,
				MemoryAccessor.GetUshortsFromMemory(
					_deviceContext.DeviceMemory,
					(ushort) (_dependentProperty.Address + _offset), _dependentProperty.NumberOfPoints, false));
			_localAndDeviceValueContainingViewModel.DeviceValue =
				_valueViewModelFactory.CreateFormattedValueViewModel(value);
		
		}


	}
}