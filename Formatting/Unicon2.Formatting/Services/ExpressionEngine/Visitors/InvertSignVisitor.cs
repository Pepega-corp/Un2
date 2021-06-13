using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class InvertSignVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new InvertSignNode(LexemManager.GetNodeByString(get[0]));
        }

        public InvertSignVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}