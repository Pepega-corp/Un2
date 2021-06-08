using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class PowVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new PowNode(LexemManager.GetNodeByString(get[0]), LexemManager.GetNodeByString(get[1]));
        }


        public PowVisitor(LexemManager lexemManager) : base(lexemManager)
        {
        }
    }
}