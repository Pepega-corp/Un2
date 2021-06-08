using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class IfVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            if (get.Count == 2)
            {
                return new IfNode(LexemManager.GetNodeByString(get[0]), LexemManager.GetNodeByString(get[1]));
            }

            return new IfNode(LexemManager.GetNodeByString(get[0]), LexemManager.GetNodeByString(get[1]),
                LexemManager.GetNodeByString(get[2]));
        }

        public IfVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}