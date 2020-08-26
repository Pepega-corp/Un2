using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class DependentSubscriptionHelpers
	{
		public static Result<bool> CheckCondition(IDependancyCondition dependancyCondition, ushort ushortToCompare,DeviceContext deviceContext,IFormattingService formattingService,bool isLocal)
		{
			var conditionUshortResult = GetConditionPropertyUshort(dependancyCondition,deviceContext,formattingService,isLocal);
		    if (!conditionUshortResult.IsSuccess)
		    {
		        return Result<bool>.Create(false);
		    }
		    var conditionUshort = conditionUshortResult.Item;

            switch (dependancyCondition.ConditionsEnum)
			{
				case ConditionsEnum.Equal:
					return Result<bool>.Create(conditionUshort == ushortToCompare,true);
				case ConditionsEnum.HaveFalseBitAt:
					return Result<bool>.Create(!conditionUshort.GetBoolArrayFromUshort()[ushortToCompare], true);
				case ConditionsEnum.NotEqual:
					return Result<bool>.Create(conditionUshort != ushortToCompare, true);
				case ConditionsEnum.More:
					return Result<bool>.Create(conditionUshort > ushortToCompare, true);
				case ConditionsEnum.Less:
					return Result<bool>.Create(conditionUshort < ushortToCompare, true);
				case ConditionsEnum.LessOrEqual:
					return Result<bool>.Create(conditionUshort <= ushortToCompare, true);
				case ConditionsEnum.MoreOrEqual:
					return Result<bool>.Create(conditionUshort >= ushortToCompare, true);
				case ConditionsEnum.HaveTrueBitAt:
					return Result<bool>.Create(conditionUshort.GetBoolArrayFromUshort()[ushortToCompare], true);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static Result<ushort> GetConditionPropertyUshort(IDependancyCondition dependancyCondition,DeviceContext deviceContext,IFormattingService formattingService,bool isLocal)
		{
			var resourceProperty = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
				container =>
					container.ResourceName == dependancyCondition.ReferencedPropertyResourceName).Resource as IProperty;

		    if (!MemoryAccessor.IsMemoryContainsAddresses(deviceContext.DeviceMemory,
		        (ushort) (resourceProperty.Address), resourceProperty.NumberOfPoints, isLocal))
		    {
		        return Result<ushort>.Create(false);
		    }


			var propertyUshorts = MemoryAccessor.GetUshortsFromMemory(
				deviceContext.DeviceMemory,
				(ushort)(resourceProperty.Address), resourceProperty.NumberOfPoints, isLocal);

			if (resourceProperty.UshortsFormatter != null)
			{
				var value = formattingService.FormatValue(resourceProperty.UshortsFormatter, propertyUshorts);

				if (double.TryParse(value.AsString(), out double conditionNumber))
				{
				    return Result<ushort>.Create((ushort)conditionNumber, true);
				}
				else
				{
				    return Result<ushort>.Create(propertyUshorts.First(), true);
				}
			}
			return Result<ushort>.Create(propertyUshorts.First(),true);
		}
	}
}
