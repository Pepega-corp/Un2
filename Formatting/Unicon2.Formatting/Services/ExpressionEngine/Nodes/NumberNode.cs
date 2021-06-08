using System.Threading.Tasks;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class NumberNode : RuleNodeBase
    {
        private readonly double _number;

        public NumberNode(double number)
        {
            _number = number;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return _number;
        }
    }
}