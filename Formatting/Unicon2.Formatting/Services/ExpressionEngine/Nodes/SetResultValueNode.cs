using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SetResultValueNode : RuleNodeBase
    {
        private readonly IRuleNode _numberNode;

        public SetResultValueNode(IRuleNode numberNode)
        {
            _numberNode = numberNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            ruleExecutionContext.Variables[VariableNames.RESULT_VALUE] = await _numberNode.ExecuteNode(ruleExecutionContext);
            return ruleExecutionContext.Variables[VariableNames.RESULT_VALUE];
        }
    }
}