using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Measuring.Helpers
{
    public static class AnalogElementHelpers
    {
        public static async Task<ushort> GetAnalogElementAddress(this IAnalogMeasuringElement analogMeasuringElement,
            DeviceContext deviceContext)
        {
            ushort address = analogMeasuringElement.Address;
            var deps = analogMeasuringElement.Dependencies.OfType<IBoolToAddressDependency>();
            foreach (var dependency in deps)
            {
                (await IsDependencyTrue(dependency, deviceContext)).OnSuccess(b =>
                    address = b ? dependency.ResultingAddressIfTrue : dependency.ResultingAddressIfFalse);
            }

            return address;

        }

        public static async Task<IUshortsFormatter> GetAnalogElementFormatting(this IAnalogMeasuringElement analogMeasuringElement,
            DeviceContext deviceContext)
        {
            IUshortsFormatter formatter = analogMeasuringElement.UshortsFormatter;
            var deps = analogMeasuringElement.Dependencies.OfType<IBoolToAddressDependency>();
            foreach (var dependency in deps)
            {
                (await IsDependencyTrue(dependency, deviceContext)).OnSuccess(b =>
                    formatter = b ? dependency.FormatterIfTrue : dependency.FormatterIfFalse);
            }

            return formatter;
        }

        private static async Task<Result<bool>> IsDependencyTrue(IBoolToAddressDependency boolToAddressDependency,
            DeviceContext deviceContext)
        {
            if (!deviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                return Result<bool>.Create(false);
            }
            if(string.IsNullOrWhiteSpace(boolToAddressDependency.RelatedResourceName))return Result<bool>.Create(false);
            var res = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(container =>
                container.ResourceName == boolToAddressDependency.RelatedResourceName);
            if (res == null)
            {

            }
            if (res.Resource is IDiscretMeasuringElement discretMeasuringElement)
            {
                return await discretMeasuringElement.GetDiscretElementValue(deviceContext);
            }
            return Result<bool>.Create(false);
        }
    }
}