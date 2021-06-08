using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SubtractNode : RuleNodeBase
    {
        public IRuleNode LeftNode { get; }
        public IRuleNode RightNode { get; }

        public SubtractNode(IRuleNode leftNode, IRuleNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var res1 = await LeftNode.ExecuteNode(ruleExecutionContext);
            var res2 = await RightNode.ExecuteNode(ruleExecutionContext);

            return (double)res1 -
                   (double)res2;
        }
    }
}