using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine
{
    public abstract class RuleNodeBase : IRuleNode
    {

        public virtual async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return ruleExecutionContext;
        }

    }
}