using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Formatting.Services.ExpressionEngine
{
    public static class Evaluator
    {
        public static List<IRuleNode> Initialize(string expressionString, LexemManager lexemManager)
        {
            expressionString = RefineExpressionString(expressionString);

            var parts = expressionString.Split(new[] {"=>"}, StringSplitOptions.RemoveEmptyEntries);

            var res = new List<IRuleNode>();
            foreach (var part in parts)
            {
                res.Add(lexemManager.GetNodeByString(part));
            }

            return res;
        }

        private static string RefineExpressionString(string expressionString)
        {
            return expressionString.Replace(" ", string.Empty);

        }

        public static async Task<IFormattedValue> ExecuteFormat(ushort[] deviceValue,
            RuleExecutionContext ruleExecutionContext, List<IRuleNode> ruleNodes)
        {
            ruleExecutionContext.SetVariable(VariableNames.DEVICE_VALUE, deviceValue.ToList());
            foreach (var ruleNode in ruleNodes)
            {
                try
                {
                    await ruleNode.ExecuteNode(ruleExecutionContext);
                }
                catch (Exception e)
                {

                }
            }

            return ruleExecutionContext.GetVariable<IFormattedValue>(VariableNames.RESULT_VALUE);
        }

        public static async Task<ushort[]> ExecuteFormatBack(IFormattedValue userValue,
            RuleExecutionContext ruleExecutionContext, List<IRuleNode> ruleNodes)
        {
            ruleExecutionContext.SetVariable(VariableNames.INPUT_VALUE, userValue);
            foreach (var ruleNode in ruleNodes)
            {
                try
                {
                    await ruleNode.ExecuteNode(ruleExecutionContext);
                }
                catch (Exception e)
                {

                }
            }

            return ruleExecutionContext.GetVariable<List<ushort>>(VariableNames.DEVICE_VALUE).ToArray();
        }
    }
}