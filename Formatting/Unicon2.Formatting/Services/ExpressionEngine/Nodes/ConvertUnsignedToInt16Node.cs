using System;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class ConvertUnsignedToInt16Node : IRuleNode
    {
        private readonly IRuleNode _ruleNode;

        public ConvertUnsignedToInt16Node(IRuleNode ruleNode)
        {
            _ruleNode = ruleNode;
        }
        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return (double)(short)(ushort)(double)await _ruleNode.ExecuteNode(ruleExecutionContext);
        }
    }
}