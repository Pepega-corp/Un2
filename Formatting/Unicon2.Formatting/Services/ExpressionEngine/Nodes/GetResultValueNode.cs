using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class GetResultValueNode : RuleNodeBase
    {

        public GetResultValueNode()
        {
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return ruleExecutionContext.Variables[VariableNames.RESULT_VALUE];
        }
    }
}