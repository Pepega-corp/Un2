using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Measuring.Helpers
{
    public static class DiscretElementHelper
    {
        public static async Task<Result<bool>> GetDiscretElementValue(
            this IDiscretMeasuringElement discretMeasuringElement,
            DeviceContext deviceContext)
        {
            if (discretMeasuringElement.AddressOfBit.NumberOfFunction == 3)
            {

                if (deviceContext.DeviceMemory.DeviceMemoryValues.ContainsKey(discretMeasuringElement.AddressOfBit
                    .Address))
                {
                    var result = deviceContext.DeviceMemory.DeviceMemoryValues[discretMeasuringElement
                        .AddressOfBit
                        .Address];
                    return Result<bool>.Create(
                        result.GetBoolArrayFromUshort()[discretMeasuringElement.AddressOfBit.BitAddressInWord], true);

                }
                else
                {
                    if (deviceContext.DataProviderContainer.DataProvider.IsSuccess)
                    {
                        var res =
                            await deviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(
                                discretMeasuringElement.AddressOfBit
                                    .Address, discretMeasuringElement.NumberOfPoints,
                                "Read discret: " + discretMeasuringElement.Name);
                        if (res.IsSuccessful)
                        {
                            return Result<bool>.Create(
                                res.Result.First().GetBoolArrayFromUshort()[
                                    discretMeasuringElement.AddressOfBit.BitAddressInWord], true);
                        }
                    }
                }
            }

            if (discretMeasuringElement.AddressOfBit.NumberOfFunction == 1)
            {
                if (deviceContext.DeviceMemory.DeviceBitMemoryValues.ContainsKey(discretMeasuringElement.AddressOfBit
                    .Address))
                {
                    return Result<bool>.Create(deviceContext.DeviceMemory.DeviceBitMemoryValues[discretMeasuringElement
                        .AddressOfBit
                        .Address], true);
                }
                else
                {
                    if (deviceContext.DataProviderContainer.DataProvider.IsSuccess)
                    {
                        var res = await deviceContext.DataProviderContainer.DataProvider.Item.ReadCoilStatusAsync(
                            discretMeasuringElement.AddressOfBit.Address,
                            "Read discret: " + discretMeasuringElement.Name);
                        if (res.IsSuccessful)
                        {
                            deviceContext.DeviceMemory.DeviceBitMemoryValues.Add(discretMeasuringElement
                                .AddressOfBit
                                .Address, res.Result);
                            return Result<bool>.Create(deviceContext.DeviceMemory.DeviceBitMemoryValues[
                                discretMeasuringElement.AddressOfBit
                                    .Address], true);

                        }
                    }
                }

            }

            return Result<bool>.Create(false);
        }
    }
}