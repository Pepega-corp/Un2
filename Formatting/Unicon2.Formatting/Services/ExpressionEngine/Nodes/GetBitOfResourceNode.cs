using System;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Services;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class GetBitOfResourceNode : RuleNodeBase
    {
        private string _resource;
        private IRuleNode _numberOfBit;

        public GetBitOfResourceNode(IRuleNode numberOfBit, string resource)
        {
            _numberOfBit = numberOfBit;
            _resource = resource;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var resource = ruleExecutionContext.DeviceContext.DeviceSharedResources.SharedResourcesInContainers
                .FirstOrDefault(
                    container => container.ResourceName == _resource);
            var resUshorts = await StaticContainer.Container.Resolve<IPropertyValueService>()
                .GetUshortsOfProperty(resource.Resource, ruleExecutionContext.DeviceContext, true,ruleExecutionContext.IsLocal);

            var result=resUshorts.Item[0].GetBoolArrayFromUshort()[
                    Convert.ToUInt16(await _numberOfBit.ExecuteNode(ruleExecutionContext))];
            return result;
        }
    }
}