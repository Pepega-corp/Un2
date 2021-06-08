using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class MultiplyNode : RuleNodeBase
    {
        private IRuleNode _leftNode;
        private IRuleNode _rightNode;

        public MultiplyNode(IRuleNode leftNode, IRuleNode rightNode)
        {
            _leftNode = leftNode;
            _rightNode = rightNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var res1 = await _leftNode.ExecuteNode(ruleExecutionContext);
            var res2 = await _rightNode.ExecuteNode(ruleExecutionContext);

            return (double)res1 *
                   (double)res2;
        }
    }
}