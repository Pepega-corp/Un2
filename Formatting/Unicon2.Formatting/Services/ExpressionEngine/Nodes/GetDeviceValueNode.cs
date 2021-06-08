using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class GetDeviceValueNode : RuleNodeBase
    {
        private readonly IRuleNode _numberNode;

        public GetDeviceValueNode(IRuleNode numberNode)
        {
            _numberNode = numberNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return Convert.ToDouble((ruleExecutionContext.Variables["deviceValue"] as List<ushort>)[
                Convert.ToInt32((double)await _numberNode.ExecuteNode(ruleExecutionContext))]);
        }
    }
}