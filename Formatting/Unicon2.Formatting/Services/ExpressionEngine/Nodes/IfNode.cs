using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class IfNode : RuleNodeBase
    {
        private IRuleNode _conditionNode { get; }
        private IRuleNode _ifYesNode { get; }
        private IRuleNode _ifNoNode { get; }

        public IfNode(IRuleNode conditionNode, IRuleNode ifYesNode, IRuleNode ifNoNode = null)
        {
            _conditionNode = conditionNode;
            _ifYesNode = ifYesNode;
            _ifNoNode = ifNoNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var condition = (bool)await _conditionNode.ExecuteNode(ruleExecutionContext);

            if (condition)
            {
                return await _ifYesNode.ExecuteNode(ruleExecutionContext);
            }

            if (_ifNoNode == null)
            {
                return null;
            }

            return await _ifNoNode.ExecuteNode(ruleExecutionContext);
        }
    }
}