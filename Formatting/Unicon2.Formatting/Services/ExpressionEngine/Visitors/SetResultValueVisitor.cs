using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class SetResultValueVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new SetResultValueNode(LexemManager.GetNodeByString(get[0]));
        }


        public SetResultValueVisitor(LexemManager lexemManager) : base(lexemManager)
        {
        }
    }
}