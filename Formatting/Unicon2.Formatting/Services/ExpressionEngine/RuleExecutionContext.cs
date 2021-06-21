using System.Collections.Generic;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Formatting.Services.ExpressionEngine
{

    public class RuleExecutionContext
    {
      

        public bool IsLocal { get; }
        public Dictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        public DeviceContext DeviceContext { get; }

        public RuleExecutionContext(DeviceContext deviceContext, bool isLocal, params (string var, object varValue)[] variables)
        {
            DeviceContext = deviceContext;
            IsLocal = isLocal;
            foreach (var variable in variables)
            {
                Variables.AddElement(variable.var, variable.varValue);
            }
        }

        public RuleExecutionContext SetVariable(string name, object value)
        {
            Variables.AddElement(name,value);
            return this;
        }
        public T GetVariable<T>(string name)
        {
            return (T)Variables[name];
        }
    }
}