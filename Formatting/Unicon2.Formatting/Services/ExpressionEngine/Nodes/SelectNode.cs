using System;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SelectNode : IRuleNode
    {
        private readonly FormatterType _formatterType;

        public SelectNode(FormatterType formatterType)
        {
            _formatterType = formatterType;
        }

        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var res = ruleExecutionContext.Variables[VariableNames.RESULT_VALUE];
            switch (_formatterType)
            {
                case FormatterType.Number:
                    var numValue = StaticContainer.Container.Resolve<INumericValue>();
                    numValue.NumValue = (double) res;
                    ruleExecutionContext.SetVariable(VariableNames.RESULT_VALUE, numValue);
                    return res;
                case FormatterType.Bool:
                    var boolValue = StaticContainer.Container.Resolve<IBoolValue>();
                    boolValue.BoolValueProperty = (bool) res;
                    ruleExecutionContext.SetVariable(VariableNames.RESULT_VALUE, boolValue);
                    return res;
                case FormatterType.String:
                    var stringValue = StaticContainer.Container.Resolve<IStringValue>();
                    stringValue.StrValue = (string) res;
                    ruleExecutionContext.SetVariable(VariableNames.RESULT_VALUE, stringValue);
                    return res;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}