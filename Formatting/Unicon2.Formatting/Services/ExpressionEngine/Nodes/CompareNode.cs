using System;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class CompareNode : RuleNodeBase
    {
        private readonly OperatorType _operatorType;

        private IRuleNode _leftNode { get; }
        private IRuleNode _rightNode { get; }


        public CompareNode(IRuleNode leftNode, IRuleNode rightNode, OperatorType operatorType)
        {
            _operatorType = operatorType;
            _leftNode = leftNode;
            _rightNode = rightNode;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var res1 = (double)await _leftNode.ExecuteNode(ruleExecutionContext);
            var res2 = (double)await _rightNode.ExecuteNode(ruleExecutionContext);

            switch (_operatorType)
            {
                case OperatorType.More:
                    return res1 > res2;
                case OperatorType.Less:
                    return res1 < res2;
                case OperatorType.MoreOrEqual:
                    return res1 >= res2;
                case OperatorType.LessOrEqual:
                    return res1 <= res2;
                case OperatorType.Equal:
                    return Math.Abs(res1 - res2) < 0.00001;
                case OperatorType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }
    }
}