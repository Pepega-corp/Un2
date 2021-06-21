using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class AddNode : RuleNodeBase
    {
        private readonly List<IRuleNode> _nodes;


        public AddNode(List<IRuleNode> nodes)
        {
            _nodes = nodes;
        }

        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            double res1 = 0;
          

            foreach (var node in _nodes)
            {
                res1 += Convert.ToDouble(await node.ExecuteNode(ruleExecutionContext));
            }

            return res1;
        }
    }
}