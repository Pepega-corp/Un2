using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
	public class LocalMemorySubscription : IDeviceSubscription
	{
		private readonly IEditableValueViewModel _editableValueViewModel;
		private readonly IUshortsFormatter _ushortsFormatter;
		private readonly DeviceContext _deviceContext;
		private readonly IRuntimePropertyViewModel _runtimePropertyViewModel;
		private readonly IProperty _property;
		private readonly IFormattingService _formattingService;
		private ushort[] _prevUshorts;
		private bool _prevIsBlocked;
		private IUshortsFormatter _prevUshortFormatter;
		private int _offset;

		public LocalMemorySubscription(IEditableValueViewModel editableValueViewModel, IUshortsFormatter ushortsFormatter, DeviceContext deviceContext,
			IRuntimePropertyViewModel runtimePropertyViewModel,
			IProperty property, IFormattingService formattingService, int offset, ushort[] prevUshorts)
		{
			_editableValueViewModel = editableValueViewModel;
			_ushortsFormatter = ushortsFormatter;
			_deviceContext = deviceContext;
			_runtimePropertyViewModel = runtimePropertyViewModel;
			_property = property;
			_formattingService = formattingService;
			_offset = offset;
			_prevUshorts = prevUshorts;
			_prevUshortFormatter = property.UshortsFormatter;
		}

		public void Execute()
		{


			var newUshorts = MemoryAccessor.GetUshortsFromMemorySafe(
				_deviceContext.DeviceMemory,
				(ushort) (_property.Address + _offset), _property.NumberOfPoints, true);

			if (!newUshorts.IsSuccess)
			{
				return;
			}

			if (_property?.Dependencies?.Count > 0)
			{
				bool isInteractionBlocked = false;
				var formatterForDependentProperty = _property.UshortsFormatter;

				foreach (var dependency in _property.Dependencies)
				{
					if (dependency is IConditionResultDependency conditionResultDependency)
					{
						if (conditionResultDependency.Condition is ICompareResourceCondition compareResourceCondition)
						{
							var checkResult = DependentSubscriptionHelpers.CheckCondition(compareResourceCondition,
								_deviceContext, _formattingService, true);

							if (checkResult.IsSuccess)
							{
								if (checkResult.Item)
								{
									switch (conditionResultDependency.Result)
									{
										case IApplyFormatterResult applyFormatterResult:
											formatterForDependentProperty = applyFormatterResult.UshortsFormatter;
											break;
										case IBlockInteractionResult blockInteractionResult:
											isInteractionBlocked = checkResult.Item;
											break;
									}
								}
								else
								{
									switch (conditionResultDependency.Result)
									{
										case IBlockInteractionResult blockInteractionResult:
											isInteractionBlocked = checkResult.Item;
											break;
									}
								}
							}
							else
							{
								return;
							}
						}

					}
				}

				if (_prevIsBlocked == isInteractionBlocked && formatterForDependentProperty == _prevUshortFormatter)
				{
					ProcessValueViewModel(newUshorts);
					return;
				}

				_prevUshorts = newUshorts.Item;
				_prevIsBlocked = isInteractionBlocked;
				_prevUshortFormatter = formatterForDependentProperty;
				if (_runtimePropertyViewModel?.LocalValue != null)
				{
					_deviceContext.DeviceEventsDispatcher.RemoveSubscriptionById(_runtimePropertyViewModel
						.LocalValue.Id);
					_runtimePropertyViewModel.LocalValue.Dispose();
				}

				var localValue = _formattingService.FormatValue(formatterForDependentProperty,
					newUshorts.Item);
				var editableValue = StaticContainer.Container.Resolve<IValueViewModelFactory>()
					.CreateEditableValueViewModel(new FormattedValueInfo(localValue, _property,
						formatterForDependentProperty,
						_property, !isInteractionBlocked, !_prevUshorts.IsEqual(newUshorts.Item)));
				var editSubscription =
					new LocalDataEditedSubscription(editableValue, _deviceContext, _property, _offset);
				_runtimePropertyViewModel.LocalValue = editableValue;
				editableValue?.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
				if (_runtimePropertyViewModel.LocalValue != null)
					_deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
						, _runtimePropertyViewModel.LocalValue.Id);
			}
			else
			{
				if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
					(ushort) (_property.Address), _property.NumberOfPoints, true))
				{
					return;
				}

				ProcessValueViewModel(newUshorts);
			}


		}

		private void ProcessValueViewModel(Result<ushort[]> newUshorts)
		{
			if (!newUshorts.Item.IsEqual(_prevUshorts))
			{
				_prevUshorts = newUshorts.Item;
				var localValue = StaticContainer.Container.Resolve<IFormattingService>().FormatValue(
					_ushortsFormatter,
					_prevUshorts);
				_editableValueViewModel.Accept(new EditableValueSetFromLocalVisitor(localValue));
			}
		}


	}
}