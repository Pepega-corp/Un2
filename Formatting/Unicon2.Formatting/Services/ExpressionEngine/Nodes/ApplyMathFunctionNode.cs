using System;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class ApplyMathFunctionNode : RuleNodeBase
    {
        private readonly Func<double, double> _func;

        public ApplyMathFunctionNode(Func<double, double> func)
        {
            _func = func;
        }

    }
}