using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Result<IFormattedValue>> GetValueOfProperty(object propertyMaybe, DeviceContext deviceContext)
        {
            IProperty property = propertyMaybe as IProperty;
            if (property == null)
            {
                return Result<IFormattedValue>.Create(false);
            }

            if (deviceContext.DeviceMemory != null && MemoryAccessor.IsMemoryContainsAddresses(
                    deviceContext.DeviceMemory, property.Address,
                    property.NumberOfPoints, false))
            {
                return await GetValueFromUshorts(MemoryAccessor.GetUshortsFromMemory(deviceContext.DeviceMemory,
                    property.Address,
                    property.NumberOfPoints, false), property.UshortsFormatter);
            }
            else
            {
                var ushorts =
                    await deviceContext.DataProviderContainer.DataProvider.ReadHoldingResgistersAsync(property.Address,
                        property.NumberOfPoints, "Read property");
                if (ushorts.IsSuccessful)
                {
                    MemoryAccessor.SetUshortsInMemory(deviceContext.DeviceMemory, property.Address, ushorts.Result,
                        false);
                    return await GetValueFromUshorts(ushorts.Result, property.UshortsFormatter);
                }
            }

            return Result<IFormattedValue>.Create(false);

        }

        public Task<Result<IFormattedValue>> GetValueFromUshorts(ushort[] values, IUshortsFormatter formatter)
        {
            if (formatter == null)
            {
                return Task.FromResult(Result<IFormattedValue>.Create(false));
            }

            return Task.FromResult(Result<IFormattedValue>.Create(
                this._formattingService.FormatValue(formatter, values),
                true));
        }

    }
}