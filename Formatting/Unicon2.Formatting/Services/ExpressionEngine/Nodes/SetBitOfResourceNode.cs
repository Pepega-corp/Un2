using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SetBitOfResourceNode : RuleNodeBase
    {
        private string _resoure;
        private readonly IRuleNode _valueOfBit;
        private IRuleNode _numberOfBit;

        public SetBitOfResourceNode(IRuleNode numberOfBit, string resoure, IRuleNode valueOfBit)
        {
            _numberOfBit = numberOfBit;
            _resoure = resoure;
            _valueOfBit = valueOfBit;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return null;
        }
    }
}