using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SetVariableNode : RuleNodeBase
    {
        private readonly string _varName;
        private readonly IRuleNode _node;

        public SetVariableNode(IRuleNode node, string varName)
        {
            _varName = varName;
            _node = node;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            ruleExecutionContext.Variables.Add(_varName, await _node.ExecuteNode(ruleExecutionContext));

            return ruleExecutionContext.Variables[_varName];
        }
    }
}