using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class GetResultValueVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            return new GetResultValueNode();
        }


        public GetResultValueVisitor(LexemManager lexemManager) : base(lexemManager)
        {
        }
    }
}