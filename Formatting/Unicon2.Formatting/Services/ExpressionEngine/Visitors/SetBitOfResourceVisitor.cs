using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class SetBitOfResourceVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new SetBitOfResourceNode(LexemManager.GetNodeByString(get[0]), get[1],
                LexemManager.GetNodeByString(get[2]));
        }

        public SetBitOfResourceVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}