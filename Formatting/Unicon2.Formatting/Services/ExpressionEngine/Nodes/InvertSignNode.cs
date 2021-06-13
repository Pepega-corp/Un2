using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
  public  class InvertSignNode:IRuleNode
    {
        private readonly IRuleNode _ruleNode;

        public InvertSignNode(IRuleNode ruleNode)
        {
            _ruleNode = ruleNode;
        }
        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            return -(double) await _ruleNode.ExecuteNode(ruleExecutionContext);
        }
    }
}
