using System.Threading.Tasks;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class GetVariableNode : RuleNodeBase
    {
        private readonly string _varName;

        public GetVariableNode(string varName)
        {
            _varName = varName;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return ruleExecutionContext.Variables[_varName];
        }
    }
}