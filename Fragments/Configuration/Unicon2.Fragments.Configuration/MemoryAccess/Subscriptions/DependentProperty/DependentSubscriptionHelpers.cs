using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
    public class DependentSubscriptionHelpers
    {

        public static async Task<Result<bool>> CheckCondition(ICondition condition,
            DeviceContext deviceContext, IFormattingService formattingService, bool isLocal, ushort addressOffset)
        {
            if (condition is ICompareResourceCondition compareResourceCondition)
            {
                var conditionUshortResult = await GetConditionPropertyUshort(compareResourceCondition, deviceContext,
                    formattingService, isLocal, addressOffset);
                //  var ushortToCompare = compareResourceCondition.UshortValueToCompare;
                if (!conditionUshortResult.IsSuccess)
                {
                    return Result<bool>.Create(false);
                }

                return ConditionHelper.CheckConditionEnum(compareResourceCondition, conditionUshortResult.Item);
            }

            if (condition is IRegexMatchCondition regexMatchCondition)
            {
                return await CheckRegexCondition(regexMatchCondition, deviceContext, formattingService, isLocal,
                    addressOffset);
            }

            return Result<bool>.Create(false);
        }

        private static async Task<bool> CheckRegexCondition(IRegexMatchCondition regexMatchCondition, DeviceContext deviceContext, IFormattingService formattingService, bool isLocal, ushort addressOffset)
        {
            var resourceProperty = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                container =>
                    container.ResourceName == regexMatchCondition.ReferencedPropertyResourceName).Resource as IProperty;
            var propertyUshorts = (await GetConditionPropertyUshort(regexMatchCondition.ReferencedPropertyResourceName,
                deviceContext, formattingService,
                isLocal, addressOffset, resourceProperty)).Item;

            if (resourceProperty.UshortsFormatter != null)
            {
                var value = await formattingService.FormatValueAsync(resourceProperty.UshortsFormatter, propertyUshorts,
                    new FormattingContext(null, deviceContext, isLocal));
                if (value is IStringValue stringValue)
                {
                    return Regex.IsMatch(stringValue.StrValue, regexMatchCondition.RegexPattern);
                }
            }
            return false;

        }


        public static async Task<IUshortsFormatter> GetFormatterConsideringDependencies(List<IDependency> dependencies,
            DeviceContext deviceContext, IFormattingService formattingService,
            IUshortsFormatter ushortsFormatterInitial, ushort offset, bool isLocal)
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
                            var checkResult = await CheckCondition(compareResourceCondition,
                                deviceContext, formattingService, isLocal, offset);

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

        private static async Task<Result<ushort>> GetConditionPropertyUshort(
            ICompareResourceCondition dependancyCondition, DeviceContext deviceContext,
            IFormattingService formattingService, bool isLocal, ushort addressOffset)
        {
            var resourceProperty = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                container =>
                    container.ResourceName == dependancyCondition.ReferencedPropertyResourceName).Resource as IProperty;

            var propertyUshortsRes = (await GetConditionPropertyUshort(dependancyCondition.ReferencedPropertyResourceName,
                deviceContext, formattingService,
                isLocal, addressOffset, resourceProperty));

            if (!propertyUshortsRes.IsSuccess)
            {
                return Result<ushort>.Create(false);
            }

            if (resourceProperty.UshortsFormatter != null)
            {
                var value = await formattingService.FormatValueAsync(resourceProperty.UshortsFormatter, propertyUshortsRes.Item,
                    new FormattingContext(null, deviceContext, isLocal));

                if (double.TryParse(value.AsString(), out double conditionNumber))
                {
                    return Result<ushort>.Create((ushort) conditionNumber, true);
                }
                else
                {
                    return Result<ushort>.Create(propertyUshortsRes.Item.First(), true);
                }
            }

            return Result<ushort>.Create(propertyUshortsRes.Item.First(), true);
        }

        private static async Task<Result<ushort[]>> GetConditionPropertyUshort(string resourceName,
            DeviceContext deviceContext, IFormattingService formattingService, bool isLocal, ushort addressOffset,
            IProperty resourceProperty)
        {

            if (!MemoryAccessor.IsMemoryContainsAddresses(deviceContext.DeviceMemory,
                (ushort) (resourceProperty.Address + addressOffset), resourceProperty.NumberOfPoints, isLocal))
            {
                return Result<ushort[]>.Create(false);
            }


            var propertyUshorts = MemoryAccessor.GetUshortsFromMemory(
                deviceContext.DeviceMemory,
                (ushort) (resourceProperty.Address + addressOffset), resourceProperty.NumberOfPoints, isLocal);

            if (resourceProperty is ISubProperty subProperty)
            {
                var x = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                        container =>
                            container.ResourceName == resourceName)
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

            if (resourceProperty.IsFromBits)
            {
                var x = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                        container =>
                            container.ResourceName == resourceName)
                    .Resource;

                var resultBitArray = new bool[16];
                var boolArray = propertyUshorts.GetBoolArrayFromUshortArray();
                int counter = 0;
                for (int i = 0; i < 16; i++)
                {
                    if (resourceProperty.BitNumbers.Contains((ushort) i))
                    {
                        resultBitArray[counter] = boolArray[i];
                        counter++;
                    }

                }

                propertyUshorts = resultBitArray.BoolArrayToUshort().AsCollection();

            }

            return Result<ushort[]>.Create(propertyUshorts, true);
        }
    }
}
