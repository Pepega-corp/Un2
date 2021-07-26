using System;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SelectNode : IRuleNode
    {
        private readonly FormatterType _formatterType;
        private readonly int? _numberOfSymbolsAfterComma;

        public SelectNode(FormatterType formatterType,int? numberOfSymbolsAfterComma = null)
        {
            _formatterType = formatterType;
            _numberOfSymbolsAfterComma = numberOfSymbolsAfterComma;
        }

        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            Result<object> res= ruleExecutionContext.Variables.GetElement(VariableNames.RESULT_VALUE);
            switch (_formatterType)
            {
                case FormatterType.Number:
                    var numValue = StaticContainer.Container.Resolve<INumericValue>();
                    res.OnSuccess(o =>
                    {
                        if (_numberOfSymbolsAfterComma.HasValue)
                        {
                            var resNum = (double) o;
                            resNum = Math.Round(resNum, _numberOfSymbolsAfterComma.Value);
                            numValue.NumValue = resNum;
                        }
                        else
                        {
                            numValue.NumValue = (double)o;
                        }
                    });
                    ruleExecutionContext.SetVariable(VariableNames.RESULT_VALUE, numValue);
                    return res;
                case FormatterType.Bool:
                    var boolValue = StaticContainer.Container.Resolve<IBoolValue>();
                    res.OnSuccess(o => { boolValue.BoolValueProperty = (bool) o; });
                    ruleExecutionContext.SetVariable(VariableNames.RESULT_VALUE, boolValue);
                    return res;
                case FormatterType.String:
                    var stringValue = StaticContainer.Container.Resolve<IStringValue>();
                    res.OnSuccess(o => { stringValue.StrValue = (string) o; });
                    ruleExecutionContext.SetVariable(VariableNames.RESULT_VALUE, stringValue);
                    return res;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}