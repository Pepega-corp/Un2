using System.Threading.Tasks;

namespace Unicon2.Formatting.Services.ExpressionEngine.Common
{
    public interface IRuleNode
    {
        Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext);
    }
}