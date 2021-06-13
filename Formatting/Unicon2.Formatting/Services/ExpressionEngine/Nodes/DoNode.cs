using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class DoNode : IRuleNode
    {
        private readonly IEnumerable<IRuleNode> _ruleNodes;

        public DoNode(IEnumerable<IRuleNode> ruleNodes)
        {
            _ruleNodes = ruleNodes;
        }

        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            object res = null;
            foreach (var ruleNode in _ruleNodes)
            {
                res = await ruleNode.ExecuteNode(ruleExecutionContext);

            }
            return res;
        }
    }
}
