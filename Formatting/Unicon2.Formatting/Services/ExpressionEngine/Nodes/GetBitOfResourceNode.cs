using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class GetBitOfResourceNode : RuleNodeBase
    {
        private string _resoure;
        private IRuleNode _numberOfBit;

        public GetBitOfResourceNode(IRuleNode numberOfBit, string resoure)
        {
            _numberOfBit = numberOfBit;
            _resoure = resoure;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return null;
        }
    }
}