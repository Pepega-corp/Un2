using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class HideCurrentPropertyNode:IRuleNode
    {
        public HideCurrentPropertyNode()
        {
            
        }

        public Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            ruleExecutionContext.Variables.GetElement(VariableNames.CURRENT_VALUE_OWNER).OnSuccess(o =>
            {
                if (o is ICanBeHidden canBeHidden)
                {
                    canBeHidden.IsHidden = true;
                }
            });
            return null;
        }
    }
}
