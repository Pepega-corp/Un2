using System;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SetBitOfResourceNode : RuleNodeBase
    {
        private string _resoure;
        private readonly IRuleNode _valueOfBit;
        private IRuleNode _numberOfBit;

        public SetBitOfResourceNode(IRuleNode valueOfBit,IRuleNode numberOfBit, string resoure)
        {
            _numberOfBit = numberOfBit;
            _resoure = resoure;
            _valueOfBit = valueOfBit;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var resource = ruleExecutionContext.DeviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
                container => container.ResourceName == _resoure);
            var resUshorts = await StaticContainer.Container.Resolve<IPropertyValueService>().GetUshortsOfProperty(resource.Resource, ruleExecutionContext.DeviceContext, true);


            var boolArray = resUshorts.Item.GetBoolArrayFromUshortArray();
            bool value=Convert.ToBoolean(await _valueOfBit.ExecuteNode(ruleExecutionContext));
            boolArray[Convert.ToUInt16(await _numberOfBit.ExecuteNode(ruleExecutionContext))] = value;

            var subPropertyUshort = boolArray.BoolArrayToUshort();


            ruleExecutionContext.DeviceContext.DeviceMemory.LocalMemoryValues[(resource.Resource as IWithAddress).Address] =
                subPropertyUshort;
            ruleExecutionContext.DeviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
                (resource.Resource as IWithAddress).Address, 1);
            return value;
        }
    }
}