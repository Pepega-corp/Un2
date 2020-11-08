using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services;

namespace Unicon2.Presentation.Connection
{
    public class ConnectionService : IConnectionService
    {
        private readonly IPropertyValueService _propertyValueService;

        public ConnectionService(IPropertyValueService propertyValueService)
        {
            this._propertyValueService = propertyValueService;
        }

        public async Task<Result<string>> CheckConnection(IConnectionState connectionState, DeviceContext deviceContext)
        {
            bool isMatches = false;
            if (connectionState.ExpectedValues == null) return Result<string>.Create(false);
            var testValueResult = await GetTestResultValue(connectionState.RelatedResourceString, deviceContext);
            if (!testValueResult.IsSuccess) return Result<string>.Create(false);
            foreach (var expectedValue in connectionState.ExpectedValues)
            {
                string pattern = expectedValue.Replace(" ", "");
                if (Regex.IsMatch(testValueResult.Item.Replace(" ", ""), pattern, RegexOptions.IgnoreCase))
                {
                    isMatches = true;
                }

                if (expectedValue.Replace(" ", "") == testValueResult.Item.Replace(" ", "")) isMatches = true;
            }

            return Result<string>.Create(testValueResult.Item,isMatches);
        }

        private async Task<Result<string>> GetTestResultValue(string connectionStateRelatedResourceString,
            DeviceContext deviceContext)
        {
            var resource = deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                container => container.ResourceName == connectionStateRelatedResourceString);
            if (resource?.Resource == null)
            {
                return Result<string>.Create(false);
            }
            var res = await this._propertyValueService.GetValueOfProperty(resource.Resource, deviceContext);
            return Result<string>.Create(() => res.Item.AsString(), () => res.IsSuccess);
        }
    }
}