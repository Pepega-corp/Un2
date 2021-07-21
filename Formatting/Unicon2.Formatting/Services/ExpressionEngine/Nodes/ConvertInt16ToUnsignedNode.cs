using System;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class ConvertInt16ToUnsignedNode : IRuleNode
    {
        private readonly IRuleNode _ruleNode;

        public ConvertInt16ToUnsignedNode(IRuleNode ruleNode)
        {
            _ruleNode = ruleNode;
        }
        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return (double)(ushort)(short)(double)await _ruleNode.ExecuteNode(ruleExecutionContext);
        }
    }
}