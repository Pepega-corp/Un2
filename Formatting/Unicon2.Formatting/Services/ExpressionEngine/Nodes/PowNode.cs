using System;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class PowNode : RuleNodeBase
    {
        private readonly IRuleNode _nodeFirst;
        private readonly IRuleNode _nodeSecond;


        public PowNode(IRuleNode nodeFirst, IRuleNode nodeSecond)
        {
            _nodeFirst = nodeFirst;
            _nodeSecond = nodeSecond;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var value1 = (double)await _nodeFirst.ExecuteNode(ruleExecutionContext);
            var value2 = (double)await _nodeSecond.ExecuteNode(ruleExecutionContext);
            var res = Math.Pow(value1, value2);
            return res;
        }
    }
}