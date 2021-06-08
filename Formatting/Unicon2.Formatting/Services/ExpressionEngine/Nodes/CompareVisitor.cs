using Unicon2.Formatting.Services.ExpressionEngine.Common;

namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class CompareVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new CompareNode(LexemManager.GetNodeByString(get[0]), LexemManager.GetNodeByString(get[1]),
                get[2].GetOperatorType());
        }

        public CompareVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}