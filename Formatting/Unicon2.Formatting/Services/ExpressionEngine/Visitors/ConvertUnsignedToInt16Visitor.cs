using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class ConvertUnsignedToInt16Visitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new ConvertUnsignedToInt16Node(LexemManager.GetNodeByString(get[0]));
        }

        public ConvertUnsignedToInt16Visitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}