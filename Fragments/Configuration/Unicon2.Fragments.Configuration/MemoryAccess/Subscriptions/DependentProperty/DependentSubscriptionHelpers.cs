using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class DependentSubscriptionHelpers
	{

		public static Result<bool> CheckConditionFromResource(ICompareResourceCondition condition,
			DeviceContext deviceContext, IFormattingService formattingService, bool isLocal, ushort addressOffset)
		{
			var conditionUshortResult = GetConditionPropertyUshort(condition,deviceContext,formattingService,isLocal, addressOffset);
			var ushortToCompare = condition.UshortValueToCompare;
			if (!conditionUshortResult.IsSuccess)
			{
				return Result<bool>.Create(false);
			}
            return ConditionHelper.CheckCondition(condition, conditionUshortResult.Item);
        }



		public static IUshortsFormatter GetFormatterConsideringDependencies(List<IDependency> dependencies,
			DeviceContext deviceContext, IFormattingService formattingService,IUshortsFormatter ushortsFormatterInitial, ushort offset)
		{
			var formatterForDependentProperty = ushortsFormatterInitial;
			foreach (var dependency in dependencies)
			{
				switch (dependency)
				{
					case IConditionResultDependency conditionResultDependency:
						if (
							conditionResultDependency
								?.Condition is ICompareResourceCondition compareResourceCondition &&
							conditionResultDependency.Result is IApplyFormatterResult applyFormatterResult)
						{
							var checkResult = CheckConditionFromResource(compareResourceCondition,
								deviceContext, formattingService, true,offset);

							if (checkResult.IsSuccess)
							{

								if (checkResult.Item)
								{
									formatterForDependentProperty = applyFormatterResult.UshortsFormatter;
								}
							}
						}

						break;
				}
			}

			return formatterForDependentProperty;
		}

		private static Result<ushort> GetConditionPropertyUshort(ICompareResourceCondition dependancyCondition,DeviceContext deviceContext,IFormattingService formattingService,bool isLocal, ushort addressOffset)
		{
			var resourceProperty = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
				container =>
					container.ResourceName == dependancyCondition.ReferencedPropertyResourceName).Resource as IProperty;

			if (!MemoryAccessor.IsMemoryContainsAddresses(deviceContext.DeviceMemory,
				(ushort) (resourceProperty.Address+addressOffset), resourceProperty.NumberOfPoints, isLocal))
			{
				return Result<ushort>.Create(false);
			}


			var propertyUshorts = MemoryAccessor.GetUshortsFromMemory(
				deviceContext.DeviceMemory,
				(ushort)(resourceProperty.Address+addressOffset), resourceProperty.NumberOfPoints, isLocal);

            if (resourceProperty is ISubProperty subProperty)
            {
                var x = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                        container =>
                            container.ResourceName == dependancyCondition.ReferencedPropertyResourceName)
                    .Resource;

				var resultBitArray = new bool[16];
                var boolArray = propertyUshorts.GetBoolArrayFromUshortArray();
                int counter = 0;
                for (int i = 0; i < 16; i++)
                {
                    if (subProperty.BitNumbersInWord.Contains(i))
                    {
                        resultBitArray[counter] = boolArray[i];
                        counter++;
                    }

                }

                propertyUshorts = resultBitArray.BoolArrayToUshort().AsCollection();

            }


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
