using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class GetVariableVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new GetBitOfResourceNode(LexemManager.GetNodeByString(get[0]), get[1]);
        }

        public GetVariableVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}