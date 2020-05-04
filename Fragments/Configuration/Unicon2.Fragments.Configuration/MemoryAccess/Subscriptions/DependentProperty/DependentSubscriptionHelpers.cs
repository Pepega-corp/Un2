using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class DependentSubscriptionHelpers
	{
		public static bool CheckCondition(IDependancyCondition dependancyCondition, ushort ushortToCompare,DeviceContext deviceContext,IFormattingService formattingService,bool isLocal)
		{
			var conditionUshort = GetConditionPropertyUshort(dependancyCondition,deviceContext,formattingService,isLocal);

			switch (dependancyCondition.ConditionsEnum)
			{
				case ConditionsEnum.Equal:
					return conditionUshort == ushortToCompare;
					break;
				case ConditionsEnum.HaveFalseBitAt:
					return !conditionUshort.GetBoolArrayFromUshort()[ushortToCompare];
					break;
				case ConditionsEnum.NotEqual:
					return conditionUshort != ushortToCompare;
					break;
				case ConditionsEnum.More:
					return conditionUshort > ushortToCompare;
					break;
				case ConditionsEnum.Less:
					return conditionUshort < ushortToCompare;
					break;
				case ConditionsEnum.LessOrEqual:
					return conditionUshort <= ushortToCompare;
					break;
				case ConditionsEnum.MoreOrEqual:
					return conditionUshort >= ushortToCompare;
					break;
				case ConditionsEnum.HaveTrueBitAt:
					return conditionUshort.GetBoolArrayFromUshort()[ushortToCompare];
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static ushort GetConditionPropertyUshort(IDependancyCondition dependancyCondition,DeviceContext deviceContext,IFormattingService formattingService,bool isLocal)
		{
			var resourceProperty = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
				container =>
					container.ResourceName == dependancyCondition.ReferencedPropertyResourceName).Resource as IProperty;

			var propertyUshorts = MemoryAccessor.GetUshortsFromMemory(
				deviceContext.DeviceMemory,
				(ushort)(resourceProperty.Address), resourceProperty.NumberOfPoints, isLocal);

			if (resourceProperty.UshortsFormatter != null)
			{
				var value = formattingService.FormatValue(resourceProperty.UshortsFormatter, propertyUshorts);

				if (double.TryParse(value.AsString(), out double conditionNumber))
				{
					return (ushort)conditionNumber;
				}
				else
				{
					return propertyUshorts.First();
				}
			}
			return propertyUshorts.First();
		}
	}
}
