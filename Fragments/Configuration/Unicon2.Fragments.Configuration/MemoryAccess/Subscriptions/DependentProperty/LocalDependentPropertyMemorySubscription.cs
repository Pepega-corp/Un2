using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class LocalDependentPropertyMemorySubscription : IDeviceSubscription
	{
		private readonly IRuntimeDependentPropertyViewModel _runtimeDependentPropertyViewModel;
		private readonly IDependentProperty _dependentProperty;
		private readonly DeviceContext _deviceContext;
		private readonly IFormattingService _formattingService;
		private readonly int _offset;
		private ushort[] _prevUshorts=new ushort[0];
		private bool _prevIsBlocked = false;
		private IUshortsFormatter _prevUshortFormatter = null;


		public LocalDependentPropertyMemorySubscription(IRuntimeDependentPropertyViewModel runtimeDependentPropertyViewModel, IDependentProperty dependentProperty, DeviceContext deviceContext
			,IFormattingService formattingService, int offset)
		{
			_runtimeDependentPropertyViewModel = runtimeDependentPropertyViewModel;
			_dependentProperty = dependentProperty;
			_deviceContext = deviceContext;
			_formattingService = formattingService;
			_offset = offset;
		}

		public void Execute()
		{
			IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor = StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();
			if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
				(ushort) (_dependentProperty.Address + _offset), _dependentProperty.NumberOfPoints, true))
			{
				return;
			}

			var newUshorts = MemoryAccessor.GetUshortsFromMemory(
				_deviceContext.DeviceMemory,
				(ushort) (_dependentProperty.Address + _offset), _dependentProperty.NumberOfPoints, true);
		

			bool isInteractionBlocked = false;
			var formatterForDependentProperty = _dependentProperty.UshortsFormatter;
			foreach (var condition in _dependentProperty.DependancyConditions)
			{
				if (condition.ConditionResult == ConditionResultEnum.ApplyingFormatter)
				{
				    var checkResult = DependentSubscriptionHelpers.CheckCondition(condition, condition.UshortValueToCompare,
				        _deviceContext, _formattingService, true);

                    if (checkResult.IsSuccess)
					{
					    if (checkResult.Item)
					    {
					        formatterForDependentProperty = condition.UshortsFormatter;
					    }
					}
                    else
                    {
                        return;
                    }
				}

				if (condition.ConditionResult == ConditionResultEnum.DisableInteraction && !isInteractionBlocked)
				{
				    var checkResult = DependentSubscriptionHelpers.CheckCondition(condition, condition.UshortValueToCompare,
				        _deviceContext, _formattingService, true);


				    if (checkResult.IsSuccess)
				    {
				        if (checkResult.Item)
				        {
				            isInteractionBlocked = checkResult.Item;
                        }
				    }
				    else
				    {
				        return;
				    }
                    
				}
			}

			if (_prevUshorts.IsEqual(newUshorts)&&_prevIsBlocked==isInteractionBlocked&&formatterForDependentProperty==_prevUshortFormatter)
			{
				return;
			}
			_prevUshorts = newUshorts;
			_prevIsBlocked = isInteractionBlocked;
			_prevUshortFormatter = formatterForDependentProperty;

			if (_runtimeDependentPropertyViewModel?.LocalValue != null)
			{
				_deviceContext.DeviceEventsDispatcher.RemoveSubscriptionById(_runtimeDependentPropertyViewModel
					.LocalValue.Id);
				_runtimeDependentPropertyViewModel.LocalValue.Dispose();
			}

		
			
			var localValue = _formattingService.FormatValue(formatterForDependentProperty,
				newUshorts);

			var editableValue = StaticContainer.Container.Resolve<IValueViewModelFactory>()
				.CreateEditableValueViewModel(new FormattedValueInfo(localValue, _dependentProperty, formatterForDependentProperty,
					_dependentProperty, !isInteractionBlocked));

			

			var editSubscription =
				new DependentLocalDataEditedSubscription(editableValue,_dependentProperty, _deviceContext, _offset);


			_runtimeDependentPropertyViewModel.LocalValue = editableValue;
			editableValue.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
			_deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
				, _runtimeDependentPropertyViewModel.LocalValue.Id);
		}
	}
}