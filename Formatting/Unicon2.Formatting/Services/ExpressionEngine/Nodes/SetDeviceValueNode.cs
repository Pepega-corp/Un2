using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SetDeviceValueNode : RuleNodeBase
    {
        private readonly IRuleNode _numberOfValue;
        private readonly IRuleNode _valueToSet;

        public SetDeviceValueNode(IRuleNode valueToSet, IRuleNode numberOfValue)
        {
            _valueToSet = valueToSet;
            _numberOfValue = numberOfValue;
        }


        public override async Task<object> ExecuteNode(RuleExecutionContext ruleExecutionContext)
        {
            var numberOfValue =
                Convert.ToInt32((double)await _numberOfValue.ExecuteNode(ruleExecutionContext));
            var deviceValue = new List<ushort>();
            if (ruleExecutionContext.Variables.ContainsKey(VariableNames.DEVICE_VALUE))
            {
                deviceValue = ruleExecutionContext.Variables[VariableNames.DEVICE_VALUE] as List<ushort>;
            }

            if (deviceValue.Count < numberOfValue + 1)
            {
                for (int i = 0; i <= numberOfValue; i++)
                {
                    if (deviceValue.Count < i + 1)
                    {
                        deviceValue.Add(0);
                    }
                }
            }

            deviceValue[numberOfValue] =
               Convert.ToUInt16((double)await _valueToSet.ExecuteNode(ruleExecutionContext));
            ruleExecutionContext.Variables[VariableNames.DEVICE_VALUE] = deviceValue;
            return deviceValue[numberOfValue];
        }
    }
}