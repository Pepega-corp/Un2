using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class DivideNode : RuleNodeBase
    {
        private IRuleNode _leftNode { get; }
        private IRuleNode _rghtNode { get; }

        public DivideNode(IRuleNode leftNode, IRuleNode rightNode)
        {
            _leftNode = leftNode;
            _rghtNode = rightNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var res1 = await _leftNode.ExecuteNode(ruleExecutionContext);
            var res2 = await _rghtNode.ExecuteNode(ruleExecutionContext);

            return (double)res1 /
                   (double)res2;
        }
    }
}