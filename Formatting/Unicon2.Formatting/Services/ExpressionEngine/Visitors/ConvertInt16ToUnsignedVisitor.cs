using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class ConvertInt16ToUnsignedVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new ConvertInt16ToUnsignedNode(LexemManager.GetNodeByString(get[0]));
        }

        public ConvertInt16ToUnsignedVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}