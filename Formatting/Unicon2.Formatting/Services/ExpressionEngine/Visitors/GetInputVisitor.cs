using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class GetInputVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            return new GetInputNode();
        }


        public GetInputVisitor(LexemManager lexemManager) : base(lexemManager)
        {
        }
    }
}