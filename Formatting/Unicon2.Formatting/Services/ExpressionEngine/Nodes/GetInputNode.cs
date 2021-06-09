using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class GetInputNode : IRuleNode
    {
        public async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var inputFormattedValue = ruleExecutionContext.Variables.GetElement(VariableNames.INPUT_VALUE).Item as IFormattedValue;

            switch (inputFormattedValue)
            {
                case INumericValue numericValue :
                    return numericValue.NumValue;
                case IBoolValue boolValue :
                    return boolValue.BoolValueProperty;
                case IStringValue stringValue :
                    return stringValue.StrValue;
            }
            return null;
        }
    }
}