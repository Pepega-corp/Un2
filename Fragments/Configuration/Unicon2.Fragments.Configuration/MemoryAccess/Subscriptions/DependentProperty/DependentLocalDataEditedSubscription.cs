using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class DependentLocalDataEditedSubscription : ILocalDataMemorySubscription
	{
		private readonly IDependentProperty _dependentProperty;
		private readonly DeviceContext _deviceContext;
		private readonly int _offset;

		public DependentLocalDataEditedSubscription(IEditableValueViewModel editableValueViewModel, IDependentProperty dependentProperty,DeviceContext deviceContext, int offset)
		{
			_dependentProperty = dependentProperty;
			_deviceContext = deviceContext;
			_offset = offset;
			EditableValueViewModel = editableValueViewModel;
		}

		public void Execute()
		{
			IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();
			IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor =
				StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();

			var formatterForDependentProperty = _dependentProperty.UshortsFormatter;
		    foreach (var condition in _dependentProperty.DependancyConditions)
		    {
		        if (condition.ConditionResult == ConditionResultEnum.ApplyingFormatter)
		        {
		            var checkResult = DependentSubscriptionHelpers.CheckCondition(condition, condition.UshortValueToCompare,
		                _deviceContext, formattingService, true);

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
		    }


		    var ushorts = formattingService.FormatBack(formatterForDependentProperty,
				EditableValueViewModel.Accept(fetchingFromViewModelVisitor));

			MemoryAccessor.GetUshortsInMemory(_deviceContext.DeviceMemory,
				(ushort) (_dependentProperty.Address + _offset), ushorts, true);
			_deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
				(ushort) (_dependentProperty.Address + _offset), (ushort) ushorts.Length);
		}

		public IEditableValueViewModel EditableValueViewModel { get; }
	}
}