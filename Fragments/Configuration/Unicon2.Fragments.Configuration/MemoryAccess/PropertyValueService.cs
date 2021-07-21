using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class PropertyValueService : IPropertyValueService
    {
        private readonly IFormattingService _formattingService;

        public PropertyValueService(IFormattingService formattingService)
        {
            this._formattingService = formattingService;
        }

        public async Task<Result<IFormattedValue>> GetValueOfProperty(object propertyMaybe, DeviceContext deviceContext,
            bool cacheAllowed, bool isLocal)
        {
            IProperty property = propertyMaybe as IProperty;
            if (property == null || !deviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                return Result<IFormattedValue>.Create(false);
            }

            if (cacheAllowed && deviceContext.DeviceMemory != null && MemoryAccessor.IsMemoryContainsAddresses(
                deviceContext.DeviceMemory, property.Address,
                property.NumberOfPoints, false))
            {
                return await GetValueFromUshorts(MemoryAccessor.GetUshortsFromMemory(deviceContext.DeviceMemory,
                    property.Address,
                    property.NumberOfPoints, isLocal), property.UshortsFormatter,deviceContext,isLocal);
            }

            var ushorts =
                await deviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(property.Address,
                    property.NumberOfPoints, "Read property");
            if (ushorts.IsSuccessful)
            {
                MemoryAccessor.SetUshortsInMemory(deviceContext.DeviceMemory, property.Address, ushorts.Result,
                    false);
                return await GetValueFromUshorts(ushorts.Result, property.UshortsFormatter,deviceContext,isLocal);
            }

            return Result<IFormattedValue>.Create(false);

        }

        public async Task<Result<ushort[]>> GetUshortsOfProperty(object propertyMaybe, DeviceContext deviceContext,
        bool cacheAllowed,bool isLocal)
        {
            IProperty property = propertyMaybe as IProperty;
            if (property == null || !deviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                return Result<ushort[]>.Create(false);
            }

            if (cacheAllowed && deviceContext.DeviceMemory != null && MemoryAccessor.IsMemoryContainsAddresses(
                deviceContext.DeviceMemory, property.Address,
                property.NumberOfPoints, isLocal))
            {
                return MemoryAccessor.GetUshortsFromMemory(deviceContext.DeviceMemory,
                    property.Address,
                    property.NumberOfPoints, isLocal);
            }

            var ushorts =
                await deviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(property.Address,
                    property.NumberOfPoints, "Read property");
            if (ushorts.IsSuccessful)
            {
                MemoryAccessor.SetUshortsInMemory(deviceContext.DeviceMemory, property.Address, ushorts.Result,
                    isLocal);
                return ushorts.Result;
            }

            return Result<ushort[]>.Create(false);
        }

        public async Task<Result<IFormattedValue>> GetValueFromUshorts(ushort[] values, IUshortsFormatter formatter,
            DeviceContext deviceContext, bool isLocal)
        {
            if (formatter == null)
            {
                return Result<IFormattedValue>.Create(false);
            }

            return Result<IFormattedValue>.Create(
                await this._formattingService.FormatValueAsync(formatter, values,
                    new FormattingContext(null, deviceContext, isLocal)),
                true);
        }

    }
}