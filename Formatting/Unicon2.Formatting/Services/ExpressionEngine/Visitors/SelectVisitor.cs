using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class SelectVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new SelectNode(get[0].GetFormatterType());
        }

        public SelectVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}